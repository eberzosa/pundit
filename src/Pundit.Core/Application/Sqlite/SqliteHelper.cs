using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Text;

namespace Pundit.Core.Application.Sqlite
{
   class SqliteHelper : IDisposable
   {
      private const string SelectId = ";select last_insert_rowid()";

      private string _absolutePath;
      private string _absoluteDir;
      private SQLiteConnection _conn;

      public SqliteHelper(string dbPath)
      {
         if (dbPath == null) throw new ArgumentNullException("dbPath");
         _absolutePath = dbPath;
         _absoluteDir = Path.GetDirectoryName(_absolutePath);

         if (!Directory.Exists(_absoluteDir))
            throw new DirectoryNotFoundException("target folder not found (" + _absoluteDir + ")");
      }

      private string GetConnectionString()
      {
         if (!File.Exists(_absolutePath))
         {
            using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(SqliteHelper).Namespace + ".pundit.db"))
            {
               using (Stream tgt = File.Create(_absolutePath))
               {
                  s.CopyTo(tgt);
               }
            }
         }

         return "Data Source=" + _absolutePath;
      }

      private SQLiteConnection Connection
      {
         get
         {
            if (_conn == null)
            {
               _conn = new SQLiteConnection(GetConnectionString());
               _conn.Open();
            }

            return _conn;
         }
      }

      private void Add(IDbCommand cmd, object value)
      {
         if (value is string)
            cmd.Parameters.Add(new SQLiteParameter(DbType.String, value));
         else if (value is long)
            cmd.Parameters.Add(new SQLiteParameter(DbType.Int32, value));
         else if (value is bool)
            cmd.Parameters.Add(new SQLiteParameter(DbType.Boolean, value));
         else if (value == null)
            cmd.Parameters.Add(new SQLiteParameter());
         else if (value is SQLiteParameter)
            cmd.Parameters.Add((SQLiteParameter) value);
         else throw new ArgumentException("type " + value.GetType() + " not supported");
      }

      public long Insert(string tableName, string[] columns, object[] values)
      {
         if (tableName == null) throw new ArgumentNullException("tableName");
         if (columns == null || columns.Length == 0)
            throw new ArgumentException("columns are required");
         if (values == null || values.Length != columns.Length)
            throw new ArgumentException("values should match columns");

         StringBuilder b = new StringBuilder();
         b.Append("insert into [");
         b.Append(tableName);
         b.Append("] (");

         for(int i = 0; i < columns.Length; i++)
         {
            if (i != 0) b.Append(", ");
            b.Append(columns[i]);
         }

         b.Append(") values (");
         for(int i = 0; i < columns.Length; i++)
         {
            if (i != 0) b.Append(", ");
            b.Append("(?)");
         }
         b.Append(")");
         b.Append(SelectId);

         using (SQLiteCommand cmd = Connection.CreateCommand())
         {
            cmd.CommandText = b.ToString();
            foreach (object value in values)
            {
               Add(cmd, value);
            }

            return (long) cmd.ExecuteScalar();
         }
      }

      public IDbCommand CreateCommand()
      {
         return Connection.CreateCommand();
      }

      public IDbTransaction BeginTransaction()
      {
         return Connection.BeginTransaction();
      }

      public IDataReader ExecuteReader(string tableName, string[] columns, string[] where, params object[] parameters)
      {
         if (tableName == null) throw new ArgumentNullException("tableName");

         StringBuilder s = new StringBuilder();
         s.Append("select ");

         if (columns == null)
         {
            s.Append("*");
         }
         else
         {
            for(int i = 0; i < columns.Length; i++)
            {
               if (i != 0) s.Append(", ");
               s.Append(columns[i]);
            }
         }

         s.Append(" from ");
         s.Append(tableName);

         if (where != null)
         {
            s.Append(" where ");
            for (int i = 0; i < where.Length; i++)
            {
               if (i != 0) s.Append(" AND ");
               s.Append(where[i]);
            }
         }

         using (IDbCommand cmd = CreateCommand())
         {
            cmd.CommandText = s.ToString();
            if (parameters != null)
            {
               for (int i = 0; i < parameters.Length; i++)
               {
                  Add(cmd, parameters[i]);
               }
            }

            return cmd.ExecuteReader();
         }

      }

      public T ExecuteScalar<T>(string tableName, string column, string[] where, params object[] parameters)
      {
         StringBuilder b = new StringBuilder();
         b.Append("select ");
         b.Append(column);
         b.Append(" from ");
         b.Append(tableName);

         if(where != null && where.Length > 0)
         {
            b.Append(" where ");
            for(int i = 0; i < where.Length; i++)
            {
               if (i != 0) b.Append(" AND ");
               b.Append(where[i]);
            }
         }

         using (IDbCommand cmd = CreateCommand())
         {
            cmd.CommandText = b.ToString();
            if (parameters != null && parameters.Length > 0)
            {
               for (int i = 0; i < parameters.Length; i++)
               {
                  Add(cmd, parameters[i]);
               }
            }

            object r = cmd.ExecuteScalar();

            return r == null ? default(T) : (T) r;
         }
      }

      public void Dispose()
      {
         _conn.Close();
      }
   }
}
