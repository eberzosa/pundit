namespace EBerzosa.Pundit.Core.Versioning
{
   /// <summary>
   /// Specifies the floating behavior type.
   /// </summary>
   public enum FloatBehaviour
   {
      /// <summary>
      /// Lowest version, no float
      /// </summary>
      None,

      /// <summary>
      /// x.y.z-ttt pre-release starting with ttt
      /// </summary>
      Prerelease,

      /// <summary>
      /// x.y.z.*
      /// </summary>
      Revision,

      /// <summary>
      /// x.y.*
      /// </summary>
      Patch,

      /// <summary>
      /// x.*
      /// </summary>
      Minor,

      /// <summary>
      /// *
      /// </summary>
      Major,
      
      /// <summary>
      /// x.y.z-ttt.* matching pre-release
      /// </summary>
      RevisionPrerelease,

      /// <summary>
      /// x.y.*-ttt.*, matching pre-release
      /// </summary>
      PatchPrerelease,

      /// <summary>
      /// x.*-ttt.*, matching pre-release
      /// </summary>
      MinorPrerelease,

      /// <summary>
      /// *-ttt, matching pre-release
      /// </summary>
      MajorPrerelease
   }
}