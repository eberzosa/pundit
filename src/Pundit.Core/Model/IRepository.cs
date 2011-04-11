using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

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
      /// </summary>
      /// <param name="packageStream">Package stream</param>
      [OperationContract]
      void Publish(Stream packageStream);

      [OperationContract]
      IEnumerable<Package> Search(string nameSubstring);

      /// <summary>
      /// Downloads package
      /// </summary>
      /// <param name="key">Package unique identifier</param>
      /// <returns>Package stream</returns>
      [OperationContract]
      Stream Download(PackageKey key);

      [OperationContract]
      Version[] GetVersions(string packageId, string platform, VersionPattern pattern);

      [OperationContract]
      Package GetManifest(PackageKey key);
   }
}
