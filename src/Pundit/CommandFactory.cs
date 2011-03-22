﻿using System;
using Pundit.Console.Commands;

namespace Pundit.Console
{
   class CommandFactory
   {
      public static ICommand CreateCommand(string[] cmdline)
      {
         if(cmdline == null || cmdline.Length == 0)
            throw new ArgumentException("command line is empty", "cmdline");

         string command = cmdline[0];
         string[] parameters = new string[cmdline.Length - 1];
         Array.Copy(cmdline, 1, parameters, 0, cmdline.Length - 1);

         ICommand cmd;

         switch (command)
         {
            case "pack":
               cmd = new PackCommand(parameters);
               break;
            case "publish":
               cmd = new PublishCommand(parameters);
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
