using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Exceptions
{
   public class RemoteRepositoryException : Exception
   {
      public RemoteRepositoryException(string message, Exception inner) : base(message, inner)
      {
         
      }
   }
}
