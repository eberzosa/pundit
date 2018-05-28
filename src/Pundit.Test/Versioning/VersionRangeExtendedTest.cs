using EBerzosa.Pundit.Core.Versioning;
using Xunit;

namespace Pundit.Test.Versioning
{
   public class VersionRangeExtendedTest
   {
      [Theory]
      [InlineData("1.1.1.1", "1.1.1.1",       true)]
      [InlineData("1.1.1.1", "1.1.1.0",       false)]
      [InlineData("1.1.1.1", "1.1.1.2",       false)]
      [InlineData("1.1.1.1", "1.1.1-beta1.1", false)]
      [InlineData("1.1.1.1", "1.1.1-beta1.0", false)]
      [InlineData("1.1.1.1", "1.1.1-beta1.2", false)]
      [InlineData("1.1.1.1", "1.1.1-beta1",   false)]
      [InlineData("1.1.1.1", "1.1.2-beta1",   false)]
      [InlineData("1.1.1.1", "1.1.0-beta1",   false)]

      [InlineData("1.1.1.0", "1.1.1.0",       true)]
      [InlineData("1.1.1.0", "1.1.1",         true)]
      [InlineData("1.1.1.0", "1.1.1.1",       false)]
      [InlineData("1.1.1.0", "1.1.1-beta1.0", false)]
      [InlineData("1.1.1.0", "1.1.1-beta1",   false)]
      [InlineData("1.1.1.0", "1.1.0-beta1",   false)]
      [InlineData("1.1.1.0", "1.1.2-beta1",   false)]

      [InlineData("1.1.0.0", "1.1.0.0",       true)]
      [InlineData("1.1.0.0", "1.1.0",         true)]
      [InlineData("1.1.0.0", "1.1",           true)]
      [InlineData("1.1.0.0", "1.0",           false)]
      [InlineData("1.1.0.0", "1.2",           false)]
      [InlineData("1.1.0.0", "1.1.1.1",       false)]
      [InlineData("1.1.0.0", "1.1.0.1",       false)]
      [InlineData("1.1.0.0", "1.1.0-beta1.0", false)]
      [InlineData("1.1.0.0", "1.1.0-beta1.1", false)]
      [InlineData("1.1.0.0", "1.1.1-beta1.0", false)]
      [InlineData("1.1.0.0", "1.1.0-beta1",   false)]
      [InlineData("1.1.0.0", "1.0.0-beta1",   false)]
      [InlineData("1.1.0.0", "1.2.0-beta1",   false)]

      [InlineData("1.0.0.0", "1.0.0.0",       true)]
      [InlineData("1.0.0.0", "1.0.0",         true)]
      [InlineData("1.0.0.0", "1.0",           true)]
      [InlineData("1.0.0.0", "1",             true)]
      [InlineData("1.0.0.0", "1.1",           false)]
      [InlineData("1.0.0.0", "1.0.1.1",       false)]
      [InlineData("1.0.0.0", "1.0.0.1",       false)]
      [InlineData("1.0.0.0", "1.0.0-beta1.0", false)]
      [InlineData("1.0.0.0", "1.0.0-beta1.1", false)]
      [InlineData("1.0.0.0", "1.0.1-beta1.0", false)]
      [InlineData("1.0.0.0", "1.0.0-beta1",   false)]
      [InlineData("1.0.0.0", "1.0-beta1",     false)]
      [InlineData("1.0.0.0", "1-beta1",       false)]
      [InlineData("1.0.0.0", "1.1.0-beta1",   false)]

      [InlineData("1.1.1", "1.1.1.0",       true)]
      [InlineData("1.1.1", "1.1.1.1",       true)]
      [InlineData("1.1.1", "1.1.1-beta1.0", false)]
      [InlineData("1.1.1", "1.1.1-beta1",   false)]
      [InlineData("1.1.1", "1.1.0-beta1",   false)]
      [InlineData("1.1.1", "1.1.2-beta1",   false)]

