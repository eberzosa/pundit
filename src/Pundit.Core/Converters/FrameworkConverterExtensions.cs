using System;
using EBerzosa.Pundit.Core.Framework;

namespace EBerzosa.Pundit.Core.Converters
{
   internal static class FrameworkConverterExtensions
   {
      public static NuGet.Frameworks.NuGetFramework ToNuGetFramework(this PunditFramework framework)
      {
         return !framework.IsLegacy
            ? NuGet.Frameworks.NuGetFramework.Parse(framework.GetShortFolderName())
            : throw new NotSupportedException();
      }

      public static PunditFramework ToPunditFramework(this NuGet.Frameworks.NuGetFramework framework)
      {
         return new PunditFramework(framework);
      }
   }
}
