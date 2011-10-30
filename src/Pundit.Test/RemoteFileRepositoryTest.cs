using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pundit.Core.Model;
using Pundit.Core.Application.Repository;

namespace Pundit.Test
{
   [TestFixture, Ignore]
   public class RemoteFileRepositoryTest
   {
      private IRemoteRepository _remote;

      [SetUp]
      public void SetUp()
      {
         _remote = new RemoteFileRepository(@"C:\Users\aloneguid\Dropbox\shared\pundit-repo\packed");
      }

      [Test]
      public void GetShapshotTest()
      {
         string nextChangeId;
         PackageSnapshotKey[] snapshot = _remote.GetSnapshot(null, out nextChangeId);

         Assert.Greater(0, snapshot.Length);
      }
   }
}
