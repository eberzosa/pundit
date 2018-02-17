using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using EBerzosa.Pundit.Core.Model;
using NuGet.Versioning;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Repository contract
   /// </summary>
   [ServiceContract]
   public interface IRepository
   {
      /// <summary>
      /// Publishes package in the repository. Only package stream should be passes as
      /// the rest of the info is alredy in the package's manifest.
      /// All the builds are removed and only this one is kept.
      /// </summary>
      /// <param name="packageStream">Package stream</param>
      [OperationContract]
      [WebInvoke(UriTemplate = "/publish")]
      void Publish(Stream packageStream);

      /// <summary>
      /// Downloads package
      /// </summary>
      /// <param name="key">Package unique identifier</param>
      /// <returns>Package stream</returns>
      [OperationContract]
      [FaultContract(typeof(FileNotFoundException))]
      [WebGet(UriTemplate = "/download")]
      Stream Download(PackageKey key);

      [OperationContract]
      [FaultContract(typeof(FileNotFoundException))]
      [WebGet(UriTemplate = "/versions")]
      NuGetVersion[] GetVersions(UnresolvedPackage package);

      [OperationContract]
      [FaultContract(typeof(FileNotFoundException))]
      [WebGet(UriTemplate = "/manifest")]
      PackageManifest GetManifest(PackageKey key);

      [OperationContract]
      [WebGet(UriTemplate = "/packagesexist")]
      bool[] PackagesExist(PackageKey[] packages);

      [OperationContract]
      [WebGet(UriTemplate = "/search?s={substring}")]
      PackageKey[] Search(string substring);
   }
}
