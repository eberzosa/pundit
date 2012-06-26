using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Pundit.Test.Import
{
   //those are only a bunch or raw experiments for now

   [TestFixture, Ignore]
   class NuGetImporterTest
   {
      [Test]
      public void ImportListTest()
      {
         var client = new RestClient("https://nuget.org/api/v2");
         //first page only
         var request =
            new RestRequest(
               "Packages()?$orderby=Id&$filter=IsLatestVersion",  
               Method.GET);
         IRestResponse response = client.Execute(request);
         JObject all = JObject.Parse(response.Content);
      }
   }
}
