using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pundit.Core.Server.Application
{
   /// <summary>
   /// Incremental sql database upgrade system (work in progress)
   /// </summary>
   class LiquidSql
   {
      private readonly IDbConnection _connection;

      public LiquidSql(IDbConnection connection)
      {
         _connection = connection;
      }
   }
}
