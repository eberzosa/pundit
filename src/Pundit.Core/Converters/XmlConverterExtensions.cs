using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Model.Xml;
using EBerzosa.Pundit.Core.Package;
using EBerzosa.Pundit.Core.PackageManager.Xml;
using EBerzosa.Pundit.Core.Repository;
using EBerzosa.Pundit.Core.Repository.Xml;
using Mapster;
using Pundit.Core.Application;
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

      public static XmlPackageManifestRoot ToXmlPackageManifestRoot(this PackageManifestRoot packageManifestRoot)
      {
         return packageManifestRoot.Adapt<XmlPackageManifestRoot>();
      }

      public static PackageManifestRoot ToPackageManifestRoot(this XmlPackageManifestRoot xmlPackageManifestRoot)
      {
         return xmlPackageManifestRoot.Adapt<PackageManifestRoot>();
      }

      public static XmlPackageManifest ToXmlPackageManifest(this PackageManifest packageManifest)
      {
         return packageManifest.Adapt<XmlPackageManifest>();
      }

      public static PackageManifest ToPackageManifest(this XmlPackageManifest xmlPackageManifest)
      {
         return xmlPackageManifest.Adapt<PackageManifest>();
      }

      public static PackageManifestRoot ToPackageManifestRoot(this XmlPackageLegacyCrap xmlPackageLegacyCrap)
      {
         return xmlPackageLegacyCrap.Adapt<XmlPackageManifestRoot>().ToPackageManifestRoot();
      }

      public static PackageSpec ToPackageSpec(this XmlPackageLegacyCrap xmlPackageLegacyCrap)
      {
         return xmlPackageLegacyCrap.Adapt<XmlPackageSpec>().ToPackageSpec();
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

      public static InstalledPackagesIndex ToInstalledPackagesIndex(this XmlInstalledPackagesIndex xmlInstalledPackagesIndex, string filePath)
      {
         return xmlInstalledPackagesIndex.BuildAdapter().AddParameters("location", filePath).AdaptToType<InstalledPackagesIndex>();
            //.Adapt<InstalledPackagesIndex>();
      }

      public static XmlInstalledPackagesIndex ToXmlInstalledPackagesIndex(this InstalledPackagesIndex installedPackagesIndex)
      {
         return installedPackagesIndex.Adapt<XmlInstalledPackagesIndex>();
      }


      private static void RegisterMappings()
      {
         // Packages

         TypeAdapterConfig<PackageSpec, XmlPackageSpec>.NewConfig()
            .Map(dst => dst.Platform, src => src.Framework.GetShortFolderName());

         TypeAdapterConfig<XmlPackageSpec, PackageSpec>.NewConfig()
            .Map(dst => dst.Framework, src => PackageExtensions.GetFramework(src.Platform));

         TypeAdapterConfig<PackageManifestRoot, XmlPackageManifestRoot>.NewConfig()
            .Map(dst => dst.Platform, src => src.Framework.GetShortFolderName());

         TypeAdapterConfig<XmlPackageManifestRoot, PackageManifestRoot>.NewConfig()
            .Map(dst => dst.Framework, src => PackageExtensions.GetFramework(src.Platform));

         TypeAdapterConfig<PackageManifest, XmlPackageManifest>.NewConfig()
            .Include<PackageSpec, XmlPackageSpec>()
            .Include<PackageManifestRoot, XmlPackageManifestRoot>()
            .Map(dst => dst.Version, src => src.Version.ToString())
            .Map(dst => dst.Platform, src => src.LegacyFramework);

         TypeAdapterConfig<XmlPackageManifest, PackageManifest>.NewConfig()
            .Include<XmlPackageSpec, PackageSpec>()
            .Include<XmlPackageManifestRoot, PackageManifestRoot>()
            .Map(dst => dst.Version, src => NuGet.Versioning.NuGetVersion.Parse(src.Version))
            .Map(dst => dst.LegacyFramework, src => src.Platform);

         TypeAdapterConfig<XmlPackageLegacyCrap, XmlPackageManifestRoot>.NewConfig();
         TypeAdapterConfig<XmlPackageLegacyCrap, XmlPackageSpec>.NewConfig();


         TypeAdapterConfig<PackageDependency, XmlPackageDependency>.NewConfig()
            .Map(dst => dst.VersionPattern, src => src.AllowedVersions.OriginalString)
            .Map(dst => dst.Platform, src => src.Framework);

         TypeAdapterConfig<XmlPackageDependency, PackageDependency>.NewConfig()
            .ConstructUsing(xml => new PackageDependency(xml.PackageId, VersionConverterExtensions.ConvertPunditDependencyVersionToVersionRange(xml.VersionPattern)))
            .Ignore(dst => dst.PackageId, src => src.AllowedVersions)
            .Map(dst => dst.Framework, src => src.Platform)
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

         TypeAdapterConfig<SourceFiles, XmlSourceFiles>.NewConfig();
         TypeAdapterConfig<XmlSourceFiles, SourceFiles>.NewConfig();
         
         TypeAdapterConfig<XmlRegisteredRepositories, RegisteredRepositories>.NewConfig();
         TypeAdapterConfig<XmlRegisteredRepository, RegisteredRepository>.NewConfig();
         TypeAdapterConfig<XmlRepositoryType, RepositoryType>.NewConfig();


         // PackagesManager
         TypeAdapterConfig<XmlInstalledPackagesIndex, InstalledPackagesIndex>.NewConfig()
            .ConstructUsing(src => new InstalledPackagesIndex(MapContext.Current.Parameters["location"].ToString()));

         TypeAdapterConfig<InstalledPackagesIndex, XmlInstalledPackagesIndex>.NewConfig();

         TypeAdapterConfig<XmlPackageKey, PackageKey>.NewConfig()
            .ConstructUsing(src => new PackageKey(src.PackageId, NuGet.Versioning.NuGetVersion.Parse(src.Version), src.Framework));

         TypeAdapterConfig<PackageKey, XmlPackageKey>.NewConfig()
            .Map(dst => dst.Version, src => src.Version.OriginalVersion);
      }
   }
}