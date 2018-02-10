using System;

namespace EBerzosa.Pundit.Core.Utils
{
   public class ByteFormatProvider : IFormatProvider, ICustomFormatter
   {
      public const string Base10 = "bf";
      public const string Base2 = Base10 + "i";

      private static readonly string[] Base10Units = {"B", "kB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
      private static readonly string[] Base2Units = {"B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"};

      private static readonly decimal[] Base10Divisor =
      {
         1m,
         1000m,
         1000000m,
         1000000000m,
         1000000000000m,
         1000000000000000m,
         1000000000000000000m,
         1000000000000000000000m,
         1000000000000000000000000m
      };

      private static readonly decimal[] Base2Divisor =
      {
         1m, 
         1024m,
         1048576m,
         1073741824m,
         1099511627776m,
         1125899906842624m,
         1152921504606846976,
         1180591620717411303424m,
         1208925819614629174706176m
      };

      public object GetFormat(Type formatType) => formatType == typeof(ICustomFormatter) ? this : null;

      public string Format(string format, object arg, IFormatProvider formatProvider)
      {
         format = format?.ToLowerInvariant();

         if (format == null || !format.StartsWith(Base10) || arg is string)
            return DefaultFormat(format, arg, formatProvider);

         decimal value;

         try
         {
            value = Convert.ToDecimal(arg);
         }
         catch (InvalidCastException)
         {
            return DefaultFormat(format, arg, formatProvider);
         }

         GetBaseAndPrecission(format, out var byteBase, out var precision);

         GetValueAndUnit(value, byteBase, out var outValue, out var unit);

         if (unit == (byteBase == Base.Base10 ? Base10Units[0] : Base2Units[0]))
            precision = "0";

         return string.Format("{0:N" + precision + "} {1}", outValue, unit);
      }

      private void GetBaseAndPrecission(string format, out Base byteBase, out string precission)
      {
         if (format.StartsWith(Base2))
         {
            byteBase = Base.Base2;
            precission = format.Length > 3 ? format.Substring(3) : "2";
         }
         else
         {
            byteBase = Base.Base10;
            precission = format.Length > 2 ? format.Substring(2) : "2";
         }
      }

      private static void GetValueAndUnit(decimal value, Base divisorBase, out decimal outValue, out string unit)
      {
         for (int i = 8; i >= 0; i--)
            if (GetDividedValue(value, divisorBase, i, out outValue, out unit))
               return;

         throw new NotSupportedException();
      }

      private static bool GetDividedValue(decimal value, Base byteBase, int times, out decimal outValue, out string unit)
      {
         var divisor = byteBase == Base.Base10 ? Base10Divisor[times] : Base2Divisor[times];

         if (value >= divisor || (value == 0 && times == 0))
         {
            outValue = value / divisor;
            unit = byteBase == Base.Base10 ? Base10Units[times] : Base2Units[times];

            return true;
         }

         outValue = 0;
         unit = null;
         return false;
      }

      

      private static string DefaultFormat(string format, object arg, IFormatProvider formatProvider)
      {
         return arg is IFormattable formattableArg
            ? formattableArg.ToString(format, formatProvider)
            : arg.ToString();
      }

      private enum Base
      {
         Base2,
         Base10
      }
   }
}
