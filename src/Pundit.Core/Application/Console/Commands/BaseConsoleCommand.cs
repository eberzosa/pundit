using System;
using System.IO;
using NDesk.Options;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   abstract class BaseConsoleCommand : IConsoleCommand
   {
      private readonly string[] _args;
      protected readonly IConsoleOutput console;
      protected readonly string currentDirectory;

      protected BaseConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args)
      {
         if (console == null) throw new ArgumentNullException("console");

         this.console = console;
         this.currentDirectory = currentDirectory;
         _args = args;
      }

      protected string[] GetCommandLine()
      {
         return _args;
      }

      protected string GetLocalManifest()
      {
         string manifestPath = null;

         new OptionSet().Add("m:|manifest:", i => manifestPath = i).Parse(_args);

         if (manifestPath != null)
         {
            if (!Path.IsPathRooted(manifestPath))
               manifestPath = Path.Combine(currentDirectory, PathUtils.GetOSPath(manifestPath));
         }
         else
         {
            manifestPath = Path.Combine(currentDirectory, Package.DefaultManifestFileName);
         }

         if (!File.Exists(manifestPath))
            throw new ArgumentException("manifest doesn't exist at [" + manifestPath + "]");

         return manifestPath;
      }

      protected int GetDepth()
      {
         int depth = int.MaxValue;

         string sdepth = null;

         new OptionSet()
            .Add("d:|depth:", d => sdepth = d)
            .Parse(_args);

         if(sdepth != null)
         {
            if(sdepth == "local")
               depth = 0;
            else if (!int.TryParse(sdepth, out depth))
               throw new ArgumentException("wrong depth: " + sdepth);
         }

         return depth;
      }

      protected string GetText()
      {
         string text = null;

         new OptionSet().Add("t:|text:|s:|string:", s => text = s).Parse(_args);

         if(text == null)
            throw new ArgumentException("search text not specified");

         return text;
      }

      public abstract void Execute();
   }
}
