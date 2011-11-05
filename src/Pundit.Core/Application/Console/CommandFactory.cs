using System;
using Pundit.Core.Application.Console.Commands;
using Pundit.Core.Model;

namespace Pundit.Core.Application.Console
{
   /// <summary>
   /// 
   /// </summary>
   public class CommandFactory
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="console"></param>
      /// <param name="currentDirectory"></param>
      /// <param name="cmdline"></param>
      /// <returns></returns>
      /// <exception cref="ArgumentException"></exception>
      public static IConsoleCommand CreateCommand(IConsoleOutput console, string currentDirectory, string[] cmdline)
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
               cmd = new PackConsoleCommand(console, currentDirectory, parameters);
               break;
            case "publish":
               cmd = new PublishConsoleCommand(console, currentDirectory, parameters);
               break;
            case "template":
               cmd = new TemplateConsoleCommand(console, currentDirectory, parameters);
               break;
            case "resolve":
               cmd = new ResolveConsoleCommand(console, currentDirectory, parameters);
               break;
            case "search":
               cmd = new SearchConsoleCommand(console, currentDirectory, parameters);
               break;
            case "utils":
               cmd = new UtilsConsoleCommand(console, currentDirectory, parameters);
               break;
            case "update":
               cmd = new UpdateConsoleCommand(console, currentDirectory, parameters);
               break;
            case "info":
               cmd = new InfoConsoleCommand(console, currentDirectory, parameters);
               break;
            case "repo":
               cmd = new RepoConsoleCommand(console, currentDirectory, parameters);
               break;
            case "help":
               cmd = new HelpConsoleCommand(console, currentDirectory, parameters);
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
