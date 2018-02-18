using EBerzosa.Pundit.Core.Repository.Xml;

namespace EBerzosa.Pundit.Core.Repository.Mappings
{
   public class RepositoryMappings
   {
      private static bool _registered;

      public static void Initalise()
      {
         if (_registered)
            return;

         _registered = true;

         Mapster.TypeAdapterConfig<XmlRegisteredRepositories, RegisteredRepositories>.NewConfig();

         Mapster.TypeAdapterConfig<XmlRegisteredRepository, RegisteredRepository>.NewConfig();
      }
   }
}
