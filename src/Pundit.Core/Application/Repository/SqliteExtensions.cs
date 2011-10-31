using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.SQLite
{
   static class SqliteExtensions
   {
      public static SQLiteCommand Add(this SQLiteCommand cmd, string value)
      {
         SQLiteParameter p = new SQLiteParameter(DbType.String, (object)value);
         cmd.Parameters.Add(p);
         return cmd;
      }

      public static SQLiteCommand Add(this SQLiteCommand cmd, long value)
      {
         SQLiteParameter p = new SQLiteParameter(DbType.Int32, (object)value);
         cmd.Parameters.Add(p);
         return cmd;
      }

      public static SQLiteCommand Add(this SQLiteCommand cmd, bool value)
      {
         SQLiteParameter p = new SQLiteParameter(DbType.Boolean, (object)value);
         cmd.Parameters.Add(p);
         return cmd;
      }
   }
}