      [InlineData("1.1.0", "1.1.0.0",       true)]
      [InlineData("1.1.0", "1.1.0",         true)]
      [InlineData("1.1.0", "1.1.1",         false)]
      [InlineData("1.1.0", "1.1",           true)]
      [InlineData("1.1.0", "1.0",           false)]
      [InlineData("1.1.0", "1.2",           false)]
      [InlineData("1.1.0", "1.1.1.1",       false)]
      [InlineData("1.1.0", "1.1.0.1",       true)]
      [InlineData("1.1.0", "1.1.0-beta1.0", false)]
      [InlineData("1.1.0", "1.1.0-beta1.1", false)]
      [InlineData("1.1.0", "1.1.1-beta1.0", false)]
      [InlineData("1.1.0", "1.1.0-beta1",   false)]
      [InlineData("1.1.0", "1.0.0-beta1",   false)]
      [InlineData("1.1.0", "1.2.0-beta1",   false)]

      [InlineData("1.0.0", "1.0.0.0",       true)]
      [InlineData("1.0.0", "1.0.0",         true)]
      [InlineData("1.0.0", "1.0",           true)]
      [InlineData("1.0.0", "1",             true)]
      [InlineData("1.0.0", "1.1",           false)]
      [InlineData("1.0.0", "1.0.1.1",       false)]
      [InlineData("1.0.0", "1.0.0.1",       true)]
      [InlineData("1.0.0", "1.0.0-beta1.0", false)]
      [InlineData("1.0.0", "1.0.0-beta1.1", false)]
      [InlineData("1.0.0", "1.0.1-beta1.0", false)]
      [InlineData("1.0.0", "1.0.0-beta1",   false)]
      [InlineData("1.0.0", "1.0-beta1",     false)]
      [InlineData("1.0.0", "1-beta1",       false)]
      [InlineData("1.0.0", "1.1.0-beta1",   false)]

      [InlineData("1.1", "1.1.0.0",       true)]
      [InlineData("1.1", "1.1.0",         true)]
      [InlineData("1.1", "1.1",           true)]
      [InlineData("1.1", "1.0",           false)]
      [InlineData("1.1", "1.2",           false)]
      [InlineData("1.1", "1.1.1.1",       true)]
      [InlineData("1.1", "1.1.0.1",       true)]
      [InlineData("1.1", "1.1.0-beta1.0", false)]
      [InlineData("1.1", "1.1.0-beta1.1", false)]
      [InlineData("1.1", "1.1.1-beta1.0", true)]
      [InlineData("1.1", "1.1.0-beta1",   false)]
      [InlineData("1.1", "1.0.0-beta1",   false)]
      [InlineData("1.1", "1.2.0-beta1",   false)]

      [InlineData("1.0", "1.0.0.0",       true)]
      [InlineData("1.0", "1.0.0",         true)]
      [InlineData("1.0", "1.0",           true)]
      [InlineData("1.0", "1",             true)]
      [InlineData("1.0", "1.1",           false)]
      [InlineData("1.0", "1.0.1.1",       true)]
      [InlineData("1.0", "1.0.0.1",       true)]
      [InlineData("1.0", "1.0.0-beta1.0", false)]
      [InlineData("1.0", "1.0.0-beta1.1", false)]
      [InlineData("1.0", "1.0.1-beta1.0", true)]
      [InlineData("1.0", "1.1.0-beta1.0", false)]
      [InlineData("1.0", "1.0.0-beta1",   false)]
      [InlineData("1.0", "1.0-beta1",     false)]
      [InlineData("1.0", "1-beta1",       false)]
      [InlineData("1.0", "1.1.0-beta1",   false)]
      
