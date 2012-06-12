using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Pundit.Core.Server;

namespace Pundit.Web
{
   // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
   // visit http://go.microsoft.com/?LinkId=9394801

   public class MvcApplication : NinjectHttpApplication
   {
      public static void RegisterGlobalFilters(GlobalFilterCollection filters)
      {
         filters.Add(new HandleErrorAttribute());
      }

      public static void RegisterRoutes(RouteCollection routes)
      {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         routes.MapRoute(
            "IRemoteRepository-Snapshot",
            "repository/v1/snapshot/{delta}",
            new {controller = "RemoteRepository", action = "GetSnapshot"});

         routes.MapRoute(
            "IRemoteRepository-Download",
            "repository/v1/download/{platform}/{packageId}/{version}",
            new {controller = "RemoteRepository", action = "Download"});

         routes.MapRoute(
            "IRemoteRepository-Publish",
            "repository/v1/publish",
            new {controller = "RemoteRepository", action = "Publish"});

         routes.MapRoute(
             "Default", // Route name
             "{controller}/{action}/{id}", // URL with parameters
             new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
         );

      }

      protected override void OnApplicationStarted()
      {
         base.OnApplicationStarted();

         AreaRegistration.RegisterAllAreas();

         RegisterGlobalFilters(GlobalFilters.Filters);
         RegisterRoutes(RouteTable.Routes);
      }

      protected override IKernel CreateKernel()
      {
         return new StandardKernel(new InjectionModule());
      }
   }
}