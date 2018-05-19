using System;
using System.Collections.Generic;
using System.Text;

namespace EBerzosa.Pundit.Core.Model
{
   public class InvalidPackageException : Exception
   {
      private readonly List<KeyValuePair<string, string>> _listOfErrorMessages = new List<KeyValuePair<string, string>>();

      public InvalidPackageException() 
         : base("Invalid Package")
      {
      }

      public InvalidPackageException(string message) 
         : base(message)
      {  
      }
      
      public void AddError(string propertyName, string errorMessage) 
         => _listOfErrorMessages.Add(new KeyValuePair<string, string>(propertyName, errorMessage));

      public bool HasErrors => _listOfErrorMessages.Count > 0;


      public override string ToString()
      {
         var sb = new StringBuilder();

         foreach(var kvp in _listOfErrorMessages)
         {
            if (sb.Length > 0)
               sb.AppendLine();

            sb.Append("Property: ").Append(kvp.Key).Append(", Error: ").Append(kvp.Value);
         }

         return sb.ToString();
      }
   }
}
