namespace Pundit.Vsix.Model
{
   /// <summary>
   /// plain xml-serializable class for storing pundit options
   /// </summary>
   public class ExtensionSettings
   {
      public bool AutoResolveEnabled { get; set; }

      public long AutoResolveFrequencySec { get; set; }
   }
}
