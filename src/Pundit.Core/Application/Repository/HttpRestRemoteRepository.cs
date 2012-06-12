using System;
using System.IO;
using Pundit.Core.Model;
using RestSharp;

namespace Pundit.Core.Application.Repository
{
   /// <summary>
   /// Client for IRemoteRepository
   /// </summary>
   class HttpRestRemoteRepository : IRemoteRepository
   {
      private const string NullChangeId = "0";

      private readonly string _absoluteUri;
      private readonly RestClient _client;

      public HttpRestRemoteRepository(string absoluteUri)
      {
         _absoluteUri = absoluteUri;
         _client = new RestClient(absoluteUri);
      }

      public void Publish(Stream packageStream)
      {
         var request = new RestRequest("publish", Method.POST);
         IRestResponse response = _client.Execute(request);
      }

      public Stream Download(string platform, string packageId, string version)
      {
         var request = new RestRequest("download/{platform}/{packageId}/{version}", Method.GET);
         request.AddParameter("platform", platform, ParameterType.UrlSegment);
         request.AddParameter("packageId", packageId, ParameterType.UrlSegment);
         request.AddParameter("version", version, ParameterType.UrlSegment);
         IRestResponse response = _client.Execute(request);

         throw new NotImplementedException();
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         var request = new RestRequest("snapshot/{changeId}", Method.GET);
         request.AddParameter("changeId", changeId ?? NullChangeId, ParameterType.UrlSegment);
         IRestResponse response = _client.Execute(request);
         return RemoteSnapshot.FromXml(response.Content);
      }

      #region Implementation of IDisposable

      public void Dispose()
      {
      }

      #endregion
   }
}
