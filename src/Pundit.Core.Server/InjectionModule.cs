using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Pundit.Core.Model;
using Pundit.Core.Server.Application;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server
{
   public class InjectionModule : NinjectModule
   {
      #region Overrides of NinjectModule

      public override void Load()
      {
         Bind<IConfigurationRepository>().To<MySqlConfigurationRepository>();

         Bind<IPackageRepository>().To<MySqlPackageRepository>();

         Bind<IRemoteRepository>().To<RemoteRepository>().InSingletonScope();
      }

      #endregion
   }
}
