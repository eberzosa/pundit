using Pundit.Core.Model;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public interface IResolutionResult
   {
      VersionResolutionTable ResolutionTable { get; }

      DependencyNode DependencyNode { get; }
   }
}