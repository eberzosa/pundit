using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBerzosa.Pundit.Core.Model;
using Xunit;

namespace Pundit.Test.Core
{
   public class PunditVersionTest
   {
      [Theory]
      [InlineData("1.0.0.1", "1.0.0.0")]
      [InlineData("1.0.1.0", "1.0.0.0")]
      [InlineData("1.1.0.0", "1.0.0.0")]
      [InlineData("2.0.0.0", "1.0.0.0")]
      [InlineData("1.0.0.2", "1.0.0.1")]
      [InlineData("1.0.1.1", "1.0.1.0")]
      [InlineData("1.1.0.1", "1.1.0.0")]
      [InlineData("1.1.1.0", "1.1.0.0")]
      [InlineData("1.0.0.0", "1.0.0.0-dev")]
      [InlineData("1.0.0.1", "1.0.0.0-dev")]
      [InlineData("1.0.0.1-dev", "1.0.0.0-dev")]
      public void VersionIsGreaterThanValue_Test(string versionStr, string valueStr)
      {
         var version = new PunditVersion(versionStr);
         var value = new PunditVersion(valueStr);

         Assert.True(version.CompareTo(value) == 1);
      }

      [Theory]
      [InlineData("1.0.0.0", "1.0.0.0")]
      [InlineData("1.0.1.0", "1.0.1.0")]
      [InlineData("1.1.0.0", "1.1.0.0")]
      [InlineData("2.0.0.0", "2.0.0.0")]
      [InlineData("1.0.0.0-dev", "1.0.0.0-dev")]
      [InlineData("1.0.0.1-dev", "1.0.0.1-dev")]
      public void VersionIsEqualToValue_Test(string versionStr, string valueStr)
      {
         var version = new PunditVersion(versionStr);
         var value = new PunditVersion(valueStr);

         Assert.True(version.CompareTo(value) == 0);
      }

      [Theory]
      [InlineData("1.0.0.0", "1.0.0.1")]
      [InlineData("1.0.0.0", "1.0.1.0")]
      [InlineData("1.0.0.0", "1.1.0.0")]
      [InlineData("1.0.0.0", "2.0.0.0")]
      [InlineData("1.0.0.1", "1.0.0.2")]
      [InlineData("1.0.1.0", "1.0.1.1")]
      [InlineData("1.1.0.0", "1.1.0.1")]
      [InlineData("1.1.0.0", "1.1.1.0")]
      [InlineData("1.0.0.0-dev", "1.0.0.0")]
      [InlineData("1.0.0.0-dev", "1.0.0.1")]
      [InlineData("1.0.0.0-dev", "1.0.0.1-dev")]
      public void VersionIsLowerThanValue_Test(string versionStr, string valueStr)
      {
         var version = new PunditVersion(versionStr);
         var value = new PunditVersion(valueStr);

         Assert.True(version.CompareTo(value) == -1);
      }
   }
}
