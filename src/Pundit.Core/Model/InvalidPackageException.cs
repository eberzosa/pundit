using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   public class InvalidPackageException : Exception
   {
      private readonly List<KeyValuePair<string, string>> _listOfErrorMessages = new List<KeyValuePair<string, string>>();

      /// <summary>
      /// 
      /// </summary>
      public InvalidPackageException() : base("invalid package")
      {
         
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="message"></param>
      public InvalidPackageException(string message) : base(message)
      {
         
      }

      /// <summary>
      /// 
      /// </summary>
      public List<KeyValuePair<string, string>> ErrorMessages
      {
         get { return _listOfErrorMessages; }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="propertyName"></param>
      /// <param name="errorMessage"></param>
      public void AddError(string propertyName, string errorMessage)
      {
         _listOfErrorMessages.Add(new KeyValuePair<string, string>(propertyName, errorMessage));
      }

      /// <summary>
      /// 
      /// </summary>
      public bool HasErrors
      {
         get { return _listOfErrorMessages.Count > 0; }
      }

      public override string ToString()
      {
         StringBuilder b = new StringBuilder();

         foreach(var kvp in _listOfErrorMessages)
         {
            if (b.Length > 0) b.AppendLine();

            b.Append("property: ");
            b.Append(kvp.Key);
            b.Append(", error: ");
            b.Append(kvp.Value);
         }

         return b.ToString();
      }
   }
}
