using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core
{
   /// <summary>
   /// Global constants
   /// </summary>
   public static class Globals
   {
      /// <summary>
      /// Public repository base URI
      /// </summary>
      public static readonly Uri PublicBaseUri = new Uri("http://pundit-dm.com");

      /// <summary>
      /// URI to download the latest version
      /// </summary>
      public static readonly Uri GetLatestVersionNumberUri = new Uri(PublicBaseUri, "version/latest");
   }
}
