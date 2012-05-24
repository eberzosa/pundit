using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Pundit.Vsix.Application;
using Pundit.Vsix.Forms;
using Pundit.Vsix.Forms.Configuration.General;
using Pundit.Vsix.Forms.Console;
using Pundit.Vsix.Weirdo;

namespace Pundit.Vsix
{
	/// <summary>
	/// This is the class that implements the package. This is the class that Visual Studio will create
	/// when one of the commands will be selected by the user, and so it can be considered the main
	/// entry point for the integration with the IDE.
	/// Notice that this implementation derives from Microsoft.VisualStudio.Shell.Package that is the
	/// basic implementation of a package provided by the Managed Package Framework (MPF).
	/// </summary>
	[PackageRegistration(UseManagedResourcesOnly = true)]	
	[ProvideMenuResource(1000, 1)]
   [ProvideAutoLoad(UIContextGuids80.SolutionExists)]   //auto-load extension on solution start so we can start background activity if configured
   [Guid("3C7C5ABE-82AC-4A37-B077-0FF60E8B1FD3")]
   [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
   [ProvideToolWindow(typeof(ConsoleVsToolWindow))]
   [ProvideOptionPage(typeof(VsixOptionsPage), "Pundit", "General", 113, 114, true)]
   [ProvideOptionPage(typeof(IntegrationPage), "Pundit", "Integration", 113, 114, true)]
	[ComVisible(true)]
	public partial class PunditPackage : Package, IVsSolutionEvents, IPunditVsCommands
	{
	   private OleMenuCommandService _mcs;
	   private uint _solutionEventsCookie;
	   private StatusBarIconManager _statusBar;

	   private OleMenuCommand _cmdResolve;
	   private OleMenuCommand _cmdAddReference;
	   private OleMenuCommand _cmdEditManifest;
	   private OleMenuCommand _cmdSearch;
	   private OleMenuCommand _cmdHelp;

		/// <summary>
		/// Default constructor of the package. This is the constructor that will be used by VS
		/// to create an instance of your package. Inside the constructor you should do only the
		/// more basic initializazion like setting the initial value for some member variable. But
		/// you should never try to use any VS service because this object is not part of VS
		/// environment yet; you should wait and perform this kind of initialization inside the
		/// Initialize method.
		/// </summary>
		public PunditPackage()
		{
         //"VisualSVN.Update"
		}

      private OleMenuCommand BindHandler(int commandId, EventHandler handler, bool visible = true)
      {
         CommandID id = new CommandID(GuidsList.guidPunditCmdSet, commandId);
         OleMenuCommand command = new OleMenuCommand(handler, id);
         command.Visible = visible;
         _mcs.AddCommand(command);
         return command;
      }

		/// <summary>
		/// Initialization of the package; this is the place where you can put all the initialization
		/// code that relies on services provided by Visual Studio.
		/// </summary>
		[SecurityPermission(SecurityAction.Demand, Flags=SecurityPermissionFlag.UnmanagedCode)]
		protected override void Initialize()
		{
			base.Initialize();
		   _settingsStore = GetWritableSettingsStore(SettingsRoot);

			// Now get the OleCommandService object provided by the MPF; this object is the one
			// responsible for handling the collection of commands implemented by the package.
			OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
		   _mcs = mcs;

			if (null != mcs)
			{
			   _cmdAddReference = BindHandler(CommandSet.AddPackages, AddReferenceCommandCallback);
			   //BindHandler(CommandSet.cmdidGlobalSettings, GlobalSettingsCommandCallback);
			   _cmdResolve = BindHandler(CommandSet.Resolve, ResolveDependenciesCommandCallback, false);
			   BindHandler(CommandSet.cmdidPunditConsole, ShowPunditConsoleCallback);
			   _cmdSearch = BindHandler(CommandSet.SearchCombo, FindPackageCommandCallback);
			   _cmdEditManifest = BindHandler(CommandSet.cmdidShowManifest, OpenXmlManifestCallback);
			   _cmdHelp = BindHandler(CommandSet.Help, ShowHelpCallback);
			}

         InitializeShell();
         ExtensionApplication.Instance.AssignVsCommands(this);
         EnableSolutionButtons(false);

		   _statusBar = new StatusBarIconManager();
         _statusBar.StatusIcon = StatusIcon.Green;
		   _statusBar.StatusText = null;
		   StartBackgroundActivity(); //it will start when solution gets opened
		}

      private void InitializeShell()
      {
         IVsSolution2 solution = ServiceProvider.GlobalProvider.GetService(typeof (SVsSolution)) as IVsSolution2;
         if(solution != null)
         {
            solution.AdviseSolutionEvents(this, out _solutionEventsCookie);
         }

      }

      protected override void Dispose(bool disposing)
      {
         StopBackgroundActivity();

         base.Dispose(disposing);
      }
	}
}