      [InlineData("1", "1.0.0.0",       true)]
      [InlineData("1", "1.0.0",         true)]
      [InlineData("1", "1.0",           true)]
      [InlineData("1", "1",             true)]
      [InlineData("1", "1.1",           true)]
      [InlineData("1", "1.0.1.1",       true)]
      [InlineData("1", "1.0.0.1",       true)]
      [InlineData("1", "1.0.0-beta1.0", false)]
      [InlineData("1", "1.0.0-beta1.1", false)]
      [InlineData("1", "1.0.1-beta1.0", true)]
      [InlineData("1", "1.1.0-beta1",   true)]
      [InlineData("1", "1.0.0-beta1",   false)]
      [InlineData("1", "1.0-beta1",     false)]
      [InlineData("1", "1.1-beta1.2",   true)]
      [InlineData("1", "1-beta1",       false)]
      [InlineData("1", "1-beta1.3",     false)]

      [InlineData("1.1.1-beta1", "1.1.1-beta1",   true)]
      [InlineData("1.1.1-beta1", "1.1.1-beta1.0", false)]
      [InlineData("1.1.1-beta1", "1.1.1-beta1.1", false)]
      [InlineData("1.1.1-beta1", "1.1.1-beta2",   false)]
      [InlineData("1.1.1-beta1", "1.1.1-beta2.0", false)]
      [InlineData("1.1.1-beta1", "1.1.1-beta2.1", false)]
      [InlineData("1.1.1-beta1", "1.1.1",         false)]
      [InlineData("1.1.1-beta1", "1.1.2",         false)]
      [InlineData("1.1.1-beta1", "1.1.1.0",       false)]
      [InlineData("1.1.1-beta1", "1.1.1.1",       false)]

      [InlineData("1.1.1-beta1.0", "1.1.1-beta1",   false)]
      [InlineData("1.1.1-beta1.0", "1.1.1-beta1.0", true)]
      [InlineData("1.1.1-beta1.0", "1.1.1-beta1.1", false)]
      [InlineData("1.1.1-beta1.0", "1.1.1-beta2",   false)]
      [InlineData("1.1.1-beta1.0", "1.1.1-beta2.0", false)]
      [InlineData("1.1.1-beta1.0", "1.1.1-beta2.1", false)]
      [InlineData("1.1.1-beta1.0", "1.1.1",         false)]
      [InlineData("1.1.1-beta1.0", "1.1.2",         false)]
      [InlineData("1.1.1-beta1.0", "1.1.1.0",       false)]
      [InlineData("1.1.1-beta1.0", "1.1.1.1",       false)]

      [InlineData("1.1.1-beta1.1", "1.1.1-beta1",   false)]
      [InlineData("1.1.1-beta1.1", "1.1.1-beta1.1", true)]
      [InlineData("1.1.1-beta1.1", "1.1.1-beta1.2", false)]
      [InlineData("1.1.1-beta1.1", "1.1.1-beta2",   false)]
      [InlineData("1.1.1-beta1.1", "1.1.1-beta2.0", false)]
      [InlineData("1.1.1-beta1.1", "1.1.1-beta2.1", false)]
      [InlineData("1.1.1-beta1.1", "1.1.1",         false)]
      [InlineData("1.1.1-beta1.1", "1.1.2",         false)]
      [InlineData("1.1.1-beta1.1", "1.1.1.0",       false)]
      [InlineData("1.1.1-beta1.1", "1.1.1.1",       false)]
      public void Satisfies_Test(string version, string vToCheck, bool expectedResult)
      {
         var range = VersionExtensions.GetVersionRangeFromDependency(version);
         var checkVersion = NuGet.Versioning.NuGetVersion.Parse(vToCheck);
         
         var result = new VersionRangeExtended(range).Satisfies(checkVersion);

         Assert.Equal(expectedResult, result);
      }

      [Theory]
      [InlineData("1.1.1.1", "1", null)] // Not supported

