using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Pundit.Core.Model;

namespace Pundit.Server
{
   public class StandaloneHost : IDisposable
   {
      private readonly ServiceHost _host;
      private readonly ServiceEndpoint _endpoint;

      public StandaloneHost(int port = 8082)
      {
         string uri = "http://localhost:" + port + "/punditsrv";

         _host = new ServiceHost(typeof(RepositoryServer));
         _endpoint = _host.AddServiceEndpoint(typeof (IRemoteRepository),
                                              new WebHttpBinding(), uri);
         _endpoint.Behaviors.Add(new WebHttpBehavior());
      }

      public void Run()
      {
         _host.Open();
      }

      public void Dispose()
      {
         _host.Close();
         _host.Abort();
      }
   }
}
