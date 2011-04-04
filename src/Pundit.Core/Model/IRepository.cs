using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace Pundit.Core.Model
{
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

      //[OperationContract]
      //bool[] Exists(string[] exactNames);

      [OperationContract]
      IEnumerable<Package> Search(string nameSubstring);

      /// <summary>
      /// Downloads package
      /// </summary>
      /// <param name="exactName">exact package name excluding the extension</param>
      /// <returns>Package stream</returns>
      [OperationContract]
      Stream Download(string exactName);
   }
}
