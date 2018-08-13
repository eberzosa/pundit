using System.Collections.Generic;
using System.Text;
using EBerzosa.Pundit.Core.Model;
using EBerzosa.Pundit.Core.Model.Package;
using EBerzosa.Pundit.Core.Repository;

namespace EBerzosa.Pundit.Core.Resolvers
{
   public class DependencyResolver
   {
      private readonly IWriter _writer;

      public DependencyResolver(IWriter writer)
      {
         _writer = writer;
      }

      public IResolutionResult Resolve(PackageManifestRoot rootManifest, IRepository[] activeRepositories, string releaseLabel)
      {
         var resolver = new DependencyResolverImplementation(_writer, rootManifest, activeRepositories, releaseLabel);
         resolver.Resolve();

         return resolver;
      }

      public string DescribeConflict(DependencyNode rootNode, UnresolvedPackage package, ICollection<DependencyNode> collector)
      {
         var sb = new StringBuilder();
         var found = new List<DependencyNode>();

         FindNodes(rootNode, package, found);

         if (found.Count <= 0)
            return null;

         foreach (var node in found)
            collector.Add(node);

         AppendDependencyNodes(found, sb);

         return sb.ToString();
      }

      public string PrintDependencyNodes(IEnumerable<DependencyNode> nodes)
      {
         var sb = new StringBuilder();
         AppendDependencyNodes(nodes, sb);

         return sb.ToString();
      }

      private static void FindNodes(DependencyNode rootNode, UnresolvedPackage package, ICollection<DependencyNode> collector)
      {
         if (rootNode.UnresolvedPackage.Equals(package))
            collector.Add(rootNode);

         foreach (DependencyNode child in rootNode.Children)
            FindNodes(child, package, collector);
      }

      private void AppendDependencyNodes(IEnumerable<DependencyNode> nodes, StringBuilder sb)
      {
         foreach (var node in nodes)
            AppendDependencyNode(node, sb);
      }

      private void AppendDependencyNode(DependencyNode node, StringBuilder sb)
      {
         if (sb.Length != 0)
            sb.AppendLine();

         sb.Append("dependency: [")
            .Append(node.Path)
            .Append("], version: [")
            .Append(node.AllowedVersions)
            .Append("], resolved to: [");

         var isFirst = true;
         foreach (var version in node.AllVersions)
         {
            if (!isFirst)
               sb.Append(", ");
            else
               isFirst = false;

            sb.Append(version);
         }

         sb.Append("]");
      }

   }
}