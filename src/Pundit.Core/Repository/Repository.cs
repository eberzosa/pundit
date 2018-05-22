using System;
using System.IO;
using EBerzosa.Utils;

namespace EBerzosa.Pundit.Core.Repository
{
   public abstract class Repository
   {
      public string Name { get; }

      public string RootPath { get; }
      
      public bool CanPublish { get; set; }

      public RepositoryType Type { get; }


      protected Repository(string rootPath, string name, RepositoryType type)
      {
         Guard.NotNull(rootPath, nameof(rootPath));

         if (!rootPath.StartsWith("http", StringComparison.OrdinalIgnoreCase) && !Directory.Exists(rootPath))
            throw new ArgumentException($"Root directory '{rootPath}' does not exist");

         RootPath = rootPath;
         Name = name;
         Type = type;
      }

      public override string ToString() => $"{Name} [{RootPath}]";
   }
}
