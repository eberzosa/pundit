using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pundit.Core.Model;

namespace Pundit.Test.Mocks
{
   class StatCollectorInstallTarget : IInstallTarget
   {
      private readonly List<string> _createdStreams = new List<string>();

      public List<string> CreatedStreams { get { return _createdStreams; } }
      
      public Stream CreateTargetStream(IEnumerable<string> destinationPath)
      {
         _createdStreams.Add(string.Join("/", destinationPath));

         return new MemoryStream();
      }
   }
}
