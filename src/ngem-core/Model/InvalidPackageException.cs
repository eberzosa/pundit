using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGem.Core.Model
{
   public class InvalidPackageException : Exception
   {
      private readonly List<KeyValuePair<string, string>> _listOfErrorMessages = new List<KeyValuePair<string, string>>();

      public InvalidPackageException() : base("invalid package")
      {
         
      }

      public List<KeyValuePair<string, string>> ErrorMessages
      {
         get { return _listOfErrorMessages; }
      }

      public void AddError(string propertyName, string errorMessage)
      {
         _listOfErrorMessages.Add(new KeyValuePair<string, string>(propertyName, errorMessage));
      }

      public bool HasErrors
      {
         get { return _listOfErrorMessages.Count > 0; }
      }
   }
}
