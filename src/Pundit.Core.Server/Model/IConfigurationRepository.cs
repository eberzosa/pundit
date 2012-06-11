using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Server.Model
{
   public interface IConfigurationRepository
   {
      void Set(string key, string value);

      string Get(string key);
   }
}
