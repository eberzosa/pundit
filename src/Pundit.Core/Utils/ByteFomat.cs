using System;

namespace Pundit.Core
{
	public static class ByteFormat
   {

      //http://en.wikipedia.org/wiki/Kibibyte

// ReSharper disable InconsistentNaming
      private const ulong KB = 1000;         //kilobyte
      private const ulong KiB = 1024;        //kikibyte
      private const ulong MB = KB*1000;      //megabyte
      private const ulong MiB = KiB*1024;    //memibyte
      private const ulong GB = MB*1000;      //gigabyte
      private const ulong GiB = MiB*1024;    //gigibyte
      private const ulong TB = GB*1000;      //terabyte
      private const ulong TiB = GiB*1024;    //tebibyte
      private const ulong PB = TB*1024;      //petabyte
      private const ulong PiB = TiB*1024;    //pepibyte
// ReSharper restore InconsistentNaming

      public enum Standard
      {
         /// <summary>
         ///  International System of Units
         /// </summary>
         SI,

         /// <summary>
         /// International Electrotechnical Commission
         /// </summary>
         IEC
      }

      /// <summary>
      /// Returns the best formatted string representation of a byte value
      /// </summary>
      /// <param name="bytes">number of bytes</param>
      /// <param name="st"></param>
      /// <returns>formatted string</returns>
      public static string ToString(ulong bytes, Standard st = Standard.IEC)
      {
          return ToString(bytes, st, null);
      }

       /// <summary>
      /// Returns the best formatted string representation of a byte value
      /// </summary>
      /// <param name="bytes">number of bytes</param>
      /// <param name="st"></param>
      /// <param name="customFormat">Defines a custom numerical format for the conversion.
      /// If this parameters is null or empty the default format will be used 0.00</param>
      /// <returns>formatted string</returns>
      public static string ToString(ulong bytes, Standard st, string customFormat)
      {
          if (string.IsNullOrEmpty(customFormat))
              customFormat = "0.00";

          if (st == Standard.SI)
          {
              if (bytes < MB)
                  return BytesToKb(bytes, customFormat);

              if (bytes < GB)
                  return BytesToMb(bytes, customFormat);

              if (bytes < TB)
                  return BytesToGb(bytes, customFormat);

              if (bytes < PB)
                  return BytesToTb(bytes, customFormat);

              return BytesToPb(bytes, customFormat);
          }
          else
          {
              if (bytes < MiB)
                  return BytesToKib(bytes, customFormat);

              if (bytes < GiB)
                  return BytesToMib(bytes, customFormat);

              if (bytes < TiB)
                  return BytesToGib(bytes, customFormat);

              if (bytes < PiB)
                  return BytesToTib(bytes, customFormat);

              return BytesToPib(bytes, customFormat);
          }
      }

      private static string BytesToPb(ulong bytes, string customFormat)
      {
          double tb = ((double)bytes) / ((double)PB);
          return tb.ToString(customFormat) + " PB";
      }
      private static string BytesToPib(ulong bytes, string customFormat)
      {
          double tb = ((double)bytes) / ((double)PB);
          return tb.ToString(customFormat) + " PiB";
      }

      private static string BytesToTb(ulong bytes, string customFormat)
      {
          double tb = ((double)bytes) / ((double)TB);
          return tb.ToString(customFormat) + " TB";
      }
      private static string BytesToTib(ulong bytes, string customFormat)
      {
          double tb = ((double)bytes) / ((double)TB);
          return tb.ToString(customFormat) + " TiB";
      }

      private static string BytesToGb(ulong bytes, string customFormat)
      {
          double gb = ((double)bytes) / ((double)GB);
          return gb.ToString(customFormat) + " GB";
      }
      private static string BytesToGib(ulong bytes, string customFormat)
      {
          double gb = ((double)bytes) / ((double)GB);
          return gb.ToString(customFormat) + " GiB";
      }

      private static string BytesToMb(ulong bytes, string customFormat)
      {
          double mb = ((double)bytes) / ((double)MB);
          return mb.ToString(customFormat) + " MB";
      }
      private static string BytesToMib(ulong bytes, string customFormat)
      {
          double mb = ((double)bytes) / ((double)MB);
          return mb.ToString(customFormat) + " MiB";
      }

      private static string BytesToKb(ulong bytes, string customFormat)
      {
          double kb = ((double)bytes) / ((double)KB);
          return kb.ToString(customFormat) + " KB";
      }
      private static string BytesToKib(ulong bytes, string customFormat)
      {
          double kb = ((double)bytes) / ((double)KB);
          return kb.ToString(customFormat) + " KiB";
      }
   }

}

