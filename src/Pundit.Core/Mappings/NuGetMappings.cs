using EBerzosa.Pundit.Core.Framework;
using NuGet.Frameworks;

namespace EBerzosa.Pundit.Core.Mappings
{
   internal class NuGetMappings
   {
      private static bool _registered;

      public static void Map()
      {
         if (_registered)
            return;

         _registered = true;

         Mapster.TypeAdapterConfig<NuGetFramework, PunditFramework>.NewConfig()
            .MapWith(src => new PunditFramework(src));

         Mapster.TypeAdapterConfig<PunditFramework, NuGetFramework>.NewConfig()
            .MapWith(src => !src.IsLegacy ? new NuGetFramework(src.NuGetFramework) : null);
      }
   }
}
