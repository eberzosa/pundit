using System;
using System.Data;
using System.Text;

namespace Pundit.Core.Utils
{
   public abstract class SqlHelper : IDisposable
   {
      protected abstract IDbConnection Connection { get; }

      protected abstract void Add(IDbCommand cmd, object value, string name = null);

      public abstract void Dispose();

      protected abstract string GetSelectId();

      protected abstract string GetParameterName(int idx);

      public long Insert(string tableName, string[] columns, params object[] values)
      {
         if (tableName == null) throw new ArgumentNullException("tableName");
         if (columns == null || columns.Length == 0)
            throw new ArgumentException("columns are required");
         if (values == null || values.Length != columns.Length)
            throw new ArgumentException("values must match columns");

         var b = new StringBuilder();
         b.Append("insert into ");
         b.Append(tableName);
         b.Append(" (");

         for (int i = 0; i < columns.Length; i++)
         {
            if (i != 0) b.Append(", ");
            b.Append(columns[i]);
         }

         b.Append(") values (");
         for (int i = 0; i < columns.Length; i++)
         {
            if (i != 0) b.Append(", ");
            b.Append(GetParameterName(i));
         }
         b.Append(")");
         b.Append(GetSelectId());

         using (IDbCommand cmd = Connection.CreateCommand())
         {
            cmd.CommandText = b.ToString();
            for (int i = 0; i < values.Length; i++)
            {
               string name = "P" + i;
               Add(cmd, values[i], name);
            }

            return (long)cmd.ExecuteScalar();
         }
      }

      public void Update(string tableName, string[] columns, object[] values,
         string[] where, params object[] whereValues)
      {
         if (tableName == null) throw new ArgumentNullException("tableName");
         if (columns == null) throw new ArgumentNullException("columns");
         if (values == null) throw new ArgumentNullException("values");
         if (columns.Length != values.Length) throw new ArgumentException("columns must match values");

         var b = new StringBuilder();
         b.Append("update ");
         b.Append(tableName);
         b.Append(" set ");
         for (int i = 0; i < columns.Length; i++)
         {
            if (i > 0) b.Append(", ");
            b.Append(columns[i]);
            b.Append("=");
            b.Append(GetParameterName(i));
         }
         if (where != null && where.Length > 0)
         {
            b.Append(" where ");
            for (int i = 0; i < where.Length; i++)
            {
               if (i > 0) b.Append(" and ");
               b.Append(where[i]);
            }
         }

         using (IDbCommand cmd = Connection.CreateCommand())
         {
            cmd.CommandText = b.ToString();
            int pi = 0;
            foreach (object t in values)
            {
               string name = "P" + (pi++);
               Add(cmd, t, name);
            }
            if (whereValues != null)
            {
               foreach (object t in whereValues)
               {
                  string name = "P" + (pi++);
                  Add(cmd, t, name);
               }
            }
            cmd.ExecuteNonQuery();
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
         return ExecuteReaderPage(tableName, -1, -1, columns, where, parameters);
      }

      public IDataReader ExecuteReaderPage(string tableName, long offset, long count, string[] columns, string[] where, params object[] parameters)
      {
         return ExecuteSortedReaderPage(tableName, offset, count, null, true, columns, where, parameters);
      }

      public IDataReader ExecuteSortedReaderPage(string tableName,
         long offset, long count, string sortColumn, bool sortAscending,
         string[] columns,
         string[] where, params object[] parameters)
      {
         if (tableName == null) throw new ArgumentNullException("tableName");

         var s = new StringBuilder();
         s.Append("select ");

         if (columns == null)
         {
            s.Append("*");
         }
         else
         {
            for (int i = 0; i < columns.Length; i++)
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

         if(sortColumn != null)
         {
            s.Append(" ORDER BY ");
            s.Append(sortColumn);
            s.Append(sortAscending ? " ASC" : " DESC");
         }

         if(offset > -1 && count > -1)
         {
            s.Append(" LIMIT ");
            s.Append(offset);
            s.Append(", ");
            s.Append(count);
         }

         using (IDbCommand cmd = CreateCommand())
         {
            cmd.CommandText = s.ToString();
            if (parameters != null)
            {
               for (int i = 0; i < parameters.Length; i++)
               {
                  Add(cmd, parameters[i], "P" + i);
               }
            }

            return cmd.ExecuteReader();
         }

      }

      private T CastScalar<T>(object value)
      {
         if (value == null || value is DBNull) return default(T);
         if (!(value is T)) throw new InvalidCastException("cannot cast " + value.GetType() + " to " + typeof (T));
         return (T) value;
      }
      
      public T ExecuteScalar<T>(string script)
      {
         using(IDbCommand cmd = CreateCommand())
         {
            cmd.CommandText = script;
            object r = cmd.ExecuteScalar();
            return CastScalar<T>(r);
         }
      }

      public void Execute(string script)
      {
         using(IDbCommand cmd = CreateCommand())
         {
            cmd.CommandText = script;
            cmd.ExecuteNonQuery();
         }
      }

      public T ExecuteScalar<T>(string tableName, string column, string[] where, params object[] parameters)
      {
         var b = new StringBuilder();
         b.Append("select ");
         b.Append(column);
         b.Append(" from ");
         b.Append(tableName);

         if (where != null && where.Length > 0)
         {
            b.Append(" where ");
            for (int i = 0; i < where.Length; i++)
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
                  Add(cmd, parameters[i], "P" + i);
               }
            }

            object r = cmd.ExecuteScalar();
            return CastScalar<T>(r);
         }
      }

      public void Delete(string tableName, string[] where, params object[] parameters)
      {
         var b = new StringBuilder();
         b.Append("delete from ");
         b.Append(tableName);

         if (where != null && where.Length > 0)
         {
            b.Append(" where ");
            for (int i = 0; i < where.Length; i++)
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
                  Add(cmd, parameters[i], "P" + i);
               }
            }

            cmd.ExecuteNonQuery();
         }
      }

      public void DeleteRecord(string tableName, long rowId)
      {
         using (IDbCommand cmd = CreateCommand())
         {
            cmd.CommandText = "delete from " + tableName + " where " + tableName + "Id=" + rowId;
            cmd.ExecuteNonQuery();
         }
      }
      
      public abstract bool TableExists(string tableName);
   }
}
