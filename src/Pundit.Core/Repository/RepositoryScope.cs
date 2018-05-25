using System;

namespace EBerzosa.Pundit.Core.Repository
{
   [Flags]
   public enum RepositoryScope
   {
      Unknown,
      Standard = 1 << 0,
      Cache    = 1 << 1,

      Any      = Standard | Cache
   }
}