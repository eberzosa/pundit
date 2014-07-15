using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using RestSharp;

namespace Pundit.Core.Application.Repositories
{
   class NugetRepository : IRemoteRepository
   {
      private readonly RestClient _client;

      public NugetRepository()
      {
         _client = new RestClient("https://www.nuget.org/api/v2/");
      }

      public void Dispose()
      {
      }

      public void Publish(Stream packageStream)
      {
         throw new NotImplementedException();
      }

      public Stream Download(string platform, string packageId, string version)
      {
         throw new NotImplementedException();
      }

      public RemoteSnapshot GetSnapshot(string changeId)
      {
         //get count of packages
         long count = GetPackageCount();

         return null;
      }

      private void GetPackageDefinitions()
      {
         /* as seen capturing Xamarin Studio
          * 
          * GET /api/v2/Search()?$filter=IsLatestVersion&$orderby=DownloadCount%20desc&$skip=0&$top=30&searchTerm=''&targetFramework=''&includePrerelease=false HTTP/1.1
DataServiceVersion: 1.0;NetFx
MaxDataServiceVersion: 2.0;NetFx
User-Agent: Xamarin Studio/2.8.1.0 (Microsoft Windows NT 6.2.9200.0, Xamarin Studio/5.1.2.0)
Accept: application/atom+xml,application/xml
Accept-Charset: UTF-8
Host: www.nuget.org
Accept-Encoding: gzip, deflate
Connection: Keep-Alive
          * 
          * 
          * 
          */


      }

      private long GetPackageCount()
      {
         var request = CreateRequest("Search()/$count?$filter=IsLatestVersion&$orderby=DownloadCount%20desc&searchTerm=''&targetFramework=''&includePrerelease=false");
         IRestResponse response = _client.Execute(request);
         long count;
         long.TryParse(response.Content, out count);
         return count;
      }

      private RestRequest CreateRequest(string resource)
      {
         var request = new RestRequest(resource);
         request.AddHeader("DataServiceVersion", "2.0;NetFx");
         request.AddHeader("MaxDataServiceVersion", "2.0;NetFx");
         request.AddHeader("Accept-Charset", "UTF-8");
         request.AddHeader("Accept", "text/plain");
         return request;
      }
   }
}
