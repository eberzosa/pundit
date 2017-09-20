using EBerzosa.Pundit.Core.Application;
using EBerzosa.Utils;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Repository
{
   public class RepositoryFactory
   {
      private readonly PackageReaderFactory _packageReaderFactory;

      public RepositoryFactory(PackageReaderFactory packageReaderFactory)
      {
         Guard.NotNull(packageReaderFactory, nameof(packageReaderFactory));

         _packageReaderFactory = packageReaderFactory;
      }

      public IRepository CreateFromUri(string uri)
      {
         return new FileRepository(_packageReaderFactory, uri);
      }
   }
}
