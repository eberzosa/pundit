using System;
using System.Diagnostics;

namespace EBerzosa.Pundit.Core.Utils
{
   internal class TimeMeasurer : IDisposable
   {
      private readonly Func<string, IWriter> _writer;
      private readonly Stopwatch _stopwatch;

      public TimeMeasurer(Func<string, IWriter> writer)
      {
         _writer = writer;
         _stopwatch = Stopwatch.StartNew();
      }

      public void Dispose()
      {
         _stopwatch.Stop();
         _writer(" " + _stopwatch.ElapsedMilliseconds + "ms");
      }
   }
}
