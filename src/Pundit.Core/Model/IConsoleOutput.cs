using System;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   public interface IConsoleOutput
   {
      /// <summary>
      /// 
      /// </summary>
      /// <param name="format"></param>
      /// <param name="args"></param>
      void Write(string format, params object[] args);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="format"></param>
      /// <param name="args"></param>
      void WriteLine(string format, params object[] args);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="color"></param>
      /// <param name="format"></param>
      /// <param name="args"></param>
      void Write(ConsoleColor color, string format, params object[] args);

      /// <summary>
      /// 
      /// </summary>
      /// <param name="defaultColor"></param>
      /// <param name="format"></param>
      /// <param name="args"></param>
      void WriteLine(ConsoleColor defaultColor, string format, params object[] args);


      /// <summary>
      /// 
      /// </summary>
      /// <param name="result"></param>
      void Write(bool result);
   }
}
