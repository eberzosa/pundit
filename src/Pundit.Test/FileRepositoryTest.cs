using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Application.Repository;
using Pundit.Core.Model;

namespace Pundit.Test
{
   [TestFixture]
   public class FileRepositoryTest
   {
      private IRemoteRepository _repo = new RemoteFolderRepository(@"C:\Users\aloneguid\Dropbox\shared\pundit-repo\packed");

      [Test]
      public void PublishPackageTest()
      {
         string nextChangeId;

         var snapshot = _repo.GetSnapshot(null);
      }
   }
}
