using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBerzosa.Pundit.Core;
using EBerzosa.Pundit.Core.Utils;
using NuGet.Versioning;
using Xunit;

namespace Pundit.Test.Utils
{
   public class VersionUtilsTest
   {
      [Fact]
      public void AllVersionsNoDev() => Assert.Equal("*-*", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("*"), false));

      [Fact]
      public void MajorNoDev() => Assert.Equal("1.*-*", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("1.*"), false));

      [Fact]
      public void MinorNoDev() => Assert.Equal("1.2.*-*", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("1.2.*"), false));

      [Fact]
      public void PatchNoDev() => Assert.Equal("1.2.3-*", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("1.2.3.*"), false));
      
      [Fact]
      public void ReleaseNoDev() => Assert.Equal("1.2.3-4", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("1.2.3.4"), false));

      [Fact]
      public void AllVersionsDev() => Assert.Equal("*-dev*", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("*"), true));

      [Fact]
      public void MajorDev() => Assert.Equal("1.*-dev*", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("1.*"), true));

      [Fact]
      public void MinorDev() => Assert.Equal("1.2.*-dev*", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("1.2.*"), true));

      [Fact]
      public void PatchDev() => Assert.Equal("1.2.3-dev*", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("1.2.3.*"), true));

      [Fact]
      public void ReleaseDev() => Assert.Equal("1.2.3-dev4", VersionUtils.ToPunditSearchVersion(VersionRange.Parse("1.2.3.4"), true));
   }
}
