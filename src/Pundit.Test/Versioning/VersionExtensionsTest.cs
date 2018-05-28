using EBerzosa.Pundit.Core.Versioning;
using Xunit;

namespace Pundit.Test.Versioning
{
   public class VersionExtensionsTest
   {
      [Theory]
      [InlineData("1.1.1.1", 1, VersionPart.Major, "2.1.1.1")]
      [InlineData("1.1.1.1", 1, VersionPart.Minor, "1.2.1.1")]
      [InlineData("1.1.1.1", 1, VersionPart.Patch, "1.1.2.1")]
      [InlineData("1.1.1.1", 1, VersionPart.Revision, "1.1.1.2")]
      public void Add_Version_Test(string version, int value, VersionPart versionPart, string expectedVersion)
      {
         var nugetVersion = NuGet.Versioning.NuGetVersion.Parse(version);
         var expected = NuGet.Versioning.NuGetVersion.Parse(expectedVersion);

         var result = nugetVersion.Add(value, versionPart);

         Assert.Equal(expected, result);
      }

      [Theory]
      [InlineData("1.0",     "newLabel", "1.0-newLabel")]
      [InlineData("1.0-Old", "newLabel", "1.0-Old.newLabel")]
      [InlineData("1.0-x.y", "newLabel", "1.0-x.y.newLabel")]
      [InlineData("1.0-3",   "newLabel", "1.0-3.newLabel")]
      public void Append_ReleaseLabel_Test(string version, string label, string expectedVersion)
      {
         var nugetVersion = NuGet.Versioning.NuGetVersion.Parse(version);
         var expected = NuGet.Versioning.NuGetVersion.Parse(expectedVersion);

         var result = nugetVersion.Append(label);

         Assert.Equal(expected, result);
      }

      [Theory]
      [InlineData("1.0", "newLabel", "1.0-newLabel")]
      [InlineData("1.0-Old", "newLabel", "1.0-newLabel.Old")]
      [InlineData("1.0-x.y", "newLabel", "1.0-newLabel.x.y")]
      [InlineData("1.0-3", "newLabel", "1.0-newLabel.3")]
      public void Prepend_ReleaseLabel_Test(string version, string label, string expectedVersion)
      {
         var nugetVersion = NuGet.Versioning.NuGetVersion.Parse(version);
         var expected = NuGet.Versioning.NuGetVersion.Parse(expectedVersion);

         var result = nugetVersion.Prepend(label);

         Assert.Equal(expected, result);
      }

      [Theory]
      [InlineData("1.0", "1.1")]
      [InlineData("1.0", "1.0-x")]
      public void ReplaceMaxVersion_Test(string range, string newMaxVersion)
      {
         var versionRange = NuGet.Versioning.VersionRange.Parse(range);
         var maxVersion = NuGet.Versioning.NuGetVersion.Parse(newMaxVersion);

         var expected = new NuGet.Versioning.VersionRange(versionRange.MinVersion, true, NuGet.Versioning.NuGetVersion.Parse(newMaxVersion));

         var result = versionRange.ReplaceMaxVersion(maxVersion);
         
         Assert.Equal(expected, result);
      }

      [Theory]
      [InlineData("",              null,      null)]
      [InlineData("1",             "1.0.0.0", "2.0.0")]
      [InlineData("1.0",           "1.0.0.0", "1.1.0")]
      [InlineData("1.0.0",         "1.0.0.0", "1.0.1")]
      [InlineData("1.0.0.0",       "1.0.0.0", "1.0.0")]
      [InlineData("1.0.0.0.0",     null,      null)]
      [InlineData("1.0.0.0-b",     null,      null)]
      [InlineData("1.0.0.0-b.0",   null,      null)]
      [InlineData("1.0.0.0-b.0.0", null,      null)]
      [InlineData("1.0.0.0-b-0.0", null,      null)]

      [InlineData("1.0.0.1",     "1.0.0.1", "1.0.0.1")]
      [InlineData("1.0.0.1-b",   null,      null)]
      [InlineData("1.0.0.1-b.3", null,      null)]

      [InlineData("1.0.0-b",       "1.0.0.0-b",       "1.0.0.0-b")]
      [InlineData("1.0.0-b.0",     "1.0.0.0-b.0",     "1.0.0.0-b.0")]
      [InlineData("1.0.0-b.0.0",   "1.0.0.0-b.0.0",   "1.0.0.0-b.0.0")]
      [InlineData("1.0.0-b-0.0",   "1.0.0.0-b-0.0",   "1.0.0.0-b-0.0")]
      [InlineData("1.0.0-b.c-0.0", "1.0.0.0-b.c-0.0", "1.0.0.0-b.c-0.0")]

      [InlineData("1.0-b",     null, null)]
      [InlineData("1.0-b.1",   null, null)]
      [InlineData("1.0-b.1.1", null, null)]
      [InlineData("1.0-b-1.1", null, null)]
      [InlineData("1.0-b.1-1", null, null)]

      [InlineData("1-0.1-1", null, null)]
      [InlineData("1-b.1.1", null, null)]
      [InlineData("1-b.1-1", null, null)]
      public void GetVersionRangeFromDependency(string version, string expectedMinVersion, string expectedMaxVersion)
      {
         var expectedMin = expectedMinVersion == null ? null : new NuGet.Versioning.NuGetVersion(expectedMinVersion);
         var expectedMax = expectedMaxVersion == null ? null : new NuGet.Versioning.NuGetVersion(expectedMaxVersion);

         NuGet.Versioning.VersionRange range;
         try
         {
            range = VersionExtensions.GetVersionRangeFromDependency(version);
         }
         catch
         {
            Assert.Null(expectedMin);
            Assert.Null(expectedMax);

            return;
         }

         Assert.Equal(expectedMin, range.MinVersion);
         Assert.Equal(expectedMax, range.MaxVersion);
      }
   }
}
