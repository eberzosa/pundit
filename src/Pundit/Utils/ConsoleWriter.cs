using System;
using EBerzosa.CommandLineProcess.Utils;
using EBerzosa.Pundit.Core;

namespace EBerzosa.Pundit.CommandLine.Utils
{
   public class ConsoleWriter : IWriter
   {
      private struct Colours
      {
         public const ConsoleColor Success = ConsoleColor.Green;
         public const ConsoleColor Warning = ConsoleColor.Yellow;
         public const ConsoleColor Error = ConsoleColor.Red;
         public const ConsoleColor Text = ConsoleColor.Gray;
         public const ConsoleColor Reserved = ConsoleColor.White;
         public const ConsoleColor Info = ConsoleColor.Magenta;

         public const ConsoleColor Highlightted = ConsoleColor.Yellow;
      }

      private readonly IOutput _output;

      private bool _started;
      private int?[] _columnLengths;
      private int _currentColumn;

      public ConsoleWriter(IOutput output)
      {
         _output = output;
      }

      public IWriter Title(string message)
      {
         _output.Print("", true);
         _output.Print(message, ConsoleColor.White);
         _output.Print("".PadLeft(message.Length, '='));
         _output.Print("", true);
         return this;
      }

      public IWriter Text(string message)
      {
         _output.Print(PadMessage(message), !_started, Colours.Text);
         return this;
      }

      public IWriter Reserved(string message)
      {
         _output.Print(PadMessage(message), !_started, Colours.Reserved);
         return this;
      }

      public IWriter Info(string message)
      {
         _output.Print(PadMessage(message), !_started, Colours.Info);
         return this;
      }

      public IWriter Success(string message)
      {
         _output.Print(PadMessage(message), !_started, Colours.Success);
         return this;
      }

      public IWriter Error(string message)
      {
         _output.Print(PadMessage(message), !_started, Colours.Error);
         return this;
      }

      public IWriter Warning(string message)
      {
         _output.Print(PadMessage(message), !_started, Colours.Warning);
         return this;
      }

      public IWriter HightLight(string message)
      {
         _output.Print(PadMessage(message), !_started, null, Colours.Highlightted);
         return this;
      }

      public IWriter Empty()
      {
         _output.Print(string.Empty);
         return this;
      }

      public IWriter BeginWrite()
      {
         _started = true;
         return this;
      }

      public IWriter EndWrite()
      {
         if (!_started)
            return this;

         _started = false;

         _output.Print(string.Empty);

         return this;
      }

      public IWriter BeginColumns(int?[] lengths)
      {
         _columnLengths = lengths;
         _currentColumn = 0;
         
         return this;
      }

      public IWriter EndColumns()
      {
         _columnLengths = null;

         EndWrite();

         return this;
      }

      private string PadMessage(string message)
      {
         if (_columnLengths == null)
            return message;

         var pad = _columnLengths[_currentColumn];
         
         if (_currentColumn == 0)
         {
            EndWrite();
            BeginWrite();
         }

         _currentColumn = (_currentColumn + 1) % _columnLengths.Length;

         if (!pad.HasValue)
            return message;

         return message.PadRight(pad.Value, ' ');
      }
   }
}
