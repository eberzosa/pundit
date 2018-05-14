using EBerzosa.Pundit.Core.Utils;
using EBerzosa.Pundit.Core.Versioning;
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
      public void GetRangeFromPuntitDependencyVersionTest(string version, string expectedVersion)
      {
         var range = VersionUtils.ConvertPunditDependencyVersionToFloatVersion(version);

         Assert.Equal(expectedVersion, range.MinVersion.ToString());
      }
   }
}
