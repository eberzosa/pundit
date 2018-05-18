using System;

namespace EBerzosa.Pundit.Core.Versioning
{
   internal static class VersionUtils
   {
      public static FloatRange ConvertPunditDependencyVersionToFloatVersion(string punditDependencyVersion)
      {
         if (punditDependencyVersion.Contains("*"))
            throw new NotSupportedException("Pundit versions cannot contain *");

         var parts = punditDependencyVersion.Split('.');

         if (parts.Length > 4 || parts.Length < 1)
            throw new NotSupportedException($"Version '{punditDependencyVersion}' is not supported");

         if (punditDependencyVersion.IndexOf('-') != -1)
            throw new NotSupportedException("Versions cannot contain Release part");

         return parts.Length == 4
            ? FloatRange.Parse(punditDependencyVersion)
            : FloatRange.Parse(punditDependencyVersion + ".*");
      }

      public static FloatRange ToFloatRange(this PunditVersion version) 
         => new FloatRange(FloatBehaviour.None, version);
   }
}
