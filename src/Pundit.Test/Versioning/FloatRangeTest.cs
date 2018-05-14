using EBerzosa.Pundit.Core.Versioning;
using Xunit;

namespace Pundit.Test.Versioning
{
   public class FloatRangeTest
   {
      [Theory]
      [InlineData("1",             FloatBehaviour.None,               "1",            null)]
      [InlineData("1.2",           FloatBehaviour.None,               "1.2",          null)]
      [InlineData("1.2.3",         FloatBehaviour.None,               "1.2.3",        null)]
      [InlineData("1.2.3.4",       FloatBehaviour.None,               "1.2.3.4",      null)]
      [InlineData("1.2.3-4",       FloatBehaviour.None,               "1.2.3-4",      "4")]
      [InlineData("1.2.3-beta",    FloatBehaviour.None,               "1.2.3-beta",   "beta")]
      [InlineData("1.2.3-beta.4",  FloatBehaviour.None,               "1.2.3-beta.4", "beta.4")]
                                                                    
      [InlineData("*",             FloatBehaviour.Major,              "0",             null)]
      [InlineData("1.*",           FloatBehaviour.Minor,              "1.0",           null)]
      [InlineData("1.2.*",         FloatBehaviour.Patch,              "1.2.0",         null)]
      [InlineData("1.2.3.*",       FloatBehaviour.Revision,           "1.2.3.0",       null)]

      [InlineData("1.2.3-*",       FloatBehaviour.Prerelease,         "1.2.3-0",       "")]
      [InlineData("1.2.3-4*",      FloatBehaviour.Prerelease,         "1.2.3-4",       "4")]

      [InlineData("1.2.3-beta.*",  FloatBehaviour.RevisionPrerelease, "1.2.3-beta.0",  "beta.")]
      [InlineData("1.2.*-beta.*",  FloatBehaviour.PatchPrerelease,    "1.2.0-beta.0",  "beta.")]
      [InlineData("1.*-beta.*",    FloatBehaviour.MinorPrerelease,    "1.0-beta.0",    "beta.")]
      [InlineData("*-beta.*",      FloatBehaviour.MajorPrerelease,    "0-beta.0",      "beta.")]
      public void Parse_Valid(string version, FloatBehaviour expectedFloatBehaviour, string expectedMinVersion, string expectedOriginalRelease)
      {
         var result = FloatRange.TryParse(version, out var floatVersion);
         
         Assert.True(result);
         Assert.Equal(expectedFloatBehaviour, floatVersion.FloatBehaviour);
         Assert.Equal(expectedMinVersion, floatVersion.MinVersion.OriginalVersion);
         Assert.Equal(expectedOriginalRelease, floatVersion.OriginalReleasePrefix);
      }

      [Theory]
      [InlineData("1*")]
      [InlineData("*1")]
      [InlineData("1.2*")]
      [InlineData("1.*2")]
      [InlineData("1.2.3*")]
      [InlineData("1.2.*3")]
      [InlineData("1.2.3.4*")]
      [InlineData("1.2.3.*4")]
      [InlineData("1.2.*-beta*")]
      [InlineData("1.*-beta*")]
      [InlineData("*-beta*")]
      [InlineData("1.*.1-beta*")]
      [InlineData("1.*-*.4")]
      [InlineData("1.0.0-*.4")]
      [InlineData("1.0.0-*.")]
      [InlineData(".*")]
      [InlineData("*.")]
      [InlineData("0.0.0-")]
      public void Parse_NotValid(string version)
      {
         var result = FloatRange.TryParse(version, out var floatVersion);

         Assert.False(result);
         Assert.Null(floatVersion);
      }

      [Theory]
      [InlineData("1",             "1.0.0")]
      [InlineData("1",             "1.0.0.0")]
      [InlineData("1.2",           "1.2.0")]
      [InlineData("1.2",           "1.2.0.0")]
      [InlineData("1.2.3",         "1.2.3")]
      [InlineData("1.2.3",         "1.2.3.0")]
      [InlineData("1.2.3.4",       "1.2.3.4")]
      [InlineData("1.2.3-4",       "1.2.3-4")]
      [InlineData("1.2.3-beta",    "1.2.3-beta")]
      [InlineData("1.2.3-beta.4",  "1.2.3-beta.4")]
                                   
