using System;
using Pundit.Core.Application.Console.Commands;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Console
{
   public class CommandFactory
   {
      public static IConsoleCommand CreateCommand(IConsoleOutput console, string[] cmdline)
      {
         if(cmdline == null || cmdline.Length == 0)
            throw new ArgumentException("command line is empty");

         string command = cmdline[0];
         string[] parameters = new string[cmdline.Length - 1];
         Array.Copy(cmdline, 1, parameters, 0, cmdline.Length - 1);

         IConsoleCommand cmd;

         switch (command)
         {
            case "pack":
               cmd = new PackConsoleCommand(console, parameters);
               break;
            case "publish":
               cmd = new PublishConsoleCommand(console, parameters);
               break;
            case "template":
               cmd = new TemplateConsoleCommand(console, parameters);
               break;
            case "resolve":
               cmd = new ResolveConsoleCommand(console, parameters);
               break;
            case "search":
               cmd = new SearchConsoleCommand(console, parameters);
               break;
            case "utils":
               cmd = new UtilsConsoleCommand(console, parameters);
               break;
            case "update":
               cmd = new UpdateConsoleCommand(console, parameters);
               break;
            case "info":
               cmd = new InfoConsoleCommand(console, parameters);
               break;
            case "help":
               cmd = new HelpConsoleCommand(console, parameters);
               break;
            default:
               cmd = null;
               break;
         }

         if(cmd == null)
            throw new ArgumentException("unknown command: " + command, "cmdline");

         return cmd;
      }
   }
}
