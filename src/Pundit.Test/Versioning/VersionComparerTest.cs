using EBerzosa.Pundit.Core.Versioning;
using Xunit;

namespace Pundit.Test.Versioning
{
   public class VersionComparerTest
   {

      [Theory]
      [InlineData("1", "1", Equal)]
      [InlineData("1", "2", RightGreater)]
      [InlineData("2", "1", LeftGreater)]
      [InlineData("1.0", "1.0", Equal)]
      [InlineData("1.1", "1.1", Equal)]
      [InlineData("1.0", "1.1", RightGreater)]
      [InlineData("1.1", "1.2", RightGreater)]
      [InlineData("1.1", "1.0", LeftGreater)]
      [InlineData("1.2", "1.1", LeftGreater)]
      [InlineData("1.1.0", "1.1.0", Equal)]
      [InlineData("1.1.1", "1.1.1", Equal)]
      [InlineData("1.1.0", "1.1.1", RightGreater)]
      [InlineData("1.1.1", "1.1.2", RightGreater)]
      [InlineData("1.1.1", "1.1.0", LeftGreater)]
      [InlineData("1.1.2", "1.1.1", LeftGreater)]
      [InlineData("1.1.1.0", "1.1.1.0", Equal)]
      [InlineData("1.1.1.1", "1.1.1.1", Equal)]
      [InlineData("1.1.1.0", "1.1.1.1", RightGreater)]
      [InlineData("1.1.1.1", "1.1.1.2", RightGreater)]
      [InlineData("1.1.1.1", "1.1.1.0", LeftGreater)]
      [InlineData("1.1.1.2", "1.1.1.1", LeftGreater)]
      [InlineData("1.1.1-ABC", "1.1.1-ABC", Equal)]
      [InlineData("1.1.1-ABC.0", "1.1.1-ABC.0", Equal)]
      [InlineData("1.1.1-ABC.1", "1.1.1-ABC.1", Equal)]
      [InlineData("1.1.1-ABC", "1.1.1-ABC.1", RightGreater)]
      [InlineData("1.1.1-ABC.0", "1.1.1-ABC.1", RightGreater)]
      [InlineData("1.1.1-ABC.1", "1.1.1-ABC.2", RightGreater)]
      [InlineData("1.1.1-ABC.1", "1.1.1-ABC", LeftGreater)]
      [InlineData("1.1.1-ABC.1", "1.1.1-ABC.0", LeftGreater)]
      [InlineData("1.1.1-ABC.2", "1.1.1-ABC.1", LeftGreater)]
      [InlineData("1.1.1-ABC", "1.1.1", RightGreater)]
      [InlineData("1.1.1-ABC.0", "1.1.1", RightGreater)]
      [InlineData("1.1.1-ABC.1", "1.1.1", LeftGreater)]
      [InlineData("1.1.1-ABC", "1.1.0", LeftGreater)]
      [InlineData("1.1.1-ABC.0", "1.1.0", LeftGreater)]
      [InlineData("1.1.1-ABC.1", "1.1.0", LeftGreater)]
      [InlineData("1.1.2-ABC", "1.1.1", LeftGreater)]
      [InlineData("1.1.0", "1.1.1-ABC", RightGreater)]
      [InlineData("1.1.0", "1.1.1-ABC.0", RightGreater)]
      [InlineData("1.1.0", "1.1.1-ABC.1", RightGreater)]
      [InlineData("1.1.1", "1.1.2-ABC", RightGreater)]
      public void VersionRelease_Compare_Test(string v1, string v2, int expectedResult)
      {
         var version1 = NuGet.Versioning.NuGetVersion.Parse(v1);
         var version2 = NuGet.Versioning.NuGetVersion.Parse(v2);

         var result = VersionComparer.Compare(version1, version2, VersionComparison.VersionRelease);

         Assert.Equal(expectedResult, result);
      }

      public const int LeftGreater = 1;
      public const int RightGreater = -1;
      public const int Equal = 0;
   }
}
