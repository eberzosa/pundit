using System;
using NuGet.Frameworks;

namespace EBerzosa.Pundit.Core.Framework
{
   public class PunditFramework : IEquatable<PunditFramework>
   {
      public const string NoArchName = "noarch";

      public static PunditFramework AgnosticFramework = new PunditFramework(NuGetFramework.AgnosticFramework);

      public static PunditFramework UnsupportedFramework = new PunditFramework(NuGetFramework.UnsupportedFramework);

      public static PunditFramework AnyFramework = new PunditFramework(NuGetFramework.AnyFramework);


      private readonly string _legacyFramework;


      public NuGetFramework NuGetFramework { get; }

      public bool IsLegacy => _legacyFramework != null;


      public PunditFramework(NuGetFramework framework) => NuGetFramework = framework;

      private PunditFramework(string legacyFramework) => _legacyFramework = legacyFramework;


      public string GetShortFolderName() => _legacyFramework ?? NuGetFramework.GetShortFolderName();

      public static PunditFramework Parse(string framework)
      {
         var nuGetFramework = NuGetFramework.Parse(framework);

         return nuGetFramework.IsUnsupported ? new PunditFramework(framework) : new PunditFramework(nuGetFramework);
      }
      

      public override string ToString() => _legacyFramework ?? NuGetFramework.ToString();

      public bool Equals(PunditFramework other) => Equals(this, other);

      public static bool operator ==(PunditFramework left, PunditFramework right) => Equals(left, right);

      public static bool operator !=(PunditFramework left, PunditFramework right) => !Equals(left, right);

      public override int GetHashCode() => IsLegacy ? _legacyFramework.GetHashCode() : NuGetFramework.GetHashCode();

      public override bool Equals(object obj) => Equals(this, obj as PunditFramework);

      private static bool Equals(PunditFramework left, PunditFramework right)
      {
         var leftObj = (object)left;
         var rightObj = (object)right;

         if (leftObj == null && rightObj == null)
            return true;

         if (leftObj == null || rightObj == null)
            return false;
         
         if (left.IsLegacy && right.IsLegacy)
            return left._legacyFramework.Equals(right._legacyFramework);

         if (!left.IsLegacy && !right.IsLegacy)
            return left.NuGetFramework.Equals(right.NuGetFramework);

         return false;
      }
   }
}
