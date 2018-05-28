using EBerzosa.Pundit.Core.Versioning;
using Xunit;

namespace Pundit.Test.Versioning
{
   public class VersionComparerExtendedTest
   {
      [Theory]
      [InlineData("1.0", "1.0",         0)]
      [InlineData("1.0", "1.0.1",       -1)]
      [InlineData("1.0", "1.0.0.1",     -1)]
      [InlineData("1.0", "1.1",         -1)]
      [InlineData("1.0", "2.0",         -1)]
      [InlineData("1.0", "1.0.0-ABC",    1)]
      [InlineData("1.0", "1.0.0-ABC.5",  1)]
      [InlineData("1.0", "1.0.1-ABC",   -1)]

      [InlineData("1.0.1", "1.0",         1)]
      [InlineData("1.0.1", "1.0.1",       0)]
      [InlineData("1.0.1", "1.0.0.1",     1)]
      [InlineData("1.0.1", "1.0.1.1",    -1)]
      [InlineData("1.0.1", "1.1",        -1)]
      [InlineData("1.0.1", "2.0",        -1)]
      [InlineData("1.0.1", "1.0.1-ABC",   1)]
      [InlineData("1.0.1", "1.0.1-ABC.5", 1)]
      [InlineData("1.0.1", "1.0.2-ABC",  -1)]
      
      [InlineData("1.0.1.1", "1.0",           1)]
      [InlineData("1.0.1.1", "1.0.1",         1)]
      [InlineData("1.0.1.1", "1.0.0.1",       1)]
      [InlineData("1.0.1.1", "1.0.1.1",       0)]
      [InlineData("1.0.1.1", "1.0.1.2",      -1)]
      [InlineData("1.0.1.1", "1.1",          -1)]
      [InlineData("1.0.1.1", "2.0",          -1)]
      [InlineData("1.0.1.1", "1.0.1.1-ABC",   1)]
      [InlineData("1.0.1.1", "1.0.1.1-ABC.5", 1)]
      [InlineData("1.0.1.1", "1.0.1.2-ABC",  -1)]
      [InlineData("1.0.1.1", "1.0.2-ABC",    -1)]

      [InlineData("1.0.1.0-ABC", "1.0.1.0-ABC",  0)]
      [InlineData("1.0.1.0-ABC", "1.0.1-ABC",    0)]
      [InlineData("1.0.1.0-ABC", "1.0.1.2-ABC", -1)]
      [InlineData("1.0.1.0-ABC", "1.0.0.9-ABC",  1)]
      [InlineData("1.0.1.1-ABC", "1.0.1.1-ABC",  0)]
      [InlineData("1.0.1.1-ABC", "1.0.1-ABC.1",  1)]
      [InlineData("1.0.1.1-ABC", "1.0.1.2-ABC", -1)]
      [InlineData("1.0.1.1-ABC", "1.0.1.0-ABC",  1)]

      [InlineData("1.0.1-ABC", "1.0.1.0-ABC",  0)]
      [InlineData("1.0.1-ABC", "1.0.1-ABC.0", -1)]
      [InlineData("1.0.1-ABC", "1.0.1-ABC",    0)]
      [InlineData("1.0.1-ABC", "1.0.1.2-ABC", -1)]
      [InlineData("1.0.1-ABC", "1.0.0.9-ABC",  1)]
      [InlineData("1.0.1-ABC", "1.0.1.1-ABC", -1)]
      [InlineData("1.0.1-ABC", "1.0.1-ABC.1", -1)]
      [InlineData("1.0.1-ABC", "1.0.1-ABC.2", -1)]
      [InlineData("1.0.1-ABC", "1.0.2-ABC",   -1)]
      public void Compare_VersionRelease_Test(string version1, string version2, int expected)
      {
         var v1 = NuGet.Versioning.NuGetVersion.Parse(version1);
         var v2 = NuGet.Versioning.NuGetVersion.Parse(version2);

         var comparer = new VersionComparerExtended(VersionComparer.VersionRelease);

         var result = comparer.Compare(v1, v2);
         var resultInverse = comparer.Compare(v2, v1);

         if (expected == 0)
         {
            Assert.Equal(0, result);
            Assert.Equal(0, resultInverse);
         }
         else if (expected < 0)
         {
            Assert.InRange(result, int.MinValue, -1);
            Assert.InRange(resultInverse, 1, int.MaxValue);
         }
         else
         {
            Assert.InRange(result, 1, int.MaxValue);
            Assert.InRange(resultInverse, int.MinValue, -1);
         }
      }
      
