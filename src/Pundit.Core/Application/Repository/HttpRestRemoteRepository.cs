using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using Pundit.Core.Exceptions;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Repository
{
   class HttpRestRemoteRepository : IRemoteRepository
   {
      private const string NullChangeId = "0";

      private readonly string _absoluteUri;
      private IRemoteRepository _channel;

      public HttpRestRemoteRepository(string absoluteUri)
      {
         _absoluteUri = absoluteUri;
         InitializeChannel();
      }

      private void InitializeChannel()
      {
         var binding = new WebHttpBinding
                          {
                             Security = {Mode = WebHttpSecurityMode.None},
                             TransferMode = TransferMode.Streamed,
                             MaxReceivedMessageSize = int.MaxValue
                          };

         var factory = new WebChannelFactory<IRemoteRepository>(
            binding, new Uri(_absoluteUri));

         _channel = factory.CreateChannel();
      }

      public void Publish(Stream packageStream)
      {
         _channel.Publish(packageStream);
      }

      public Stream Download(string platform, string packageId, string version)
      {
         return _channel.Download(platform, packageId, version);
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         try
         {
            return _channel.GetSnapshot(changeId ?? NullChangeId);
         }
         catch(EndpointNotFoundException)
         {
            throw new RepositoryOfflineException();
         }
      }
   }
}
