using System.Collections.Generic;
using System.ServiceModel;

namespace NGem.Core.Model
{
   [ServiceContract]
   public interface IRepository
   {
      [OperationContract]
      IEnumerable<Package> SearchPackage(string nameSubstring, VersionPattern minVersion);


   }
}
