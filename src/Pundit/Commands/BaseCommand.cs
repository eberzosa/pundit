using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using log4net;
using NDesk.Options;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Console.Commands
{
   abstract class BaseCommand : ICommand
   {
      private string[] _args;
      protected readonly ILog Log = LogManager.GetLogger(typeof (BaseCommand));

      protected BaseCommand(string[] args)
      {
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
               manifestPath = Path.Combine(Environment.CurrentDirectory, PathUtils.GetOSPath(manifestPath));
         }
         else
         {
            manifestPath = Path.Combine(Environment.CurrentDirectory, Package.DefaultPackageFileName);
         }

         if (!File.Exists(manifestPath))
            throw new ArgumentException("manifest doesn't exist at [" + manifestPath + "]");

         return manifestPath;
      }

      public abstract void Execute();
   }
}
