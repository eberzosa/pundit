using NuGet.Frameworks;

namespace EBerzosa.Pundit.Core.Mappings
{
   internal class FrameworkMappings
   {
      private static bool _registered;

      public static void Initialise()
      {
         if (_registered)
            return;

         _registered = true;

         Mapster.TypeAdapterConfig<NuGetFramework, string>.NewConfig()
            .ConstructUsing(fmwk => fmwk.GetShortFolderName());
      }
   }
}
