using System.Collections.Generic;
using System.Globalization;
using EBerzosa.Pundit.Core.Utils;
using Xunit;

namespace Pundit.Test.Utils
{
   public class ByteFormatProviderTest
   {
      public static IEnumerable<object[]> GetBase10Numbers()
      {
         yield return new object[] {                            1,       "1 B",         "1 B"};
         yield return new object[] {                          999,     "999 B",       "999 B"};
         yield return new object[] {                         1000,      "1 kB",     "1,00 kB"};
         yield return new object[] {                       999450,    "999 kB",   "999,45 kB"};
         yield return new object[] {                      1000000,      "1 MB",     "1,00 MB"};
         yield return new object[] {                    999450000,    "999 MB",   "999,45 MB"};
         yield return new object[] {                   1000000000,      "1 GB",     "1,00 GB"};
         yield return new object[] {                 999450000000,    "999 GB",   "999,45 GB"};
         yield return new object[] {                1000000000000,      "1 TB",     "1,00 TB"};
         yield return new object[] {              999450000000000,    "999 TB",   "999,45 TB"};
         yield return new object[] {             1000000000000000,      "1 PB",     "1,00 PB"};
         yield return new object[] {           999450000000000000,    "999 PB",   "999,45 PB"};
         yield return new object[] {          1000000000000000000,      "1 EB",     "1,00 EB"};
         yield return new object[] {       999450000000000000000f,    "999 EB",   "999,45 EB"};
         yield return new object[] {      1000000000000000000000f,      "1 ZB",     "1,00 ZB"};
         yield return new object[] {    999450000000000000000000f,    "999 ZB",   "999,45 ZB"};
         yield return new object[] {   1000000000000000000000000f,      "1 YB",     "1,00 YB"};
         yield return new object[] { 999450000000000000000000000f,    "999 YB",   "999,45 YB"};
         yield return new object[] {1000000000000000000000000000f,  "1.000 YB", "1.000,00 YB"};
      }

      public static IEnumerable<object[]> GetBase2Numbers()
      {
         yield return new object[] {                            1,       "1 B",         "1 B"};
         yield return new object[] {                         1023,   "1.023 B",     "1.023 B"};
         yield return new object[] {                         1024,     "1 KiB",     "1,00 KiB"};
         yield return new object[] {                      1048012, "1.023 KiB", "1.023,45 KiB"};
         yield return new object[] {                      1048576,     "1 MiB",     "1,00 MiB"};
         yield return new object[] {                   1073165107, "1.023 MiB", "1.023,45 MiB"};
         yield return new object[] {                   1073741824,     "1 GiB",     "1,00 GiB"};
         yield return new object[] {                1098921069772, "1.023 GiB", "1.023,45 GiB"};
         yield return new object[] {                1099511627776,     "1 TiB",     "1,00 TiB"};
         yield return new object[] {             1125295175447347, "1.023 TiB", "1.023,45 TiB"};
         yield return new object[] {             1125899906842624,     "1 PiB",     "1,00 PiB"};
         yield return new object[] {          1152302259658083532, "1.023 PiB", "1.023,45 PiB"};
         yield return new object[] {          1152921504606846976,     "1 EiB",     "1,00 EiB"};
         yield return new object[] {      1179957513889877537587f, "1.023 EiB", "1.023,45 EiB"};
         yield return new object[] {      1180591620717411303424f,     "1 ZiB",     "1,00 ZiB"};
         yield return new object[] {   1208276494223234598489292f, "1.023 ZiB", "1.023,45 ZiB"};
         yield return new object[] {   1208925819614629174706176f,     "1 YiB",     "1,00 YiB"};
         yield return new object[] {1237275130084592228853035827f, "1.023 YiB", "1.023,45 YiB"};
         yield return new object[] {1237940039285380274899124224f, "1.024 YiB", "1.024,00 YiB"};
      }

      [Theory]
      [MemberData(nameof(GetBase10Numbers))]
      public void Base10_NoDecimals_Test(decimal value, string expected, string notUsed)
      {
         var result = string.Format(new ByteFormatProvider(), "{0:bf0}", value);

         Assert.Equal(expected.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator), result);
      }

      [Theory]
      [MemberData(nameof(GetBase10Numbers))]
      public void Base10_DefaultDecimals_Test(decimal value, string notUsed, string expected)
      {
         var result = string.Format(new ByteFormatProvider(), "{0:bf}", value);

         expected = expected.Replace(".", "~").Replace(",", "±")
            .Replace("~", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
            .Replace("±", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

         Assert.Equal(expected, result);
      }

      [Theory]
      [MemberData(nameof(GetBase2Numbers))]
      public void Base2_NoDecimals_Test(decimal value, string expected, string notUsed)
      {
         var result = string.Format(new ByteFormatProvider(), "{0:bfi0}", value);

         Assert.Equal(expected.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator), result);
      }

      [Theory]
      [MemberData(nameof(GetBase2Numbers))]
      public void Base2_DefaultDecimals_Test(decimal value, string notUsed, string expected)
      {
         var result = string.Format(new ByteFormatProvider(), "{0:bfi}", value);

         expected = expected.Replace(".", "~").Replace(",", "±")
            .Replace("~", CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator)
            .Replace("±", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

         Assert.Equal(expected, result);
      }
   }
}
