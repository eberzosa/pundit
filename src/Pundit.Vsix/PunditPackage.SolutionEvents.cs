﻿using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace Pundit.Vsix
{
   partial class PunditPackage
   {
      public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
      {
         return VSConstants.S_OK;
      }

      public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
      {
         return VSConstants.S_OK;
      }

      public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
      {
         return VSConstants.S_OK;
      }

      public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
      {
         return VSConstants.S_OK;
      }

      public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
      {
         return VSConstants.S_OK;
      }

      public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
      {
         return VSConstants.S_OK;
      }

      public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
      {
         OnSolutionOpened();

         return VSConstants.S_OK;
      }

      public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
      {
         return VSConstants.S_OK;
      }

      public int OnBeforeCloseSolution(object pUnkReserved)
      {
         return VSConstants.S_OK;
      }

      public int OnAfterCloseSolution(object pUnkReserved)
      {
         OnSolutionClosed();

         return VSConstants.S_OK;
      }

   }
}