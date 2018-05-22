namespace EBerzosa.Pundit.Core.Services
{
   public class UtilService
   {
      public string EncryptNuGetApiKey(string key)
      {
         return NuGet.Configuration.EncryptionUtility.EncryptString(key);
      }
   }
}
