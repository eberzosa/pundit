using System;
using Pundit.Core.Server.Model;

namespace Pundit.Core.Server.Application
{
   internal class MySqlConfigurationRepository : MySqlRepositoryBase, IConfigurationRepository
   {
      private const string OptionTableName = "`Option`";

      #region Implementation of IConfigurationRepository

      public MySqlConfigurationRepository() : this(null)
      {
         
      }

      public MySqlConfigurationRepository(string connectionString) : base(connectionString)
      {
      }

      public void Set(string key, string value)
      {
         if (key == null) throw new ArgumentNullException("key");
         uint id = ExecuteScalar<uint>(OptionTableName, "OptionId", new[] {"Name=?P0"}, key);

         if(id == 0)
         {
            Insert(OptionTableName, new[] {"Name", "Value"}, key, value);
         }
         else
         {
            Update(OptionTableName, new[] {"Value"}, new object[] {value}, new[] {"Name=?P1"}, key);
         }
      }

      public string Get(string key)
      {
         if (key == null) throw new ArgumentNullException("key");
         return ExecuteScalar<string>(OptionTableName, "`Value`", new[] {"Name=?P0"}, key);
      }

      public long IncrementCounter(string counterName)
      {
         long value = GetCounterValue(counterName);
         Set(counterName, (value + 1).ToString());
         return value + 1;
      }

      public long GetCounterValue(string counterName)
      {
         string s = ExecuteScalar<string>(OptionTableName, "Value",
                                          new[] {"Name=?P0"}, counterName);
         long l;
         long.TryParse(s, out l);
         return l;
      }

      #endregion
   }
}