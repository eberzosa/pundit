using System;
using System.Data;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Server
{
   class MySqlHelper : SqlHelper
   {
      public MySqlHelper()
      {
         //Configuration.
      }

      #region Overrides of SqlHelper

      protected override IDbConnection Connection
      {
         get { throw new NotImplementedException(); }
      }

      protected override void Add(IDbCommand cmd, object value)
      {
         throw new NotImplementedException();
      }

      public override void Dispose()
      {
         throw new NotImplementedException();
      }

      protected override string GetSelectId()
      {
         throw new NotImplementedException();
      }

      #endregion

      public Package GetManifest(long manifestId)
      {
         throw new NotImplementedException();
      }

      public long WriteManifest(Package manifest)
      {
         throw new NotImplementedException();
      }
   }
}
