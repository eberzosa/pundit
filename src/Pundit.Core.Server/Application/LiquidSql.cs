using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Pundit.Core.Utils;

namespace Pundit.Core.Server.Application
{
   /// <summary>
   /// Incremental sql database upgrade system (work in progress)
   /// This may go into a separate library in future
   /// </summary>
   class LiquidSql
   {
      private const string CreateScriptName = "create.sql";
      private const string OptionTableName = "`option`";
      private const string KeyColumnName = "`Name`";
      private const string ValueColumnName = "`Value`";
      private const string ParameterName = "version";

      private readonly SqlHelper _sql;
      private readonly string _resourceFolder;
      private string _createScriptPath;
      private readonly SortedSet<string> _upgradeScripts = new SortedSet<string>();

      public LiquidSql(SqlHelper sql, string resourceFolder)
      {
         if (resourceFolder == null) throw new ArgumentNullException("resourceFolder");

         _sql = sql;
         _resourceFolder = resourceFolder;

         DiscoverScripts(resourceFolder);
      }

      private void DiscoverScripts(string resourceFolder)
      {
         foreach(string res in Assembly.GetExecutingAssembly().GetManifestResourceNames())
         {
            if(res != null && res.StartsWith(resourceFolder))
            {
               string name = res.Substring(resourceFolder.Length + 1);
               if(name == CreateScriptName)
               {
                  _createScriptPath = name;
               }
               else
               {
                  _upgradeScripts.Add(name);
               }
            }
         }
      }

      private bool IsDbEmpty
      {
         get
         {
            long count = _sql.ExecuteScalar<long>(
               "select count(*) from information_schema.tables where table_schema = (select database());");
            return count == 0;
         }
      }


      private string GetScriptText(string scriptName)
      {
         using(var rdr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(_resourceFolder + "." + scriptName)))
         {
            return rdr.ReadToEnd();
         }
      }

      private int DbVersion
      {
         get
         {
            int v;
            if(_sql.TableExists(OptionTableName))
            {
               string sver = _sql.ExecuteScalar<string>(OptionTableName, ValueColumnName, new[] {KeyColumnName + "=?P0"}, ParameterName);
               int.TryParse(sver, out v);
            }
            else
            {
               v = 0;
            }
            return v;
         }
         set
         {
            if(DbVersion == 0)
            {
               _sql.Insert(OptionTableName,
                           new[] {KeyColumnName, ValueColumnName}, ParameterName, value.ToString());
            }
            else
            {
               _sql.Update(OptionTableName, new[] {ValueColumnName}, new object[] {value.ToString()},
                           new[] {KeyColumnName + "=?P0"}, ParameterName);
            }
         }
      }

      public void Execute()
      {
         if(DbVersion == 0)
         {
            if(_createScriptPath == null) throw new ApplicationException("database is empty but create script was not found");

            string script = GetScriptText(_createScriptPath);
            _sql.Execute(script);
            DbVersion = 1;
         }
         else
         {
            //todo: has to be fixed only by the next DB upgrade
         }
      }
   }
}
