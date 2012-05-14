namespace Pundit.Vsix.Weirdo
{
	/// <summary>
	/// This class is used to expose the list of the IDs of the commands implemented
	/// by this package. This list of IDs must match the set of IDs defined inside the
	/// Buttons section of the VSCT file.
	/// </summary>
	internal static class CommandSet
	{
		// Now define the list a set of public static members.
		//public const int cmdidMyCommand = 0x2001;
		//public const int cmdidMyGraph = 0x2002;
		//public const int cmdidMyZoom = 0x2003;
		//public const int cmdidDynamicTxt = 0x2004;
		//public const int cmdidDynVisibility1 = 0x2005;
		//public const int cmdidDynVisibility2 = 0x2006;
	   public const int AddPackages = 0x2007;
	   public const int cmdidGlobalSettings = 0x2008;
	   public const int Resolve = 0x2009;
	   public const int cmdidPunditConsole = 0x200A;
      public const int Help = 0x200B;
	   public const int SearchCombo = 0x200C;
	   public const int cmdidShowManifest = 0x200D;
	}
}
