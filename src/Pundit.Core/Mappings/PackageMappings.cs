using System;
using EBerzosa.Pundit.Core.Model.Enums;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Model.Xml;
using ExpressMapper;
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

         Mapper.Register<PackageSpec, XmlPackageSpec>()
            .Member(dst => dst.Version, src => src.Version.ToString());
         Mapper.Register<XmlPackageSpec, PackageSpec>()
            .Member(dst => dst.Version, src => new Version(src.Version));


         Mapper.Register<PackageManifest, XmlPackageManifest>()
            .Member(dst => dst.Version, src => src.Version.ToString());

         Mapper.Register<XmlPackageManifest, PackageManifest>()
            .Member(dst => dst.Version, src => new Version(src.Version));


         Mapper.Register<PackageDependency, XmlPackageDependency>();
         Mapper.Register<XmlPackageDependency, PackageDependency>()

            .After((src, dst) =>
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

         Mapper.Register<SourceFiles, XmlSourceFiles>();
         Mapper.Register<XmlSourceFiles, SourceFiles>();

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