      [Theory]
      [InlineData("1.0", "1.0",          0)]
      [InlineData("1.0", "1.0.1",       -1)]
      [InlineData("1.0", "1.0.0.1",     -1)]
      [InlineData("1.0", "1.1",         -1)]
      [InlineData("1.0", "2.0",         -1)]
      [InlineData("1.0", "1.0.0-ABC",   -1)]
      [InlineData("1.0", "1.0.0-ABC.5", -1)]
      [InlineData("1.0", "1.0.1-ABC",   -1)]

      [InlineData("1.0.1", "1.0",          1)]
      [InlineData("1.0.1", "1.0.1",        0)]
      [InlineData("1.0.1", "1.0.0.1",      1)]
      [InlineData("1.0.1", "1.0.1.1",     -1)]
      [InlineData("1.0.1", "1.1",         -1)]
      [InlineData("1.0.1", "2.0",         -1)]
      [InlineData("1.0.1", "1.0.1-ABC",   -1)]
      [InlineData("1.0.1", "1.0.1-ABC.5", -1)]
      [InlineData("1.0.1", "1.0.2-ABC",   -1)]
      
      [InlineData("1.0.1.1", "1.0",            1)]
      [InlineData("1.0.1.1", "1.0.1",          1)]
      [InlineData("1.0.1.1", "1.0.0.1",        1)]
      [InlineData("1.0.1.1", "1.0.1.1",        0)]
      [InlineData("1.0.1.1", "1.0.1.2",       -1)]
      [InlineData("1.0.1.1", "1.1",           -1)]
      [InlineData("1.0.1.1", "2.0",           -1)]
      [InlineData("1.0.1.1", "1.0.1.1-ABC",   -1)]
      [InlineData("1.0.1.1", "1.0.1.1-ABC.5", -1)]
      [InlineData("1.0.1.1", "1.0.1.2-ABC",   -1)]
      [InlineData("1.0.1.1", "1.0.2-ABC",     -1)]

      [InlineData("1.0.1.0-ABC", "1.0.1.0-ABC",  0)]
      [InlineData("1.0.1.0-ABC", "1.0.1-ABC",    0)]
      [InlineData("1.0.1.0-ABC", "1.0.1.2-ABC", -1)]
      [InlineData("1.0.1.0-ABC", "1.0.0.9-ABC",  1)]
      [InlineData("1.0.1.1-ABC", "1.0.1.1-ABC",  0)]
      [InlineData("1.0.1.1-ABC", "1.0.1-ABC.1",  1)]
      [InlineData("1.0.1.1-ABC", "1.0.1.2-ABC", -1)]
      [InlineData("1.0.1.1-ABC", "1.0.1.0-ABC",  1)]

      [InlineData("1.0.1-ABC", "1.0.1.0-ABC",  0)]
      [InlineData("1.0.1-ABC", "1.0.1-ABC.0",  0)]
      [InlineData("1.0.1-ABC", "1.0.1-ABC",    0)]
      [InlineData("1.0.1-ABC", "1.0.1.2-ABC", -1)]
      [InlineData("1.0.1-ABC", "1.0.0.9-ABC",  1)]
      [InlineData("1.0.1-ABC", "1.0.1.1-ABC", -1)]
      [InlineData("1.0.1-ABC", "1.0.1-ABC.1", -1)]
      [InlineData("1.0.1-ABC", "1.0.1-ABC.2", -1)]
      [InlineData("1.0.1-ABC", "1.0.2-ABC",   -1)]
      public void Compare_Pundit_Test(string version1, string version2, int expected)
      {
         var v1 = NuGet.Versioning.NuGetVersion.Parse(version1);
         var v2 = NuGet.Versioning.NuGetVersion.Parse(version2);

         var comparer = new VersionComparerExtended(VersionComparer.Pundit);

         var result = comparer.Compare(v1, v2);
         var resultInverse = comparer.Compare(v2, v1);

         if (expected == 0)
         {
            Assert.Equal(0, result);
            Assert.Equal(0, resultInverse);
         }
         else if (expected < 0)
         {
            Assert.InRange(result, int.MinValue, -1);
            Assert.InRange(resultInverse, 1, int.MaxValue);
         }
         else
         {
            Assert.InRange(result, 1, int.MaxValue);
            Assert.InRange(resultInverse, int.MinValue, -1);
         }
      }
   }
}
