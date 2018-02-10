using ExpressMapper;
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

         Mapper.RegisterCustom<NuGetFramework, string>(fmwk => fmwk.GetShortFolderName());
      }
   }
}
