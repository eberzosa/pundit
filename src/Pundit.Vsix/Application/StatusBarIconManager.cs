using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Pundit.Vsix.Resources;

namespace Pundit.Vsix.Application
{
   public enum StatusIcon : ulong
   {
      Empty,
      Green    = 0xb2c78b,
      Red      = 0xf5863d,
      Yellow   = 0xfddd74
   }

   /// <summary>
   /// This is a pure hack created after decompiling VisualSVN with Reflector. May not work in the next version of Visual Studio.
   /// </summary>
   class StatusBarIconManager
   {
      private DockPanel _statusBarPanel;
      private StackPanel _indicatorPanel;
      private Label _statusText;
      private Image _statusImage;

      private readonly Dictionary<StatusIcon, BitmapImage> _statusImages = new Dictionary<StatusIcon, BitmapImage>();

      public StatusBarIconManager()
      {
         Connect();
         InitImages();
      }

      private void Connect()
      {
         UIElement statusBar = GetStatusBar(System.Windows.Application.Current.MainWindow);
         _statusBarPanel = GetStatusBarPanel(statusBar);
         _indicatorPanel = new StackPanel();
         _indicatorPanel.Orientation = Orientation.Horizontal;
         _indicatorPanel.VerticalAlignment = VerticalAlignment.Bottom;
         DockPanel.SetDock(_indicatorPanel, Dock.Right);
         _statusBarPanel.Children.Insert(_statusBarPanel.Children.IndexOf(statusBar), _indicatorPanel);

         _statusText = new Label();
         _statusText.Content = "";
         _indicatorPanel.Children.Add(_statusText);

         _statusImage = new Image();
         _statusImage.Width = 16;
         _statusImage.Height = 16;
         _indicatorPanel.Children.Add(_statusImage);
      }

      private void InitImages()
      {
         foreach(StatusIcon icon in Enum.GetValues(typeof(StatusIcon)))
         {
            string resourceName = string.Format("{0}.Resources.Status.{1}.png",
                                                typeof (PunditPackage).Namespace,
                                                icon.ToString().ToLower());

            using(Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
               if (s != null)
               {
                  var ms = new MemoryStream();
                  s.CopyTo(ms);
                  s.Flush();
                  ms.Position = 0;

                  var img = new BitmapImage();
                  img.BeginInit();
                  img.StreamSource = ms;
                  img.EndInit();
                  _statusImages[icon] = img;
               }
            }
         }
      }

      public string StatusText
      {
         get { return _statusText.Content as string; }
         set { _statusText.Content = value; }
      }

      public StatusIcon StatusIcon
      {
         set
         {
            if(_statusImages.ContainsKey(value))
            {
               _statusImage.Source = _statusImages[value];
               string sc = string.Format("#{0:X}", (ulong) value);
               _statusText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sc));
               _statusImage.ToolTip = "pundit (work in progress)";
            }
            else
            {
               _statusImage.Source = null;
            }
         }
      }

      private UIElement GetStatusBar(DependencyObject mainWindow)
      {
         DependencyObject statusBar = FindWpfStatusBar(mainWindow);
         if (statusBar == null) throw new Exception(VSPackage.Ex_CantFindStatusBar);
         var sbe = statusBar as UIElement;
         if(sbe == null) throw new InvalidCastException(VSPackage.Ex_CantFindStatusBar);
         return sbe;
      }

      private static DependencyObject FindWpfStatusBar(DependencyObject container)
      {
         for (int i = 0; i < VisualTreeHelper.GetChildrenCount(container); i++)
         {
            DependencyObject child = VisualTreeHelper.GetChild(container, i);
            if (child.GetType().Name == "WorkerThreadStatusBarContainer")
            {
               return child;
            }
            DependencyObject obj3 = FindWpfStatusBar(child);
            if (obj3 != null)
            {
               return obj3;
            }
         }
         return null;
      }

      private static DockPanel GetStatusBarPanel(UIElement statusBar)
      {
         return (DockPanel)VisualTreeHelper.GetParent(statusBar);
      } 
   }
}