      [InlineData("*",             "0.0.0")]
      [InlineData("*",             "0.0.0.0")]
      [InlineData("*",             "0.0.0.1")]
      [InlineData("*",             "0.0.1.0")]
      [InlineData("*",             "0.1.0.0")]
      [InlineData("*",             "1.0.0.0")]
      [InlineData("1.*",           "1.0.0")]
      [InlineData("1.*",           "1.0.0.0")]
      [InlineData("1.*",           "1.0.0.1")]
      [InlineData("1.*",           "1.0.1.0")]
      [InlineData("1.*",           "1.1.0.0")]
      [InlineData("1.2.*",         "1.2.0")]
      [InlineData("1.2.*",         "1.2.0.0")]
      [InlineData("1.2.*",         "1.2.0.1")]
      [InlineData("1.2.*",         "1.2.1.0")]
      [InlineData("1.2.3.*",       "1.2.3")]
      [InlineData("1.2.3.*",       "1.2.3.0")]
      [InlineData("1.2.3.*",       "1.2.3.1")]

      [InlineData("1.2.3-*",       "1.2.3-alpha")]
      [InlineData("1.2.3-*",       "1.2.3")]
      [InlineData("1.2.3-*",       "1.2.3-4")]
      [InlineData("1.2.3-4*",      "1.2.3-4")]
      [InlineData("1.2.3-4*",      "1.2.3-4alpha")]
      [InlineData("1.2.3-4*",      "1.2.3")]

      [InlineData("1.2.3-beta.*",  "1.2.3-beta")]
      [InlineData("1.2.3-beta.*",  "1.2.3-beta.0")]
      [InlineData("1.2.3-beta.*",  "1.2.3-beta.1")]
      [InlineData("1.2.3-beta.*",  "1.2.3")]
      [InlineData("1.2.*-beta.*",  "1.2.0-beta")]
      [InlineData("1.2.*-beta.*",  "1.2.0-beta.0")]
      [InlineData("1.2.*-beta.*",  "1.2.0-beta.1")]
      [InlineData("1.2.*-beta.*",  "1.2.1-beta")]
      [InlineData("1.2.*-beta.*",  "1.2.1-beta.0")]
      [InlineData("1.2.*-beta.*",  "1.2.1-beta.1")]
      [InlineData("1.2.*-beta.*",  "1.2.1")]
      [InlineData("1.*-beta.*",    "1.0.0-beta")]
      [InlineData("1.*-beta.*",    "1.0.0-beta.0")]
      [InlineData("1.*-beta.*",    "1.0.0-beta.1")]
      [InlineData("1.*-beta.*",    "1.0.1-beta")]
      [InlineData("1.*-beta.*",    "1.0.1-beta.0")]
      [InlineData("1.*-beta.*",    "1.0.1-beta.1")]
      [InlineData("1.*-beta.*",    "1.1.0-beta")]
      [InlineData("1.*-beta.*",    "1.1.0-beta.0")]
      [InlineData("1.*-beta.*",    "1.1.0-beta.1")]
      [InlineData("1.*-beta.*",    "1.1.0")]
      [InlineData("*-beta.*",      "0.0.0-beta")]
      [InlineData("*-beta.*",      "0.0.0-beta.0")]
      [InlineData("*-beta.*",      "0.0.0-beta.1")]
      [InlineData("*-beta.*",      "0.0.1-beta")]
      [InlineData("*-beta.*",      "0.0.1-beta.0")]
      [InlineData("*-beta.*",      "0.0.1-beta.1")]
      [InlineData("*-beta.*",      "0.1.0-beta")]
      [InlineData("*-beta.*",      "0.1.0-beta.0")]
      [InlineData("*-beta.*",      "0.1.0-beta.1")]
      [InlineData("*-beta.*",      "1.0.0-beta")]
      [InlineData("*-beta.*",      "1.0.0-beta.0")]
      [InlineData("*-beta.*",      "1.0.0-beta.1")]
      [InlineData("*-beta.*",      "1.0.0")]
      public void Satisfies_True(string version, string satisfiesVersion)
      {
         var satisfies = PunditVersion.Parse(satisfiesVersion);
         var floatVersion = FloatRange.Parse(version);

         var result = floatVersion.Satisfies(satisfies);

         Assert.True(result);
      }
      
