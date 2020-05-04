using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.IO
{
    /// <summary>
    /// A entity, which serves the input/output of a mobile suit.
    /// </summary>
    public interface IIOServer
    {
        /// <summary>
        /// Default IOServer, using stdin, stdout, stderr.
        /// </summary>
        public static IOServer GeneralIO { get; } = new IOServer();

        /// <summary>
        /// Check if this IOServer's error stream is redirected (NOT stderr)
        /// </summary>
        bool IsErrorRedirected { get; }

        /// <summary>
        /// Check if this IOServer's output stream is redirected (NOT stdout)
        /// </summary>
        bool IsOutputRedirected { get; }

        /// <summary>
        /// Error stream (default stderr)
        /// </summary>
        TextWriter ErrorStream { get; set; }

        /// <summary>
        /// Output stream (default stdout)
        /// </summary>
        TextWriter Output { get; set; }

        /// <summary>
        /// The prefix of WriteLine() output, usually used to make indentation.
        /// </summary>
        string Prefix { get; set; }

        /// <summary>
        /// Color settings for this IOServer. (default DefaultColorSetting)
        /// </summary>
        IColorSetting ColorSetting { get; set; }

        /// <summary>
        /// Prompt server for the io server.
        /// </summary>
        IPromptServer Prompt { get; set; }

        /// <summary>
        /// Input stream (default stdin)
        /// </summary>
        TextReader Input { get; set; }

        /// <summary>
        /// Checks if this IOServer's input stream is redirected (NOT stdin)
        /// </summary>
        bool IsInputRedirected { get; }

        /// <summary>
        /// get label of given output type
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
        /// provides packaging for ContentArray
        /// </summary>
        /// <param name="contents">ContentArray</param>
        /// <returns>packaged ContentArray</returns>
        public static IEnumerable<(string, ConsoleColor?)> CreateContentArray(params (string, ConsoleColor?)[] contents) => contents;


        /// <summary>
        /// provides packaging for ContentArray
        /// </summary>
        /// <param name="contents">ContentArray</param>
        /// <returns>packaged ContentArray</returns>
        public static IEnumerable<(string, ConsoleColor?,ConsoleColor?)> CreateContentArray(params (string, ConsoleColor?,ConsoleColor?)[] contents) => contents;
        /// <summary>
        /// Reset this IOServer's error stream to stderr
        /// </summary>
        void ResetError();

        /// <summary>
        /// Reset this IOServer's output stream to stdout
        /// </summary>
        void ResetOutput();

        /// <summary>
        /// Append a str to Prefix, usually used to increase indentation
        /// </summary>
        /// <param name="str">the str to append</param>
        void AppendWriteLinePrefix(string str = "\t");

        /// <summary>
        /// Subtract a str from Prefix, usually used to decrease indentation
        /// </summary>
        void SubtractWriteLinePrefix();

        /// <summary>
        /// Writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console</param>
        void Write(string content, ConsoleColor? customColor);

        /// <inheritdoc/>
        void Write(string content, ConsoleColor frontColor, ConsoleColor backColor);

        /// <summary>
        /// Writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like.</param>
        /// <param name="customColor">Optional. Customized color in console</param>
        void Write(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null);

        /// <inheritdoc/>
        Task WriteAsync(string content, ConsoleColor frontColor, ConsoleColor backColor);

        /// <summary>
        /// Asynchronously writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console</param>
        Task WriteAsync(string content, ConsoleColor? customColor);

        /// <summary>
        /// Asynchronously writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like.</param>
        /// <param name="customColor">Optional. Customized color in console</param>
        Task WriteAsync(string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null);

        /// <summary>
        /// Write a blank line to output stream.
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console.</param>
        void WriteLine(string content, ConsoleColor customColor);

        /// <summary>
        /// Writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        /// <param name="customColor">Optional. Customized color in console.</param>
        void WriteLine(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null);

        /// <summary>
        /// Writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of output (in console)</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        void WriteLine(IEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default);

        /// <summary>
        /// Writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray.
        /// FOR EACH Tuple, first is a part of content;
        /// second is optional, the foreground color of output (in console),
        /// third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        void WriteLine(IEnumerable<(string, ConsoleColor?,ConsoleColor?)> contentArray, OutputType type = OutputType.Default);

        /// <summary>
        /// Asynchronously writes a blank line to output stream.
        /// </summary>
        Task WriteLineAsync();

        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console.</param>
        Task WriteLineAsync(string content, ConsoleColor customColor);

        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        /// <param name="customColor">Optional. Customized color in console.</param>
        Task WriteLineAsync(string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null);

        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of output (in console)</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        Task WriteLineAsync(IEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of output (in console)</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        Task WriteLineAsync(IAsyncEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray.
        /// FOR EACH Tuple, first is a part of content;
        /// second is optional, the foreground color of output (in console),
        /// third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        Task WriteLineAsync(IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray, OutputType type = OutputType.Default);

        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray.
        /// FOR EACH Tuple, first is a part of content;
        /// second is optional, the foreground color of output (in console),
        /// third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        Task WriteLineAsync(IAsyncEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray, OutputType type = OutputType.Default);

        /// <summary>
        /// Reset this IOServer's input stream to stdin
        /// </summary>
        void ResetInput();

        /// <summary>
        /// Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        string? ReadLine(string? prompt, bool newLine, ConsoleColor? customPromptColor = null);

        /// <summary>
        /// Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        string? ReadLine(string? prompt, ConsoleColor? customPromptColor);

        /// <summary>
        /// Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        string? ReadLine(string? prompt, string? defaultValue,
            ConsoleColor? customPromptColor);

        /// <summary>
        /// Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        string? ReadLine(string? prompt = null, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null);

        /// <summary>
        /// Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        Task<string?> ReadLineAsync(string? prompt, bool newLine,
            ConsoleColor? customPromptColor = null);

        /// <summary>
        /// Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        Task<string?> ReadLineAsync(string? prompt, ConsoleColor? customPromptColor);

        /// <summary>
        /// Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        Task<string?> ReadLineAsync(string? prompt, string? defaultValue,
            ConsoleColor? customPromptColor);

        /// <summary>
        /// Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        Task<string?> ReadLineAsync(string? prompt = null, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null);

        /// <summary>
        /// Reads the next character from input stream without changing the state of the reader or the character source. 
        /// </summary>
        /// <returns>The next available character.</returns>
        int Peek();

        /// <summary>
        /// Reads the next character from input stream.
        /// </summary>
        /// <returns>The next available character.</returns>
        int Read();

        /// <summary>
        /// Reads all characters from the current position to the end of the input stream and returns them as one string.
        /// </summary>
        /// <returns>All characters from the current position to the end.</returns>
        string ReadToEnd();

        /// <summary>
        /// Asynchronously reads all characters from the current position to the end of the input stream and returns them as one string.
        /// </summary>
        /// <returns>All characters from the current position to the end.</returns>
        Task<string> ReadToEndAsync();
    }
}
