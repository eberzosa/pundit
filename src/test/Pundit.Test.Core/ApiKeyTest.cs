using System;
using NUnit.Framework;
using Pundit.Core.Model;

namespace Pundit.Test
{
   [TestFixture]
   public class ApiKeyTest
   {
      private const string Key = "bite my ass";

      [Test]
      public void EncryptDecryptTest()
      {
         ApiKey key1 = new ApiKey(DateTime.UtcNow, DateTime.UtcNow.AddDays(1));
         string skey1 = key1.ToString(Key);

         ApiKey key2 = ApiKey.FromString(skey1, Key);
         Assert.AreEqual(key1.IssuedOn, key2.IssuedOn);
         Assert.AreEqual(key1.ExpiresOn, key2.ExpiresOn);
      }
   }
}
