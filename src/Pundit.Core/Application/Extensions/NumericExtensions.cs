using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
   public static class NumericExtensions
   {
      public static decimal FitRange(this decimal d, decimal min, decimal max)
      {
         if (d < min) d = min;
         else if (d > max) d = max;

         return d;
      }
   }
}
