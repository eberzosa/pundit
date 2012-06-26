using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Pundit.Core.Application
{
   static class Encryption
   {
      private static readonly byte[] DefaultSalt =
         new byte[]
            {
               0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            };

      /// <summary>
      /// 
      /// </summary>
      /// <param name="data"></param>
      /// <param name="key"></param>
      /// <param name="iv">Initialization vector</param>
      /// <returns></returns>
      public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
      {
         if (data == null) throw new ArgumentNullException("data");
         if (key == null) throw new ArgumentNullException("key");
         if (iv == null) throw new ArgumentNullException("iv");

         var ms = new MemoryStream();

         //Rijndael is strong and available on all platforms
         Rijndael alg = Rijndael.Create();
         alg.Key = key;
         alg.IV = iv;

         using (var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
         {
            cs.Write(data, 0, data.Length);
         }
         return ms.ToArray();
      }

      public static string Encrypt(string data, string key)
      {
         if (data == null) throw new ArgumentNullException("data");

         byte[] byteData = Encoding.Unicode.GetBytes(data);
         var pdb = new PasswordDeriveBytes(key, DefaultSalt);
         byte[] encrypted = Encrypt(byteData, pdb.GetBytes(32), pdb.GetBytes(16));
         return Convert.ToBase64String(encrypted);
      }

      public static byte[] Encrypt(byte[] data, string key)
      {
         var pdb = new PasswordDeriveBytes(key, DefaultSalt);
         return Encrypt(data, pdb.GetBytes(32), pdb.GetBytes(16));         
      }

      public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
      {
         var ms = new MemoryStream();
         Rijndael alg = Rijndael.Create();
         alg.Key = key;
         alg.IV = iv;

         using(var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
         {
            cs.Write(data, 0, data.Length);
         }
         return ms.ToArray();
      }

      public static string Decrypt(string data, string key)
      {
         byte[] byteData = Convert.FromBase64String(data);
         var pdb = new PasswordDeriveBytes(key, DefaultSalt);
         byte[] decryptedBytes = Decrypt(byteData, pdb.GetBytes(32), pdb.GetBytes(16));
         return Encoding.Unicode.GetString(decryptedBytes);
      }

      public static byte[] Decrypt(byte[] data, string key)
      {
         var pdb = new PasswordDeriveBytes(key, DefaultSalt);
         return Decrypt(data, pdb.GetBytes(32), pdb.GetBytes(16));
      }
   }
}
