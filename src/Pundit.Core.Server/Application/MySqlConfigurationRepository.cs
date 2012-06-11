using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server.Application
{
   internal class MySqlConfigurationRepository : MySqlRepositoryBase, IConfigurationRepository
   {
      private const string OptionTableName = "Option";

      #region Implementation of IConfigurationRepository

      public MySqlConfigurationRepository(string connectionString) : base(connectionString)
      {
      }

      public void Set(string key, string value)
      {
         if (key == null) throw new ArgumentNullException("key");
         long id = ExecuteScalar<long>(OptionTableName, "OptionId", new[] {"Name=?P0"}, key);

         if(id == 0)
         {
            Insert(OptionTableName, new[] {"Name", "Value"}, key, value);
         }
         else
         {
            Update(OptionTableName, new[] {"Value"}, new object[] {value}, new[] {"Name=?P0", value});
         }
      }

      public string Get(string key)
      {
         if (key == null) throw new ArgumentNullException("key");
         return ExecuteScalar<string>(OptionTableName, "Value", new[] {"Name=?P0"}, key);
      }

      #endregion
   }
}