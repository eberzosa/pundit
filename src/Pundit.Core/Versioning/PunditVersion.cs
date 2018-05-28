namespace EBerzosa.Pundit.Core.Versioning
{
   public class PunditVersion
   {
      private readonly NuGet.Versioning.NuGetVersion _nuGetVersion;

      private PunditVersion(NuGet.Versioning.NuGetVersion nuGetVersion)
      {
         _nuGetVersion = nuGetVersion;
      }

      public static implicit operator PunditVersion(NuGet.Versioning.NuGetVersion nuGetVersion)
      {
         return new PunditVersion(nuGetVersion);
      }
   }
}
