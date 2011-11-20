using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Pundit.Core.Model;
using log4net;

namespace Pundit.Server
{
   public class StandaloneHost : IDisposable
   {
      private readonly ILog _log = LogManager.GetLogger(typeof (StandaloneHost));
      private readonly ServiceHost _host;
      private readonly ServiceEndpoint _endpoint;

      public StandaloneHost(int port = 8082)
      {
         string uri = "http://localhost:" + port + "/punditsrv";

         _log.Info("starting server on address: " + uri);

         var binding = new WebHttpBinding
                          {
                             Security = {Mode = WebHttpSecurityMode.None},
                             TransferMode = TransferMode.Streamed,
                             MaxReceivedMessageSize = int.MaxValue
                          };
         _host = new ServiceHost(typeof(RepositoryServer));
         _endpoint = _host.AddServiceEndpoint(typeof (IRemoteRepository), binding, uri);
         _endpoint.Behaviors.Add(new WebHttpBehavior());
      }

      public void Run()
      {
         _host.Open();
         _log.Info("server is running");
      }

      public void Dispose()
      {
         _log.Info("shutting down");
         _host.Close();
         _host.Abort();
      }
   }
}
