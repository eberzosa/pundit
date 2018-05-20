using System;
using Pundit.Core.Model.EventArguments;

namespace EBerzosa.Pundit.Core.Package
{
   public interface IPackageWriter : IDisposable
   {
      Action<PackageFileEventArgs> OnBeginPackingFile { get; set; }

      Action<PackageFileEventArgs> OnEndPackingFile { get; set; }

      long WriteAll();
   }
}