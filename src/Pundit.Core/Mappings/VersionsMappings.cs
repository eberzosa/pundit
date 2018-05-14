using EBerzosa.Pundit.Core.Versioning;
using Mapster;
using FloatRange = EBerzosa.Pundit.Core.Versioning.FloatRange;
using VersionRange = EBerzosa.Pundit.Core.Versioning.VersionRange;

namespace EBerzosa.Pundit.Core.Mappings
{
   internal class VersionsMappings
   {
      private static bool _registered;

      public static void Map()
      {
         if (_registered)
            return;

         _registered = true;

         Mapster.TypeAdapterConfig<VersionRange, NuGet.Versioning.VersionRange>.NewConfig()
            .ConstructUsing(r => new NuGet.Versioning.VersionRange(
               r.MinVersion.Adapt<NuGet.Versioning.NuGetVersion>(), r.IsMinInclusive, 
               r.MaxVersion.Adapt<NuGet.Versioning.NuGetVersion>(), r.IsMaxInclusive, 
               r.Float.Adapt<NuGet.Versioning.FloatRange>(), 
               r.OriginalString));

         Mapster.TypeAdapterConfig<PunditVersion, NuGet.Versioning.NuGetVersion>.NewConfig()
            .ConstructUsing(v => new NuGet.Versioning.NuGetVersion(
               v.Version, v.ReleaseLabels, v.Metadata, v.OriginalVersion));

         Mapster.TypeAdapterConfig<FloatRange, NuGet.Versioning.FloatRange>.NewConfig()
            .ConstructUsing(r => new NuGet.Versioning.FloatRange(
               r.FloatBehaviour.Adapt<NuGet.Versioning.NuGetVersionFloatBehavior>(), 
               r.MinVersion.Adapt<NuGet.Versioning.NuGetVersion>(), 
               r.OriginalReleasePrefix));
         
         Mapster.TypeAdapterConfig<FloatBehaviour, NuGet.Versioning.NuGetVersionFloatBehavior>.NewConfig();


         Mapster.TypeAdapterConfig<NuGet.Versioning.VersionRange, VersionRange>.NewConfig()
            .ConstructUsing(r => new VersionRange(
               r.MinVersion.Adapt<PunditVersion>(), r.IsMinInclusive,
               r.MaxVersion.Adapt<PunditVersion>(), r.IsMaxInclusive,
               r.Float.Adapt<FloatRange>(),
               r.OriginalString))
            ;
         Mapster.TypeAdapterConfig<NuGet.Versioning.NuGetVersion, PunditVersion>.NewConfig()
            .ConstructUsing(v => new PunditVersion(
               v.Version, v.ReleaseLabels, v.Metadata, v.OriginalVersion));

         Mapster.TypeAdapterConfig<NuGet.Versioning.FloatRange, FloatRange>.NewConfig()
            .ConstructUsing(r => new FloatRange(
               r.FloatBehavior.Adapt<FloatBehaviour>(),
               r.MinVersion.Adapt<PunditVersion>(), 
               r.OriginalReleasePrefix));

         Mapster.TypeAdapterConfig<NuGet.Versioning.NuGetVersionFloatBehavior, FloatBehaviour>.NewConfig();
      }
   }
}