using System;
using System.IO;
using System.Net;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Repository
{
   /// <summary>
   /// Client for IRemoteRepository
   /// </summary>
   class HttpRestRemoteRepository : IRemoteRepository
   {
      private const string NullChangeId = "0";
      private readonly Uri _absoluteUri;

      public HttpRestRemoteRepository(string absoluteUri)
      {
         if (absoluteUri == null) throw new ArgumentNullException("absoluteUri");
         if (!absoluteUri.EndsWith("/")) absoluteUri += "/";
         _absoluteUri = new Uri(absoluteUri);
      }

      public void Publish(Stream packageStream)
      {
         var request = (HttpWebRequest) WebRequest.Create(new Uri(_absoluteUri, "publish"));
         request.Method = "POST";
         using(var rs = request.GetRequestStream())
         {
            packageStream.CopyTo(rs);
         }
         request.GetResponse();
      }

      public Stream Download(string platform, string packageId, string version)
      {
         var request = (HttpWebRequest) WebRequest.Create(
            new Uri(_absoluteUri, string.Format("download/{0}/{1}/{2}", platform, packageId, version)));
         request.Method = "GET";
         var response = (HttpWebResponse) request.GetResponse();
         return response.GetResponseStream();
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         Uri uri = new Uri(_absoluteUri, string.Format("snapshot/{0}", changeId ?? NullChangeId));
         string xml = new WebClient().DownloadString(uri);
         return RemoteSnapshot.FromXml(xml);
      }

      #region Implementation of IDisposable

      public void Dispose()
      {
      }

      #endregion
   }
}