      [InlineData("1.1.1", "1",           false)]
      [InlineData("1.1.1", "1.1",         false)]
      [InlineData("1.1.1", "1.1.1",       true)]
      [InlineData("1.1.1", "1.1.1.1",     true)]
      [InlineData("1.1.1", "1.1.1-ABC",   true)]
      [InlineData("1.1.1", "1.1.1-ABC.0", true)]
      [InlineData("1.1.1", "1.1.1-ABC.1", true)]
      [InlineData("1.1.1", "1.1.2-ABC",   false)]
      [InlineData("1.1.1", "1.1.2-ABC.1", false)]
      [InlineData("1.1.1", "1.1.1-beta1", false)]
      [InlineData("1.1.1", "1.1.2-beta1", false)]
      
      [InlineData("1.1", "1",           false)]
      [InlineData("1.1", "1.1",         true)]
      [InlineData("1.1", "1.1.1.1",     true)]
      [InlineData("1.1", "1.1.0.1",     true)]
      [InlineData("1.1", "1.1.0-ABC",   true)]
      [InlineData("1.1", "1.1.0-ABC.0", true)]
      [InlineData("1.1", "1.1.0-ABC.1", true)]
      [InlineData("1.1", "1.1.1-ABC",   true)]
      [InlineData("1.1", "1.1.1-ABC.1", true)]
      [InlineData("1.1", "1.2.0-ABC.0", false)]
      [InlineData("1.1", "1.2.0-ABC.1", false)]
      [InlineData("1.1", "1.1.0-beta1", false)]
      [InlineData("1.1", "1.2.0-beta1", false)]

      [InlineData("1", "1",           true)]
      [InlineData("1", "1.1",         true)]
      [InlineData("1", "1.0.1.1",     true)]
      [InlineData("1", "1.0.0.1",     true)]
      [InlineData("1", "1.0.0-ABC",   true)]
      [InlineData("1", "1.0.0-ABC.0", true)]
      [InlineData("1", "1.0.0-ABC.1", true)]
      [InlineData("1", "1.0.1-ABC.0", true)]
      [InlineData("1", "1.0.1-ABC.1", true)]
      [InlineData("1", "1.1.0-ABC.0", true)]
      [InlineData("1", "1.1.0-ABC.1", true)]
      [InlineData("1", "2.0.0-ABC.1", false)]
      [InlineData("1", "1.0.0-beta1", false)]
      [InlineData("1", "2.0.0-beta1", false)]

      [InlineData("1.1.1-beta1", "1.1.1", null)] // Not supported
      public void Satisfies_WithExtraLabel_Test(string version, string vToCheck, bool? expectedResult)
      {
         var range = VersionExtensions.GetVersionRangeFromDependency(version);
         var checkVersion = NuGet.Versioning.NuGetVersion.Parse(vToCheck);

         bool? result = null;
         try
         {
            result = new VersionRangeExtended(range) {ReleaseLabel = "ABC"}.Satisfies(checkVersion);
         }
         catch
         {
            Assert.Null(expectedResult);
         }

         Assert.Equal(expectedResult, result);
      }

      [Theory]
      [InlineData("1.0.0", "1.0.0", "1.0.0",     false)] // Range [1.0.0.0, 1.0.1.0)
      [InlineData("1.0.0", "1.0.0", "1.0.1",     false)]
      [InlineData("1.0.0", "1.0.0", "1.1.0",     false)]
      [InlineData("1.0.0", "1.0.0", "1.0.0.1",   true)]
      [InlineData("1.0.0", "1.0.0", "1.0.0-b",   false)] // Range [1.0.0.0, 1.0.1.0--)
      [InlineData("1.0.0", "1.0.0", "1.0.0-b.1", false)]
      [InlineData("1.0.0", "1.0.0", "1.0.1-b",   false)]
      [InlineData("1.0.0", "1.0.0", "1.0.1-b.1", false)]
      [InlineData("1.0.0", "1.0.0", "1.1.0-b",   false)]
      [InlineData("1.0.0", "1.0.0", "1.1.0-b.1", false)]

