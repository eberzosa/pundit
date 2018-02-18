using System;
using EBerzosa.Pundit.Core.Model;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class LocationInfo : IComparable<LocationInfo>
   {
      public NuGetVersion Version { get; set; }

      public IRepository Repo { get; set; }

      public IDependencyResolver Resolver { get; set; }


      public LocationInfo(NuGetVersion version, IRepository repo, IDependencyResolver resolver)
      {
         Version = version;
         Repo = repo;
         Resolver = resolver;
      }

      public int CompareTo(LocationInfo other)
      {
         return Version.CompareTo(other.Version);
      }
   }
}
