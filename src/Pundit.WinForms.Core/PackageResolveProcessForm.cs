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
      private readonly string _manifestPath;
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

                             DataGridViewRow row = null;

                             foreach(DataGridViewRow irow in gridPackages.Rows)
                             {
                                if(irow.Cells[1].Value.ToString() == packageId)
                                {
                                   row = irow;
                                   break;
                                }
                             }

                             if (row != null)
                             {
                                row.Cells[0].Value = img;
                                row.Cells[1].Value = packageId;
                                row.Cells[2].Value = version;
                                row.Cells[3].Value = status;
                             }
                             else
                             {
                                gridPackages.Rows.Add(new object[] {img, packageId, version, status});
                             }
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

            //read manifest

            using (Stream s = File.OpenRead(_manifestPath))
            {
               manifest = DevPackage.FromStream(s);
            }

            SetStatusLine(Strings.Resolve_ReadingRepositories);

            var names = LocalRepository.TakeFirstRegisteredNames(int.MaxValue, true);

            IEnumerable<IRepository> repositories = names.Select(n =>
                                RepositoryFactory.CreateFromUri(
                                   LocalRepository.GetRepositoryUriFromName(n)));

            //resolve

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

            //download missing packages

            SetStatusLine(Strings.Resolve_Downloading);

            LocalRepository.PackageDownloadToLocalRepositoryStarted += LocalRepositoryPackageDownloadToLocalRepositoryStarted;
            LocalRepository.PackageDownloadToLocalRepositoryFinished += LocalRepositoryPackageDownloadToLocalRepositoryFinished;
            LocalRepository.DownloadLocally(resolutionResult.Item1.GetPackages(), repositories.Skip(1));

            //install packages

            using (PackageInstaller installer = new PackageInstaller(
               new FileInfo(_manifestPath).Directory.FullName,
               resolutionResult.Item1,
               manifest,
               repositories.First()))
            {
               installer.BeginInstallPackage += new EventHandler<Pundit.Core.Model.EventArguments.PackageKeyDiffEventArgs>(InstallerBeginInstallPackage);
               installer.FinishInstallPackage += new EventHandler<Pundit.Core.Model.EventArguments.PackageKeyDiffEventArgs>(InstallerFinishInstallPackage);

               IEnumerable<PackageKeyDiff> diff = installer.GetDiffWithCurrent(resolutionResult.Item1.GetPackages());

               installer.Upgrade(BuildConfiguration.Release, diff);
            }

            //report success

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
#if DEBUG
            Alert.Error(Strings.Resolve_MessageConflicts + Environment.NewLine + _resolutionError.Message + _resolutionError.StackTrace);
#else
            Alert.Error(Strings.Resolve_MessageConflicts + Environment.NewLine + _resolutionError.Message);
#endif
         }
      }

      void InstallerFinishInstallPackage(object sender, Pundit.Core.Model.EventArguments.PackageKeyDiffEventArgs e)
      {
         SetPackageStatus(e.PackageKeyDiff.PackageId, e.PackageKeyDiff.Version.ToString(), "installed", Images.flag_green);
      }

      void InstallerBeginInstallPackage(object sender, Pundit.Core.Model.EventArguments.PackageKeyDiffEventArgs e)
      {
         SetPackageStatus(e.PackageKeyDiff.PackageId, e.PackageKeyDiff.Version.ToString(), "installing", Images.disk);
      }

      void LocalRepositoryPackageDownloadToLocalRepositoryFinished(object sender, Pundit.Core.Model.EventArguments.PackageKeyEventArgs e)
      {
         SetPackageStatus(e.PackageKey.PackageId, e.PackageKey.Version.ToString(), "downloaded", Images.flag_green);
      }

      void LocalRepositoryPackageDownloadToLocalRepositoryStarted(object sender, Pundit.Core.Model.EventArguments.PackageKeyEventArgs e)
      {
         SetPackageStatus(e.PackageKey.PackageId, e.PackageKey.Version.ToString(), "downloading", Images.arrow_refresh_small);
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
   }
}
