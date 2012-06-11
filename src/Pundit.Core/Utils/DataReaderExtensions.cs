namespace System.Data
{
   public static class DataReaderExtensions
   {
      public static string AsString(this IDataReader reader, string columnName)
      {
         object value = reader[columnName];
         if (value == null || value is DBNull) return null;
         return (string)value;
      }

      public static bool AsBool(this IDataReader reader, string columnName)
      {
         object value = reader[columnName];
         if (value == null || value is DBNull) return false;
         if (value is long) return (long)value != 0;
         if (value is bool) return (bool)value;
         throw new ArgumentException("can't cast from " + value.GetType() + " to bool");
      }

      public static long AsLong(this IDataReader reader, string columnName)
      {
         object value = reader[columnName];
         if (value == null || value is DBNull) return 0;
         if (value is long) return (long) value;
         if (value is uint) return (uint) value;
         if (value is byte) return (byte) value;
         return 0;
      }

      public static DateTime? AsNullableDateTime(this IDataReader reader, string columnName)
      {
         object value = reader[columnName];
         if (value == null || value is DBNull) return null;
         if (value is DateTime) return (DateTime)value;
         throw new ArgumentException("unexpected type " + value.GetType() + " (expected " + typeof(DateTime) + ")");
      }

      public static DateTime AsDateTime(this IDataReader reader, string columnName)
      {
         DateTime? v = AsNullableDateTime(reader, columnName);
         if (v == null) return DateTime.MinValue;
         return v.Value;
      }
   }
}
