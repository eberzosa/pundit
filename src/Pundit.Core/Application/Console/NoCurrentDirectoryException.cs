using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Application.Console
{
   /// <summary>
   /// 
   /// </summary>
   public class NoCurrentDirectoryException : Exception
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="message"></param>
      public NoCurrentDirectoryException(string message) : base(message)
      {
         
      }
   }
}
