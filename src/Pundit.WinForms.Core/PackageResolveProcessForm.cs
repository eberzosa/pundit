using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Pundit.Core;
using Pundit.Core.Application;
using Pundit.Core.Model;

namespace Pundit.WinForms.Core
{
   public partial class PackageResolveProcessForm : Form
   {
      private Thread _backgroundWorker;
      private string _manifestPath;
      private Exception _resolutionError;

      public PackageResolveProcessForm()
      {
         InitializeComponent();
      }

      public PackageResolveProcessForm(string manifestPath) : this()
      {
         _manifestPath = manifestPath;

         Resolve();
      }

      private void Resolve()
      {
         _backgroundWorker = new Thread(ResolveThread) { IsBackground = true };

         _backgroundWorker.Start();
      }

      private void SetStatusLine(string message)
      {
         if (InvokeRequired)
         {
            Invoke((Action)
                   delegate
                      {
                         lblProgress.Text = message;
                      });
         }
         else
         {
            lblProgress.Text = message;
         }

      }

      private void SetStatusImage(bool success)
      {
         Action code = delegate
                          {
                             if (success)
                                imgProgress.Image = Images.sun;
                             else
                                imgProgress.Image = Images.sad;
                          };

         if (InvokeRequired)
            Invoke(code);
         else
            code();
      }

      private void SetPackageStatus(string packageId, string version, string status, Image statusImage)
      {
         Action code = delegate
                          {
                             Image img = statusImage ?? Images.picture_empty;

                             gridPackages.Rows.Add(new object[] {img, packageId, version, status});
                          };

         if(InvokeRequired)
            Invoke(code);
         else
            code();
      }

      private void DisplayConflicts(VersionResolutionTable table)
      {
         foreach(UnresolvedPackage up in table.GetConflictedPackages())
         {
            SetPackageStatus(up.PackageId, "", "conflict", Images.flag_red);
         }
      }

      private void DisplaySuccess(VersionResolutionTable table)
      {
         foreach(PackageKey key in table.GetPackages())
         {
            SetPackageStatus(key.PackageId, key.Version.ToString(), "resolved", Images.flag_green);
         }
      }

      private void ResolveThread()
      {
         try
         {
            SetStatusLine(Strings.Resolve_ReadingManifest);

            DevPackage manifest;

            using (Stream s = File.OpenRead(_manifestPath))
            {
               manifest = DevPackage.FromStream(s);
            }

            SetStatusLine(Strings.Resolve_ReadingRepositories);

            var names = LocalRepository.TakeFirstRegisteredNames(int.MaxValue, true);

            IEnumerable<IRepository> repositories = names.Select(n =>
                                RepositoryFactory.CreateFromUri(
                                   LocalRepository.GetRepositoryUriFromName(n)));

            SetStatusLine(Strings.Resolve_Resolving);

            var dr = new DependencyResolution(manifest, repositories.ToArray());
            var resolutionResult = dr.Resolve();

            if (resolutionResult.Item1.HasConflicts)
            {
               DisplayConflicts(resolutionResult.Item1);
               SetStatusImage(false);
            }
            else
            {
               DisplaySuccess(resolutionResult.Item1);
               SetStatusImage(true);
            }

            SetStatusLine(Strings.Resolve_Success);
         }
         catch(Exception ex)
         {
            _resolutionError = ex;
         }
         finally
         {
            Invoke((Action)
                   delegate
                   {
                      cmdOk.Enabled = true;
                      cmdCancel.Enabled = false;
                   });            
         }

         if(_resolutionError != null)
         {
            Alert.Error(Strings.Resolve_MessageConflicts + Environment.NewLine + _resolutionError.Message);
         }
      }

      private void cmdCancel_Click(object sender, EventArgs e)
      {
         if(_backgroundWorker != null)
         {
            _backgroundWorker.Abort();

            lblProgress.Text = Strings.Resolve_Terminating;

            _backgroundWorker.Join();

            lblProgress.Text = Strings.Resolve_Terminated;

            cmdOk.Enabled = true;
            cmdCancel.Enabled = false;
         }

         Close();
      }

      private void gridPackages_CellContentClick(object sender, DataGridViewCellEventArgs e)
      {

      }
   }
}
