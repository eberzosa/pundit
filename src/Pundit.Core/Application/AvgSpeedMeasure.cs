using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pundit.Core.Application
{
   /// <summary>
   /// Measures download speed (first draft)
   /// </summary>
   class AvgSpeedMeasure
   {
      private static readonly double TicksInSecond = TimeSpan.FromSeconds(1).Ticks;
      private static readonly double TicksMin = TimeSpan.FromMilliseconds(1).Ticks;

      private readonly Dictionary<double, long> _ticksToBytes = new Dictionary<double, long>();
      private DateTime _lastSlice = DateTime.UtcNow;

      public AvgSpeedMeasure()
      {
         
      }

      public void Slice(long bytes)
      {
         DateTime now = DateTime.UtcNow;
         _ticksToBytes[(now - _lastSlice).Ticks] = bytes;
         _lastSlice = now;
      }

      public long BytesPerSecond
      {
         get
         {
            double bt = 0;
            int slices = 0;
            foreach(KeyValuePair<double, long> slice in _ticksToBytes)
            {
               if (slice.Key > TicksMin)
               {
                  double ds = TicksInSecond*(double) slice.Value/slice.Key;
                  bt += ds;
                  slices++;
               }
            }
            return (long)(bt/slices);
         }
      }
   }
}
