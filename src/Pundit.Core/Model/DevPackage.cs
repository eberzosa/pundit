using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   /// <summary>
   /// Package definition existing only at the time of development
   /// </summary>
   [XmlRoot("package")]
   public class DevPackage : Package
   {
      private List<SourceFiles> _files = new List<SourceFiles>();

      [XmlArray("files")]
      [XmlArrayItem("file")]
      public List<SourceFiles> Files
      {
         get { return _files; }
         set { _files = new List<SourceFiles>(value);}
      }

      public DevPackage()
      {
         
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="inputStream"></param>
      /// <returns></returns>
      public new static DevPackage FromStreamXml(Stream inputStream)
      {
         using (var ms = new MemoryStream())
         {
            inputStream.CopyTo(ms);
            ms.Position = 0;

            try
            {
               return ms.FromXmlStream<DevPackage>();
            }
            catch (InvalidOperationException)
            {
               ms.Position = 0;
               return ms.FromXmlStream<DevPackage>(false);
            }
         }
      }

      public override void Validate()
      {
         InvalidPackageException ex;

         try
         {
            base.Validate();

            ex = new InvalidPackageException();
         }
         catch(InvalidPackageException ex1)
         {
            ex = ex1;
         }

         //if(Files.Count == 0)
         //   ex.AddError("Files", "package requires files to be included");

         if (ex.HasErrors)
            throw ex;
      }

      public override object Clone()
      {
         throw new NotImplementedException();
      }
   }
}
