using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Model.Xml;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Repository.Xml;
using EBerzosa.Pundit.Core.Versioning;
using Mapster;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Converters
{
   internal static class XmlConverterExtensions
   {
      public const string NoArchName = "noarch";

      static XmlConverterExtensions()
      {
         RegisterMappings();
      }

      public static XmlPackageSpec ToXmlPackageSpec(this PackageSpec packageSpec)
      {
         return packageSpec.Adapt<XmlPackageSpec>();
      }

      public static PackageSpec ToPackageSpec(this XmlPackageSpec xmlPackageSpec)
      {
         return xmlPackageSpec.Adapt<PackageSpec>();
      }

      public static XmlPackageManifest ToXmlPackageManifest(this PackageManifest packageManifest)
      {
         return packageManifest.Adapt<XmlPackageManifest>();
      }

      public static PackageManifest ToPackageManifest(this XmlPackageManifest xmlPackageManifest)
      {
         return xmlPackageManifest.Adapt<PackageManifest>();
      }

      public static XmlPackageDependency ToXmlPackageDependency(this PackageDependency packageDependency)
      {
         return packageDependency.Adapt<XmlPackageDependency>();
      }

      public static PackageDependency ToPackageDependency(this XmlPackageDependency xmlPackageDependency)
      {
         return xmlPackageDependency.Adapt<PackageDependency>();
      }

      public static RegisteredRepositories ToRegisteredRepositories(this XmlRegisteredRepositories xmlRegisteredRepositories)
      {
         return xmlRegisteredRepositories.Adapt<RegisteredRepositories>();
      }


      private static void RegisterMappings()
      {
         Mapster.TypeAdapterConfig<PackageSpec, XmlPackageSpec>.NewConfig();
         Mapster.TypeAdapterConfig<XmlPackageSpec, PackageSpec>.NewConfig();

         Mapster.TypeAdapterConfig<PackageManifest, XmlPackageManifest>.NewConfig()
            .Include<PackageSpec, XmlPackageSpec>()
            .Map(dst => dst.Version, src => src.Version.ToString())
            .Map(dst => dst.Platform, src => src.Framework.GetShortFolderName());

         Mapster.TypeAdapterConfig<XmlPackageManifest, PackageManifest>.NewConfig()
            .Include<XmlPackageSpec, PackageSpec>()
            .Map(dst => dst.Version, src => NuGet.Versioning.NuGetVersion.Parse(src.Version))
            .Map(dst => dst.Framework, src => PunditFramework.Parse(src.Platform));


         Mapster.TypeAdapterConfig<PackageDependency, XmlPackageDependency>.NewConfig()
            .Map(dst => dst.VersionPattern, src => src.AllowedVersions.NuGetVersionRange.OriginalString)
            .Map(dst => dst.Platform, src => src.Framework.GetShortFolderName());

         Mapster.TypeAdapterConfig<XmlPackageDependency, PackageDependency>.NewConfig()
            .ConstructUsing(xml => new PackageDependency(xml.PackageId, VersionConverterExtensions.ConvertPunditDependencyVersionToVersionRangeExtended(xml.VersionPattern)))
            .Ignore(dst => dst.PackageId, src => src.AllowedVersions)
            .Map(dst => dst.Framework, src => PunditFramework.Parse(src.Platform))
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
         
         Mapster.TypeAdapterConfig<XmlRegisteredRepositories, RegisteredRepositories>.NewConfig();
         Mapster.TypeAdapterConfig<XmlRegisteredRepository, RegisteredRepository>.NewConfig();
         Mapster.TypeAdapterConfig<XmlRepositoryType, RepositoryType>.NewConfig();
      }
   }
}