      [InlineData("1.0.1", "1.0.1", "1.0.0",     false)] // Range [1.0.0.0, 1.0.1.0)
      [InlineData("1.0.1", "1.0.1", "1.0.1",     false)]
      [InlineData("1.0.1", "1.0.1", "1.0.2",     false)]
      [InlineData("1.0.1", "1.0.1", "1.1.0",     false)]
      [InlineData("1.0.1", "1.0.1", "1.0.1.0",   false)]
      [InlineData("1.0.1", "1.0.1", "1.0.1.2",   true)]
      [InlineData("1.0.1", "1.0.1", "1.0.1-b",   false)]
      [InlineData("1.0.1", "1.0.1", "1.0.1-b.1", false)]

      [InlineData("1.1.0", "1.1.0", "1.1.0",     false)] // Range [1.0.0.0, 1.0.1.0)
      [InlineData("1.1.0", "1.1.0", "1.1.1",     false)]
      [InlineData("1.1.0", "1.1.0", "1.2.0.0",   false)]
      [InlineData("1.1.0", "1.1.0", "1.1.0.1",   true)]
      [InlineData("1.1.0", "1.1.0", "1.1.1-b",   false)]
      [InlineData("1.1.0", "1.1.0", "1.1.1-b.1", false)]

      [InlineData("1.1", "1.1.0", "1.1.0",     false)] // Range [1.1.0, 1.2.0)
      [InlineData("1.1", "1.1.0", "1.1.1",     true)]
      [InlineData("1.1", "1.1.0", "1.2.0.0",   false)]
      [InlineData("1.1", "1.1.0", "1.1.0.1",   true)]
      [InlineData("1.1", "1.1.0", "1.1.1-b",   true)] // Range [1.1.0, 1.2.0)
      [InlineData("1.1", "1.1.0", "1.1.1-b.1", true)]
      [InlineData("1.1", "1.1.0", "1.2.0-b",   false)]

      [InlineData("2.1", "2.1.3", "2.1.0",     false)] // Range [2.1.0, 2.2.0)
      [InlineData("2.1", "2.1.3", "2.1.3",     false)]
      [InlineData("2.1", "2.1.3", "2.1.1",     false)]
      [InlineData("2.1", "2.1.3", "2.1.4",     true)]
      [InlineData("2.1", "2.1.3", "2.1.3-b",   false)] // Range [2.1.0, 2.2.0)
      [InlineData("2.1", "2.1.3", "2.1.3-b.1", false)]
      [InlineData("2.1", "2.1.3", "2.1.4-b",   true)]
      [InlineData("2.1", "2.1.3", "2.2.0",     false)]
      [InlineData("2.1", "2.1.3", "2.2.0-b",   false)]

      [InlineData("1.0.0-beta1", "1.0.0-beta1", "1.0.0-beta1",   false)]
      [InlineData("1.0.0-beta1", "1.0.0-beta1", "1.0.0-beta2",   false)]
      [InlineData("1.0.0-beta1", "1.0.0-beta1", "1.0.0-beta1.1", false)]
      public void IsBetter_Test(string rangeString, string currentString, string consideringString, bool expectedResult)
      {
         var range = VersionExtensions.GetVersionRangeFromDependency(rangeString);
         var current = NuGet.Versioning.NuGetVersion.Parse(currentString);
         var considering = NuGet.Versioning.NuGetVersion.Parse(consideringString);

         var result = new VersionRangeExtended(range).IsBetter(current, considering);

         Assert.Equal(expectedResult, result);
      }

