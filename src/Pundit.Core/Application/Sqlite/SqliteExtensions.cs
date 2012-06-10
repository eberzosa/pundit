namespace System.Data.SQLite
{
   static class SqliteExtensions
   {
      public static IDbCommand Add(this IDbCommand cmd, string value)
      {
         SQLiteParameter p = new SQLiteParameter(DbType.String, (object)value);
         cmd.Parameters.Add(p);
         return cmd;
      }

      public static IDbCommand Add(this IDbCommand cmd, long value)
      {
         SQLiteParameter p = new SQLiteParameter(DbType.Int32, (object)value);
         cmd.Parameters.Add(p);
         return cmd;
      }

      public static IDbCommand Add(this IDbCommand cmd, bool value)
      {
         SQLiteParameter p = new SQLiteParameter(DbType.Boolean, (object)value);
         cmd.Parameters.Add(p);
         return cmd;
      }

      public static IDbCommand Add(this IDbCommand cmd, DateTime value)
      {
         SQLiteParameter p = new SQLiteParameter(DbType.DateTime, (object)value);
         cmd.Parameters.Add(p);
         return cmd;
      }
   }
}
