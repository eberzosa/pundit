using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace Pundit.Core.Model
{
   [ServiceContract]
   public interface IRepository
   {
      [OperationContract]
      void Publish(Stream packageStream);

      [OperationContract]
      IEnumerable<Package> Search(string nameSubstring, VersionPattern minVersion);

      [OperationContract]
      Stream Download(string packageId, Version packageVersion);
   }
}
