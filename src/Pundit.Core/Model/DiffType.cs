namespace Pundit.Core.Model
{
   /// <summary>
   /// Type of dependency difference
   /// </summary>
   public enum DiffType
   {
      /// <summary>
      /// Nothing changed
      /// </summary>
      NoChange,

      /// <summary>
      /// This is a new dependency
      /// </summary>
      Add,

      /// <summary>
      /// Change in dependency i.e. version number, platform, or anything else
      /// </summary>
      Mod,

      /// <summary>
      /// This dependency is removed
      /// </summary>
      Del
   }
}