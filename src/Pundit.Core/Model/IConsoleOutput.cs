using System;

namespace Pundit.Core.Model
{
   /// <summary>
   /// 
   /// </summary>
   public interface IConsoleOutput
   {
      /// <summary>
      /// Returns true if console supports screen updates
      /// </summary>
      bool CanUpdate { get; }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="format"></param>
      /// <param name="args"></param>
      void Write(string format, params object[] args);

      /// <summary>
      /// Writes new line
      /// </summary>
      void WriteLine();

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

      /// <summary>
      /// Draws a progress bar starting from 0%
      /// </summary>
      void StartProgress(int maxValue);

      /// <summary>
      /// Updates current progress bar value
      /// </summary>
      /// <param name="value">0-maxValue set in StartProgress</param>
      void UpdateProgress(int value, string hint = null);

      /// <summary>
      /// Complete progress bar and dispose any objects involved in drawing the progress
      /// </summary>
      void FinishProgress();

      void FinishCommand();

      void FixPrompt();

      /// <summary>
      /// Move cursor to the beginning of the line
      /// </summary>
      void ReturnCarriage();

      /// <summary>
      /// Writes spaces to the end of the line
      /// </summary>
      void ClearToEnd();
   }
}
