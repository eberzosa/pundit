using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Model.Xml;
using NuGet.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Mappings
{
   internal class PackageMappings
   {
      private static bool _registered;

      public static void XmlMappings()
      {
         if (_registered)
            return;

         _registered = true;

         Mapster.TypeAdapterConfig<PackageSpec, XmlPackageSpec>.NewConfig()
            .Map(dst => dst.Version, src => src.Version.ToString());

         Mapster.TypeAdapterConfig<XmlPackageSpec, PackageSpec>.NewConfig()
            .Map(dst => dst.Version, src => new NuGetVersion(src.Version));


         Mapster.TypeAdapterConfig<PackageManifest, XmlPackageManifest>.NewConfig()
            .Map(dst => dst.Version, src => src.Version.ToString());

         Mapster.TypeAdapterConfig<XmlPackageManifest, PackageManifest>.NewConfig()
            .Map(dst => dst.Version, src => new NuGetVersion(src.Version));


         Mapster.TypeAdapterConfig<PackageDependency, XmlPackageDependency>.NewConfig()
            .Map(dst => dst.VersionPattern, src => src.VersionPattern.OriginalString.TrimEnd('*', '.'));

         Mapster.TypeAdapterConfig<XmlPackageDependency, PackageDependency>.NewConfig()
            .ConstructUsing(xml => new PackageDependency(xml.PackageId, VersionUtils.GetRangeFromPuntitDependencyVersion(xml.VersionPattern)))
            .Ignore(src => src.PackageId, src => src.VersionPattern)
            .AfterMapping((src, dst) =>
            {
               if (src.DevTimeOnly)
               {
                  if (src.Scope == XmlDependencyScope.Normal)
                     dst.Scope = DependencyScope.Build;
               }
               else
               {
                  if (src.Scope != XmlDependencyScope.Normal)
                     dst.Scope = DependencyScope.Normal;
               }
            });

         Mapster.TypeAdapterConfig<SourceFiles, XmlSourceFiles>.NewConfig();
         Mapster.TypeAdapterConfig<XmlSourceFiles, SourceFiles>.NewConfig();
      }
   }
}
