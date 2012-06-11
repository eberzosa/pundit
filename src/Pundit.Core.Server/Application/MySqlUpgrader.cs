using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pundit.Core.Server.Application
{
   class MySqlUpgrader
   {
      public MySqlUpgrader()
      {
         
      }

      public void Execute(IDbConnection connection)
      {
         if (connection == null) throw new ArgumentNullException("connection");

      }
   }
}
