using System;
using System.IO;
using System.Web.Mvc;
using Pundit.Core.Server.Model;

namespace Pundit.Web.Controllers
{
   public class HomeController : Controller
   {
      private readonly IPackageRepository _packages;
      private readonly IConfigurationRepository _configuration;

      public HomeController(IPackageRepository packages, IConfigurationRepository configuration)
      {
         _packages = packages;
         _configuration = configuration;
      }

      private Version GetLatestVersion()
      {
         string txtPath = Server.MapPath("~/latest/version.txt");
         if(System.IO.File.Exists(txtPath))
         {
            string versionTxt = System.IO.File.ReadAllText(txtPath);
            Version v;
            if(Version.TryParse(versionTxt, out v))
            {
               return v;
            }
         }
         return null;
      }

      private FileInfo GetWindowsInstaller(Version v)
      {
         string path = Server.MapPath(string.Format("~/latest/pundit-setup-{0}.{1}.{2}.{3}.exe",
                                                    Math.Max(0, v.Major),
                                                    Math.Max(0, v.Minor),
                                                    Math.Max(0, v.Build),
                                                    Math.Max(0, v.Revision)));
         if(System.IO.File.Exists(path))
         {
            return new FileInfo(path);
         }

         return null;
      }

      public ActionResult Index()
      {
         Version v = GetLatestVersion();
         if (v != null)
         {
            ViewBag.LatestVersion = v;
            ViewBag.WindowsInstaller = GetWindowsInstaller(v);
            ViewBag.WindowsInstallerDownloads = _configuration.Get("WindowsInstallerDownloads") ?? "0";
         }

         return View();
      }

      public FileResult DownloadWindowsInstaller()
      {
         Version v = GetLatestVersion();
         if(v != null)
         {
            FileInfo fi = GetWindowsInstaller(v);
            if (fi != null)
            {
               string s = _configuration.Get("WindowsInstallerDownloads");
               int n;
               int.TryParse(s, out n);
               n += 1;
               _configuration.Set("WindowsInstallerDownloads", n.ToString());

               return File(System.IO.File.OpenRead(fi.FullName), "application/x-msdownload", fi.Name);
            }
         }

         return null;
      }
   }
}
