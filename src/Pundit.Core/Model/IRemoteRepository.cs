using System;
using System.Collections.Generic;
using System.IO;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Remote repository wire contract
   /// </summary>
   public interface IRemoteRepository : IDisposable
   {
      /// <summary>
      /// Publishes package in the repository. Only package stream should be passes as
      /// the rest of the info is alredy in the package's manifest.
      /// All the builds are removed and only this one is kept.
      /// </summary>
      /// <param name="packageStream">Package stream</param>
      void Publish(Stream packageStream);

      /// <summary>
      /// Downloads package
      /// </summary>
      /// <returns>Package stream</returns>
      Stream Download(string platform, string packageId, string version);

      /// <summary>
      /// Gets the snapshot of repository
      /// </summary>
      /// <param name="changeId">current change id or null for the full snapshot</param>
      /// <returns></returns>
      RemoteSnapshot GetSnapshot(string changeId);
   }
}
