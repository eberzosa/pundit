using System;
using System.IO;

namespace Pundit.Core.Application.Streaming
{
   class WrappedStream : Stream
   {
      private readonly Stream _master;

      public WrappedStream(Stream master)
      {
         if (master == null) throw new ArgumentNullException("master");
         _master = master;
      }

      /// <summary>
      /// When set will always return this value in <see cref="Length"/> instead of master's
      /// </summary>
      public long? OverridenLength { get; set; }

      public override void Flush()
      {
         _master.Flush();
      }

      public override long Seek(long offset, SeekOrigin origin)
      {
         return _master.Seek(offset, origin);
      }

      public override void SetLength(long value)
      {
         _master.SetLength(value);
      }

      public override int Read(byte[] buffer, int offset, int count)
      {
         return _master.Read(buffer, offset, count);
      }

      public override void Write(byte[] buffer, int offset, int count)
      {
         _master.Write(buffer, offset, count);
      }

      public override bool CanRead
      {
         get { return _master.CanRead; }
      }

      public override bool CanSeek
      {
         get { return _master.CanSeek; }
      }

      public override bool CanWrite
      {
         get { return _master.CanWrite; }
      }

      public override long Length
      {
         get { return OverridenLength != null ? OverridenLength.Value : _master.Length; }
      }

      public override long Position
      {
         get { return _master.Position; }
         set { _master.Position = value; }
      }
   }
}
