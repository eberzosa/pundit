using EBerzosa.Pundit.Core;
using Xunit;

namespace Pundit.Test.Utils
{
   public class VersionUtilsTest
   {
      [Theory]
      [InlineData("1.2.3.4",     "1.2.3.4")]
      [InlineData("1.2.3",       "1.2.3.*")]
      [InlineData("1.2",         "1.2.*")]
      [InlineData("1",           "1.*")]
      [InlineData("1.2.3.4-dev", "1.2.3.4-dev")]
      [InlineData("1.2.3-dev",   "1.2.3.*-dev")]
      [InlineData("1.2-dev",     "1.2.*-dev")]
      [InlineData("1-dev",       "1.*-dev")]
      public void GetRangeFromPuntitDependencyVersionTest(string version, string expectedVersion)
      {
         var range = VersionUtils.GetFloatFromPuntitDependencyVersion(version);

         Assert.Equal(expectedVersion, range.MinVersion.ToString());
      }
   }
}
