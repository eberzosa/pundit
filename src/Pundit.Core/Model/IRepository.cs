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
      /// All the builds are removed and only this one is kept.
      /// </summary>
      /// <param name="packageStream">Package stream</param>
      [OperationContract]
      void Publish(Stream packageStream);

      /// <summary>
      /// Downloads package
      /// </summary>
      /// <param name="key">Package unique identifier</param>
      /// <returns>Package stream</returns>
      [OperationContract]
      [FaultContract(typeof(FileNotFoundException))]
      Stream Download(PackageKey key);

      [OperationContract]
      [FaultContract(typeof(FileNotFoundException))]
      Version[] GetVersions(UnresolvedPackage package, VersionPattern pattern);

      [OperationContract]
      [FaultContract(typeof(FileNotFoundException))]
      Package GetManifest(PackageKey key);

      [OperationContract]
      bool[] PackagesExist(PackageKey[] packages);
   }
}
