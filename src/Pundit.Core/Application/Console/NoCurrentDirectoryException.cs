using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Application.Console
{
   public class NoCurrentDirectoryException : Exception
   {
      public NoCurrentDirectoryException(string message) : base(message)
      {
         
      }
   }
}
