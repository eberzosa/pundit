using System;
using EBerzosa.Pundit.Core.Framework;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Model.Xml;
using EBerzosa.Pundit.Core.Versioning;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Mappings
{
   internal class XmlMappings
   {
      public const string NoArchName = "noarch";

      private static bool _registered;

      public static void Map()
      {
         if (_registered)
            return;

         _registered = true;

         Mapster.TypeAdapterConfig<PackageSpec, XmlPackageSpec>.NewConfig();
            //.Map(dst => dst.Version, src => src.Version.ToString())
            //.Map(dst => dst.Platform, src => ToPlatform(src.Framework));

         Mapster.TypeAdapterConfig<XmlPackageSpec, PackageSpec>.NewConfig();
            //.Map(dst => dst.Version, src => PunditVersion.Parse(src.Version))
            //.Map(dst => dst.Framework, src => ToFramework(src.Platform));


         Mapster.TypeAdapterConfig<PackageManifest, XmlPackageManifest>.NewConfig()
            .Map(dst => dst.Version, src => src.Version.ToString())
            .Map(dst => dst.Platform, src => src.Framework.GetShortFolderName());

         Mapster.TypeAdapterConfig<XmlPackageManifest, PackageManifest>.NewConfig()
            .Map(dst => dst.Version, src => PunditVersion.Parse(src.Version))
            .Map(dst => dst.Framework, src => PunditFramework.Parse(src.Platform));


         Mapster.TypeAdapterConfig<PackageDependency, XmlPackageDependency>.NewConfig()
            .Map(dst => dst.VersionPattern, src => src.AllowedVersions.OriginalVersion.Replace(".*", ""))
            .Map(dst => dst.Platform, src => src.Framework.GetShortFolderName());

         Mapster.TypeAdapterConfig<XmlPackageDependency, PackageDependency>.NewConfig()
            .ConstructUsing(xml => new PackageDependency(xml.PackageId, VersionUtils.ConvertPunditDependencyVersionToFloatVersion(xml.VersionPattern)))
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
      }
   }
}