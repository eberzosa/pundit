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
            .ConstructUsing(xml => new PackageDependency(xml.PackageId, VersionRange.Parse(VersionUtils.MakeFloatVersionString(xml.VersionPattern))))
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

         //Mapper.Register<BuildConfiguration, XmlBuildConfiguration>()
         //   .Function(dst => dst, src => (XmlBuildConfiguration)src);
         //Mapper.Register<XmlBuildConfiguration, BuildConfiguration>()
         //   .Function(dst => dst, src => (BuildConfiguration)src);

         //Mapper.Register<BuildConfiguration, XmlDependencyScope>()
         //   .Function(dst => dst, src => (XmlDependencyScope)src);
         //Mapper.Register<XmlDependencyScope, BuildConfiguration>()
         //   .Function(dst => dst, src => (DependencyScope)src);

         //Mapper.Register<PackageFileKind, XmlPackageFileKind>()
         //   .Function(dst => dst, src => (XmlPackageFileKind)src);
         //Mapper.Register<XmlPackageFileKind, PackageFileKind>()
         //   .Function(dst => dst, src => (PackageFileKind)src);
      }
   }
}
