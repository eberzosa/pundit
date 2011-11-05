using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   public interface ILocalRepository
   {
      /// <summary>
      /// Gets versions of the package in this repository which satisfy the version pattern
      /// </summary>
      /// <param name="package"></param>
      /// <param name="pattern"></param>
      /// <returns></returns>
      ICollection<Version> GetVersions(UnresolvedPackage package, VersionPattern pattern);

      /// <summary>
      /// Gets package manifest
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      Package GetManifest(PackageKey key);

      /// <summary>
      /// Determines if the binaries are downloaded into the local cache
      /// </summary>
      /// <param name="packages"></param>
      /// <returns></returns>
      ICollection<bool> BinariesExists(IEnumerable<PackageKey> packages);

      /// <summary>
      /// Performs search in the repository
      /// </summary>
      /// <param name="substring">search substring to search in package id. Case-insensitive.</param>
      /// <returns></returns>
      ICollection<PackageKey> Search(string substring);

      /// <summary>
      /// Saves package stream to local cache
      /// </summary>
      /// <param name="packageStream"></param>
      void Put(Stream packageStream);

      /// <summary>
      /// Gets the stream of local cached binary
      /// </summary>
      /// <param name="binaryKey"></param>
      /// <returns></returns>
      Stream Get(PackageKey binaryKey);
   }
}
