using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.Services;
using PlasticMetal.MobileSuit.UI;

namespace PlasticMetal.MobileSuit.Core
{
    /// <summary>
    /// A input helper.
    /// </summary>
    public interface IInputHelper
    {
        /// <summary>
        /// Expression for the input
        /// </summary>
        string? Expression { get; }
        /// <summary>
        /// Default value for the input.
        /// </summary>
        string? DefaultInput { get; }
    }
    /// <summary>
    ///     A entity, which serves the input/output of a mobile suit.
    /// </summary>
    public interface IIOHub
    {
        /*/// <summary>
        /// A input helper for current IOHub. Provides information to the prompt.
        /// </summary>
        public IInputHelper InputHelper { get; }*/
        /// <summary>
        ///     Disable Time marks which shows in Output-Redirected Environment.
        /// </summary>
        public bool DisableTags { get; set; }

        /*/// <summary>
        ///     Default IOServer, using stdin, stdout, stderr.
        /// </summary>
        public static IOHub GeneralIO => Suit.GeneralIO;*/

        /// <summary>
        ///     Check if this IOServer's error stream is redirected (NOT stderr)
        /// </summary>
        bool IsErrorRedirected { get; }

        /// <summary>
        ///     Check if this IOServer's output stream is redirected (NOT stdout)
        /// </summary>
        bool IsOutputRedirected { get; }

        /// <summary>
        ///     Error stream (default stderr)
        /// </summary>
        TextWriter ErrorStream { get; set; }

        /// <summary>
        ///     Output stream (default stdout)
        /// </summary>
        TextWriter Output { get; set; }

        /// <summary>
        ///     The prefix of WriteLine() output, usually used to make indentation.
        /// </summary>
        string Prefix { get; set; }

        /// <summary>
        ///     Color settings for this IOServer. (default DefaultColorSetting)
        /// </summary>
        IColorSetting ColorSetting { get; set; }

        /// <summary>
        ///     Prompt server for the io server.
        /// </summary>
        IAssignOncePromptGenerator Prompt { get;}

        /// <summary>
        ///     Input stream (default stdin)
        /// </summary>
        TextReader Input { get; set; }

        /// <summary>
        ///     Checks if this IOServer's input stream is redirected (NOT stdin)
        /// </summary>
        bool IsInputRedirected { get; }



        /// <summary>
        ///     get label of given output type
        /// </summary>
        /// <param name="type">output type</param>
        /// <returns>label</returns>
        protected static string GetLabel(OutputType type = OutputType.Default)
        {
            return type switch
            {
                OutputType.Default => "",
                OutputType.Prompt => "[Prompt]",
                OutputType.Error => "[Error]",
                OutputType.AllOk => "[AllOk]",
                OutputType.ListTitle => "[List]",
                OutputType.CustomInfo => "[Info]",
                OutputType.MobileSuitInfo => "[Info]",
                _ => ""
            };
        }

        /// <summary>
        ///     provides packaging for ContentArray
        /// </summary>
        /// <param name="contents">ContentArray</param>
        /// <returns>packaged ContentArray</returns>
        public static IEnumerable<(string, ConsoleColor?)> CreateContentArray(params (string, ConsoleColor?)[] contents)
        {
            return contents;
        }


        /// <summary>
        ///     provides packaging for ContentArray
        /// </summary>
        /// <param name="contents">ContentArray</param>
        /// <returns>packaged ContentArray</returns>
        public static IEnumerable<(string, ConsoleColor?, ConsoleColor?)> CreateContentArray(
            params (string, ConsoleColor?, ConsoleColor?)[] contents)
        {
            return contents;
        }

        /// <summary>
        ///     Reset this IOServer's error stream to stderr
        /// </summary>
        void ResetError();

        /// <summary>
        ///     Reset this IOServer's output stream to stdout
        /// </summary>
        void ResetOutput();

        /// <summary>
        ///     Append a str to Prefix, usually used to increase indentation
        /// </summary>
        /// <param name="prefix">the output tuple to append</param>
        void AppendWriteLinePrefix((string,ConsoleColor?,ConsoleColor?) prefix);

        /// <summary>
        ///     Subtract a str from Prefix, usually used to decrease indentation
        /// </summary>
        void SubtractWriteLinePrefix();
        /// <summary>
        /// Clear the prefix before writing line.
        /// </summary>
        void ClearWriteLinePrefix();
        /// <summary>
        ///     Writes some content to output stream, with line break. With certain Input/Output color.
        /// </summary>
        /// <param name="content">
        ///     Output Tuple
        ///     first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        void Write((string, ConsoleColor?, ConsoleColor?) content);
        /// <summary>
        ///     Writes some content to output stream, with line break. With certain Input/Output color.
        /// </summary>
        /// <param name="content">
        ///     Output Tuple
        ///     first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        Task WriteAsync((string, ConsoleColor?, ConsoleColor?) content);
        /// <summary>
        /// Get the prefix before writing line.
        /// </summary>
        /// <returns></returns>
        IEnumerable<(string, ConsoleColor?, ConsoleColor?)> GetWriteLinePrefix();
        /// <summary>
        ///     Reset this IOServer's input stream to stdin
        /// </summary>
        void ResetInput();
        /// <summary>
        ///     Reads a line from input stream, with prompt.
        /// </summary>
        /// <returns>Content from input stream, null if EOF</returns>
        string? ReadLine();
        /// <summary>
        ///     Reads a line from input stream, with prompt.
        /// </summary>
        /// <returns>Content from input stream, null if EOF</returns>
        Task<string?> ReadLineAsync();

        /// <summary>
        ///     Reads the next character from input stream without changing the state of the reader or the character source.
        /// </summary>
        /// <returns>The next available character.</returns>
        int Peek();

        /// <summary>
        ///     Reads the next character from input stream.
        /// </summary>
        /// <returns>The next available character.</returns>
        int Read();

        /// <summary>
        ///     Reads all characters from the current position to the end of the input stream and returns them as one string.
        /// </summary>
        /// <returns>All characters from the current position to the end.</returns>
        string ReadToEnd();

        /// <summary>
        ///     Asynchronously reads all characters from the current position to the end of the input stream and returns them as
        ///     one string.
        /// </summary>
        /// <returns>All characters from the current position to the end.</returns>
        Task<string> ReadToEndAsync();
    }
}