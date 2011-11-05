using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NDesk.Options;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   abstract class BaseConsoleCommand : IConsoleCommand
   {
      private readonly string[] _args;
      private string[] _nameless;
      private string[] _named;
      protected readonly IConsoleOutput console;
      private readonly string _currentDirectory;

      protected BaseConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args)
      {
         if (console == null) throw new ArgumentNullException("console");

         this.console = console;
         _currentDirectory = currentDirectory;
         _args = args;

         ParseCommandLine();
      }

      private void ParseCommandLine()
      {
         if(_args != null)
         {
            _nameless = _args.Where(s => !s.StartsWith("-")).ToArray();
            _named = _args.Where(s => s.StartsWith("-")).ToArray();
         }
         else
         {
            _nameless = new string[0];
            _named = new string[0];
         }
      }

      protected string[] GetCommandLine()
      {
         return _args;
      }

      protected string CurrentDirectory
      {
         get
         {
            if(string.IsNullOrEmpty(_currentDirectory))
               throw new NoCurrentDirectoryException("this command requires current directory");

            return _currentDirectory; 
         }
      }

      protected string[] NamelessParameters
      {
         get { return _nameless; }
      }

      protected string GetParameter(string spec, int namelessIndex = -1)
      {
         string v = null;

         if (spec != null)
            new OptionSet().Add(spec, vi => v = vi).Parse(_named);

         if (v == null && namelessIndex != -1 && namelessIndex < _nameless.Length)
            v = _nameless[namelessIndex];

         return v;
      }

      protected bool GetBoolParameter(bool defaultValue, string spec, int namelessIndex = -1)
      {
         string v = GetParameter(spec, namelessIndex);
         return v != null;
      }

      protected string GetLocalManifest()
      {
         string manifestPath = null;

         new OptionSet().Add("m:|manifest:", i => manifestPath = i).Parse(_args);

         if (manifestPath != null)
         {
            if (!Path.IsPathRooted(manifestPath))
               manifestPath = Path.Combine(CurrentDirectory, PathUtils.GetOSPath(manifestPath));
         }
         else
         {
            manifestPath = Path.Combine(CurrentDirectory, Package.DefaultManifestFileName);
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

         if (text == null && NamelessParameters.Length > 0)
            text = NamelessParameters[0];

         if(text == null)
            throw new ArgumentException("search text not specified");

         return text;
      }

      public abstract void Execute();
   }
}
