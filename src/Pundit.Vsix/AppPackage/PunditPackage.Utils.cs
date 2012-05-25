using System.IO;
using Microsoft.VisualStudio.Shell.Interop;

namespace Pundit.Vsix.AppPackage
{
	public partial class PunditPackage : Microsoft.VisualStudio.Shell.Package
	{

      private DirectoryInfo GetPropAsDir(__VSPROPID prop)
      {
         IVsSolution solution = GetService(typeof(SVsSolution)) as IVsSolution;

         if (solution != null)
         {
            object objSolutionDir;

            solution.GetProperty((int)prop, out objSolutionDir);

            if (objSolutionDir != null)
            {
               string solutionDir = objSolutionDir.ToString();

               return new DirectoryInfo(solutionDir);
            }
         }

         return null;
      }

      private DirectoryInfo SolutionDirectory
      {
         get { return GetPropAsDir(__VSPROPID.VSPROPID_SolutionDirectory); }
      }

      private void Test()
      {
         IVsProject proj = GetService(typeof (IVsProject)) as IVsProject;

         //proj.AddItem()
      }
	}
}
