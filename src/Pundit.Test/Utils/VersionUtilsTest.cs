using EBerzosa.Pundit.Core;
using NuGet.Versioning;
using Xunit;

namespace Pundit.Test.Utils
{
   public class VersionUtilsTest
   {
      [Theory]
      [InlineData("1.2.3.4", "1.2.3.4")]
      [InlineData("1.2.3", "1.2.4")]
      [InlineData("1.2", "1.3")]
      [InlineData("1", "2")]
      [InlineData("1.2.3.0", "1.2.3.0")]
      [InlineData("1.2.0", "1.2.1")]
      [InlineData("1.0.0.0", "1.0.0.0")]
      [InlineData("1.0", "1.1")]
      [InlineData("1.2.3.4-dev", "1.2.3.4-dev")]
      [InlineData("1.2.3-dev", "1.2.4-dev")]
      [InlineData("1.2-dev", "1.3-dev")]
      [InlineData("1-dev", "2-dev")]
      [InlineData("1.2.3.0-dev", "1.2.3.0-dev")]
      [InlineData("1.2.0-dev", "1.2.1-dev")]
      [InlineData("1.0.0.0-dev", "1.0.0.0-dev")]
      [InlineData("1.0-dev", "1.1-dev")]
      public void GetRangeFromPuntitDependencyVersionTest(string version, string expectedMax)
      {
         var range = VersionUtils.GetRangeFromPuntitDependencyVersion(version);

         Assert.Equal(version, range.MinVersion.ToString());
         Assert.Equal(expectedMax, range.MaxVersion.ToString());
         Assert.Equal(version == expectedMax, range.IsMaxInclusive);
      }
   }
}
