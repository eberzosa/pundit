using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Pundit.Vsix.Resources;

namespace Pundit.Vsix.Application
{
   public enum StatusIcon
   {
      Empty,
      Green,
      Red,
      Yellow
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

      private readonly Dictionary<StatusIcon, Stream> _imageStreams = new Dictionary<StatusIcon, Stream>();

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

            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            _imageStreams[icon] = s;
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
            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = _imageStreams[value];
            img.EndInit();
            _statusImage.Source = new BitmapImage();
            _statusImage.Stretch = Stretch.Uniform;
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
