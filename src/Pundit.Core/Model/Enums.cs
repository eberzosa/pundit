using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pundit.Core.Model
{
   public enum BuildConfiguration
   {
      [XmlEnum("release")]
      Release = 0,

      [XmlEnum("debug")]
      Debug
   }
}
