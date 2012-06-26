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
      public override void Load()
      {
         Bind<IConfigurationRepository>().To<MySqlConfigurationRepository>().InThreadScope();
         Bind<IPackageRepository>().To<MySqlPackageRepository>().InThreadScope();
         Bind<IUserRepository>().To<MySqlUserRepository>().InThreadScope();
         Bind<IRemoteRepository>().To<RemoteRepository>().InSingletonScope();
      }
   }
}