      [Theory]
      [InlineData("1",             "2.0.0")]
      [InlineData("1",             "1.0.0-beta")]
      [InlineData("1",             "0.0.0")]
      [InlineData("1.2",           "1.3.0.0")]
      [InlineData("1.2",           "1.2.0-beta")]
      [InlineData("1.2",           "1.1.0")]
      [InlineData("1.2",           "0.2.0")]
      [InlineData("1.2.3",         "1.2.4")]
      [InlineData("1.2.3",         "1.2.3-beta")]
      [InlineData("1.2.3",         "1.2.2")]
      [InlineData("1.2.3",         "1.1.0")]
      [InlineData("1.2.3.4",       "1.2.3.5")]
      [InlineData("1.2.3.4",       "1.2.3.3")]
      [InlineData("1.2.3.4",       "1.2.2.4")]
      [InlineData("1.2.3.4",       "1.1.3.4")]
      [InlineData("1.2.3.4",       "0.2.3.4")]
      [InlineData("1.2.3-4",       "1.2.3-5")]
      [InlineData("1.2.3-4",       "1.2.3-3")]
      [InlineData("1.2.3-4",       "1.2.2-4")]
      [InlineData("1.2.3-4",       "1.1.3-4")]
      [InlineData("1.2.3-4",       "0.2.3-4")]
      [InlineData("1.2.3-beta",    "1.2.3-alpha")]
      [InlineData("1.2.3-beta",    "1.2.2-beta")]
      [InlineData("1.2.3-beta",    "1.2.4-beta")]
      [InlineData("1.2.3-beta",    "1.2.3-beta2")]
      [InlineData("1.2.3-beta.4",  "1.2.3-beta.5")]
      [InlineData("1.2.3-beta.4",  "1.2.3-beta.3")]
      [InlineData("1.2.3-beta.4",  "1.2.3-beta")]
      [InlineData("1.2.3-beta.4",  "1.2.4-beta.4")]
      [InlineData("1.2.3-beta.4",  "1.2.5-beta.4")]
                                   
      [InlineData("*",             "1.0.0-beta")]
      [InlineData("1.*",           "0.1.0")]
      [InlineData("1.*",           "2.0.0")]
      [InlineData("1.*",           "1.0.0-beta")]
      [InlineData("1.2.*",         "1.1.0")]
      [InlineData("1.2.*",         "1.3.0")]
      [InlineData("1.2.*",         "1.2.0-beta")]
      [InlineData("1.2.3.*",       "1.2.2")]
      [InlineData("1.2.3.*",       "1.2.4")]
      [InlineData("1.2.3.*",       "1.2.3-beta")]

      [InlineData("1.2.3-*",       "1.2.4-0")]
      [InlineData("1.2.3-*",       "1.2.2-0")]
      [InlineData("1.2.3-4*",      "1.2.3-5")]
      [InlineData("1.2.3-4*",      "1.2.3-3")]

      [InlineData("1.2.3-beta.*",  "1.2.3-beta0")]
      [InlineData("1.2.3-beta.*",  "1.2.4-beta.0")]
      [InlineData("1.2.3-beta.*",  "1.2.2-beta.0")]
      [InlineData("1.2.3-beta.*",  "1.2.3-alpha")]
      [InlineData("1.2.*-beta.*",  "1.2.0-beta0")]
      [InlineData("1.2.*-beta.*",  "1.1.0-beta.0")]
      [InlineData("1.2.*-beta.*",  "1.3.0-beta.0")]
      [InlineData("1.*-beta.*",    "1.0.0-beta0")]
      [InlineData("1.*-beta.*",    "2.0.0-beta.0")]
      [InlineData("1.*-beta.*",    "0.1.0-beta.0")]
      [InlineData("*-beta.*",      "0.0.0-beta0")]
      public void Satisfies_False(string version, string satisfiesVersion)
      {
         var satisfies = PunditVersion.Parse(satisfiesVersion);
         var floatVersion = FloatRange.Parse(version);

         var result = floatVersion.Satisfies(satisfies);

         Assert.False(result);
      }
   }
}
