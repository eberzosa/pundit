using System;
using System.Linq;
using EBerzosa.Pundit.Core.Mappings;
using Xunit;

namespace Pundit.Test.Mappings
{
   public class NuGetv3PackageMappingsTest
   {
      [Theory]
      [InlineData("1.2")]
      [InlineData("1.3.4")]
      [InlineData("1.4.40")]
      [InlineData("1.4.5.6")]
      [InlineData("1.4.5.60")]
      public void Map_VersionString_To_NuGet_VersionRange(string version)
      {
         var nuGetVersion = NuGetv3PackageMappings.PunditStringVersionToNugetVersionRange(version);
         
         Assert.Equal(version.ToFullVersion(), nuGetVersion.MinVersion.Version);
         Assert.Equal(true, nuGetVersion.IsMinInclusive);

         Assert.Equal(version.ToLastVersion().ToFullVersion(), nuGetVersion.MaxVersion.Version);
         Assert.Equal(false, nuGetVersion.IsMaxInclusive);
      }

   }

   internal static class ExtensionMethods
   {
      public static int ToInt(this string number) => int.Parse(number);

      public static Version ToFullVersion(this string version)
      {
         var outVersion = version;
         for (int i = version.Count(c => c == '.'); i < 3; i++)
            outVersion += ".0";

         return new Version(outVersion);
      }

      public static string ToLastVersion(this string version)
      {
         var splitVersion = version.Split('.');

         var last = splitVersion[splitVersion.Length - 1];

         Array.Resize(ref splitVersion, splitVersion.Length - 1);

         var maxVersion = splitVersion.Aggregate("", (a, s) => a + s + ".");
         maxVersion += int.Parse(last) + 1;

         return maxVersion;
      }
   }
}
