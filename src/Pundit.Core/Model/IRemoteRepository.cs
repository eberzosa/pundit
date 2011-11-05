using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Remote repository wire contract
   /// </summary>
   [ServiceContract]
   public interface IRemoteRepository
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

      /// <summary>
      /// Gets the snapshot of repository
      /// </summary>
      /// <param name="changeId"></param>
      /// <param name="nextChangeId">change id used in next call to get only the package changes. When 0 returned
      /// all of the local shapshot must be rewritten by the received one</param>
      /// <returns></returns>
      [OperationContract]
      [FaultContract(typeof(FileNotFoundException))]
      [WebGet(UriTemplate = "/snapshot/{changeId}")]
      PackageSnapshotKey[] GetSnapshot(string changeId, out string nextChangeId);
   }
}