      [Theory]
      [InlineData("1.0.0", "1.0.0", "1.0.0",       false)] // Range [1.0.0.0, 1.0.1.0)
      [InlineData("1.0.0", "1.0.0", "1.0.1",       false)]
      [InlineData("1.0.0", "1.0.0", "1.1.0",       false)]
      [InlineData("1.0.0", "1.0.0", "1.0.0.1",     true)]
      [InlineData("1.0.0", "1.0.0", "1.0.0-ABC",   true)]
      [InlineData("1.0.0", "1.0.0", "1.0.0-ABC.0", true)]
      [InlineData("1.0.0", "1.0.0", "1.0.0-ABC.1", true)]
      [InlineData("1.0.0", "1.0.0", "1.0.0-b",     false)] // Range [1.0.0.0, 1.0.1.0--)
      [InlineData("1.0.0", "1.0.0", "1.0.0-b.1",   false)]
      [InlineData("1.0.0", "1.0.0", "1.0.1-b",     false)]
      [InlineData("1.0.0", "1.0.0", "1.0.1-b.1",   false)]
      [InlineData("1.0.0", "1.0.0", "1.1.0-b",     false)]
      [InlineData("1.0.0", "1.0.0", "1.1.0-b.1",   false)]

      [InlineData("1.0.0", "1.0.0-ABC", "1.0.0-ABC.0", false)]
      [InlineData("1.0.0", "1.0.0-ABC", "1.0.0-ABC.1", true)]
      [InlineData("1.0.0", "1.0.0-ABC", "1.0.0.2",     false)]

      [InlineData("1.0", "1.0.0", "1.0.0",       false)] // Range [1.0.0.0, 1.0.1.0)
      [InlineData("1.0", "1.0.0", "1.0.1",       true)]
      [InlineData("1.0", "1.0.0", "1.1.0",       false)]
      [InlineData("1.0", "1.0.0", "1.0.0.1",     true)]
      [InlineData("1.0", "1.0.0", "1.0.0-ABC",   true)]
      [InlineData("1.0", "1.0.0", "1.0.0-ABC.0", true)]
      [InlineData("1.0", "1.0.0", "1.0.0-ABC.1", true)]
      [InlineData("1.0", "1.0.0", "1.0.1-ABC",   true)]
      [InlineData("1.0", "1.0.0", "1.0.1-ABC.0", true)]
      [InlineData("1.0", "1.0.0", "1.0.1-ABC.1", true)]
      [InlineData("1.0", "1.0.0", "1.0.0-b",     false)] // Range [1.0.0.0, 1.0.1.0--)
      [InlineData("1.0", "1.0.0", "1.0.0-b.1",   false)]
      [InlineData("1.0", "1.0.0", "1.0.1-b",     false)]
      [InlineData("1.0", "1.0.0", "1.0.1-b.1",   false)]
      [InlineData("1.0", "1.0.0", "1.1.0-b",     false)]
      [InlineData("1.0", "1.0.0", "1.1.0-b.1",   false)]

      [InlineData("1.0", "1.0.1-ABC", "1.0.0-ABC.0", false)]
      [InlineData("1.0", "1.0.1-ABC", "1.0.0-ABC.1", false)]
      [InlineData("1.0", "1.0.1-ABC", "1.0.0.2",     false)]
      [InlineData("1.0", "1.0.1-ABC", "1.0.0-ABC.5", false)]
      [InlineData("1.0", "1.0.1-ABC", "1.0.1-ABC.1", true)]
      [InlineData("1.0", "1.0.1-ABC", "1.0.2-ABC",   true)]
      [InlineData("1.0", "1.0.1-ABC", "1.0.2-ABC.0", true)]
      [InlineData("1.0", "1.0.1-ABC", "1.0.2-ABC.1", true)]
      [InlineData("1.0", "1.0.1-ABC", "1.0.2",       true)]
      public void IsBetter_WithReleaseLabel_Test(string rangeString, string currentString, string consideringString, bool expectedResult)
      {
         var range = VersionExtensions.GetVersionRangeFromDependency(rangeString);
         var current = NuGet.Versioning.NuGetVersion.Parse(currentString);
         var considering = NuGet.Versioning.NuGetVersion.Parse(consideringString);

         var result = new VersionRangeExtended(range){ReleaseLabel = "ABC"}.IsBetter(current, considering);

         Assert.Equal(expectedResult, result);
      }
   }
}
