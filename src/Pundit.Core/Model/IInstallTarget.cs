using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   public interface IInstallTarget
   {
      /// <summary>
      /// Creates a writable stream used to write to the target location
      /// </summary>
      /// <param name="destinationPath">Parts of destination path. If they do not exist on the target they will be created
      /// automatically</param>
      /// <returns>Created stream which must be disposed by a called when not needed.</returns>
      Stream CreateTargetStream(IEnumerable<string> destinationPath);
   }
}
