using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Pundit.Core.Utils;

namespace Pundit.Core.Server.Application
{
   /// <summary>
   /// Whether data access layer should be migrated in future to Hibernate in order to support different sql servers
   /// methods are repository specific and may remain intact
   /// </summary>
   abstract class MySqlRepositoryBase : SqlHelper
   {
      private readonly string _connectionString;
      private static readonly HashSet<string> UpgradedConnections = new HashSet<string>();
      private static readonly object UpgradeLock = new object();

      protected MySqlRepositoryBase(string connectionString)
      {
         if (connectionString == null)
         {
            _connectionString = ConfigurationManager.ConnectionStrings["MySqlDb"].ToString();
         }
         else
         {
            _connectionString = connectionString;            
         }

         if(_connectionString == null) throw new ApplicationException("connection string is null and application settings has no connection configured");

         lock (UpgradeLock)
         {
            if (!UpgradedConnections.Contains(_connectionString))
            {
               new LiquidSql(this, typeof(MySqlRepositoryBase).Namespace + ".Scripts.MySql").Execute();
               UpgradedConnections.Add(_connectionString);
            }
         }
      }

      #region Overrides of SqlHelper
      
      private MySqlConnection _conn;
      protected override IDbConnection Connection
      {
         get
         {
            if (_conn == null)
            {
               _conn = new MySqlConnection(_connectionString);
               _conn.Open();
            }
            else
            {
               if (_conn.State != ConnectionState.Open) _conn.Open();
            }

            return _conn;
         }
      }

      protected override void Add(IDbCommand cmd, object value, string name = null)
      {
         if(value == null)
         {
            cmd.Parameters.Add(new MySqlParameter(name, DBNull.Value));
         }
         else if (value is string || value is long || value is bool || value is DateTime)
         {
            var p = new MySqlParameter(name, value);
            cmd.Parameters.Add(p);
         }
         else if (value is MySqlParameter)
         {
            cmd.Parameters.Add(value);
         }
         else throw new ArgumentException("type " + value.GetType() + " not supported");
      }

      public override void Dispose()
      {
         if(_conn != null)
         {
            _conn.Close();
            _conn.Dispose();
            _conn = null;
         }
      }

      protected override string GetSelectId()
      {
         return ";select last_insert_id();";
      }

      protected override string GetParameterName(int idx)
      {
         return "?P" + idx;
      }

      #endregion
   }
}
