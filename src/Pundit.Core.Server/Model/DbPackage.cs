using System;
using Pundit.Core.Model;

namespace Pundit.Core.Server.Model
{
   /// <summary>
   /// Consolidates <see cref="Package"/> data with extra fields cached in the database
   /// </summary>
   public class DbPackage : IEquatable<DbPackage>
   {
      private readonly Package _package;

      public DbPackage(long id, Package package)
      {
         if (package == null) throw new ArgumentNullException("package");
         _package = package;

         Id = id;
      }

      public long Id { get; set; }

      public Package Package { get { return _package; } }

      public DateTime CreatedDate { get; set; }

      public long FileSize { get; set; }

      public bool Equals(DbPackage other)
      {
         if (ReferenceEquals(other, null)) return false;
         return other.Package.Equals(Package);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(obj, null)) return false;
         if (ReferenceEquals(obj, this)) return true;
         if (GetType() != obj.GetType()) return false;
         return Equals((DbPackage) obj);
      }

      public override int GetHashCode()
      {
         return Package.GetHashCode();
      }

      public override string ToString()
      {
         return Package.ToString();
      }
   }
}
