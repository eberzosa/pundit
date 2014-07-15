using System;
using System.IO;
using Pundit.Core.Application;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Represents a secure key used for calling rest API
   /// </summary>
   public class ApiKey
   {
      private const int Version = 1;

      private DateTime _issuedOn;
      private DateTime _expiresOn;

      public ApiKey(DateTime issuedOn, DateTime expiresOn)
      {
         _issuedOn = issuedOn;
         _expiresOn = expiresOn;
      }

      public DateTime IssuedOn { get { return _issuedOn; } }

      public DateTime ExpiresOn { get { return _expiresOn; } }

      public string ToString(string encryptionKey)
      {
         byte[] data = new byte[4 + 8 + 8];
         byte[] b0 = BitConverter.GetBytes(Version);
         byte[] b1 = BitConverter.GetBytes(_issuedOn.ToBinary());
         byte[] b2 = BitConverter.GetBytes(_expiresOn.ToBinary());

         Array.Copy(b0, 0, data, 0, 4);
         Array.Copy(b1, 0, data, 4, 8);
         Array.Copy(b2, 0, data, 12, 8);

         byte[] encrypted = Encryption.Encrypt(data, encryptionKey);
         return Convert.ToBase64String(encrypted);
      }

      public static ApiKey FromString(string s, string encryptionKey)
      {
         byte[] encrypted = Convert.FromBase64String(s);
         byte[] data = Encryption.Decrypt(encrypted, encryptionKey);
         int version = BitConverter.ToInt32(data, 0);
         if(version != 1) throw new InvalidDataException("expected version 1 but found " + version);

         long issuedOn = BitConverter.ToInt64(data, 4);
         long expiresOn = BitConverter.ToInt64(data, 12);

         return new ApiKey(DateTime.FromBinary(issuedOn), DateTime.FromBinary(expiresOn));
      }
   }
}
