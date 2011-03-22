﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NGem.Core.Model
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

      public static DevPackage FromStream(Stream inputStream)
      {
         XmlSerializer xmls = new XmlSerializer(typeof(DevPackage));

         DevPackage dp = (DevPackage)xmls.Deserialize(inputStream);

         dp.Validate();

         return dp;
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

         if(Files.Count == 0)
            ex.AddError("Files", "package requires files to be included");

         if (ex.HasErrors)
            throw ex;
      }

      public string GetFileName()
      {
         Validate();

         return string.Format("{0}-{1}.{2}.{3}-{4}-{5}.pundit",
            PackageId,
            Version.Major, Version.Minor, Version.Build,
            Version.Revision,
            string.IsNullOrEmpty(Platform) ? "noarch" : Platform);
      }
   }
}