using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit
{
    public static class IOExtensions
    {
        /// <summary>
        ///     Append a str to Prefix, usually used to increase indentation
        /// </summary>
        /// <param name="prefix">the output tuple to append</param>
        public static void AppendWriteLinePrefix(this IIOHub hub, string prefix)
        {
            hub.AppendWriteLinePrefix((prefix, null, null));
        }

        /// <summary>
        ///     Writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console</param>
        public static void Write(this IIOHub hub, string content, ConsoleColor? customColor);


        /// <summary>
        ///     Writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like.</param>
        /// <param name="customColor">Optional. Customized color in console</param>
        public static void Write(this IIOHub hub, string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null);


        /// <summary>
        ///     Asynchronously writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console</param>
        public static Task WriteAsync(this IIOHub hub, string content, ConsoleColor? customColor);

        /// <summary>
        ///     Asynchronously writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like.</param>
        /// <param name="customColor">Optional. Customized color in console</param>
        public static Task WriteAsync(this IIOHub hub, string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null);

        /// <summary>
        ///     Write a blank line to output stream.
        /// </summary>
        public static void WriteLine(this IIOHub hub);

        /// <summary>
        ///     Writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console.</param>
        public static void WriteLine(this IIOHub hub, string content, ConsoleColor customColor);

        /// <summary>
        ///     Writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        /// <param name="customColor">Optional. Customized color in console.</param>
        public static void WriteLine(this IIOHub hub, string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null);
        /// <summary>
        ///     Writes some content to output stream. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of
        ///     output (in console)
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static void Write(this IIOHub hub, IEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default);

        /// <summary>
        ///     Writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static void Write(this IIOHub hub, IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);
        /// <summary>
        ///     Writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of
        ///     output (in console)
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static void WriteLine(this IIOHub hub, IEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default);

        /// <summary>
        ///     Writes some content to output stream. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static void WriteLine(this IIOHub hub, IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Asynchronously writes a blank line to output stream.
        /// </summary>
        public static Task WriteLineAsync(this IIOHub hub);

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console.</param>
        public static Task WriteLineAsync(this IIOHub hub, string content, ConsoleColor customColor);

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        /// <param name="customColor">Optional. Customized color in console.</param>
        public static Task WriteLineAsync(this IIOHub hub, string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null);

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of
        ///     output (in console)
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteLineAsync(this IIOHub hub, IEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of
        ///     output (in console)
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteLineAsync(this IIOHub hub, IAsyncEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteLineAsync(this IIOHub hub, IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteAsync(this IIOHub hub, IAsyncEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);
        /// <summary>
        ///     Asynchronously writes some content to output stream. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of
        ///     output (in console)
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteAsync(this IIOHub hub, IEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Asynchronously writes some content to output stream. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of
        ///     output (in console)
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteAsync(this IIOHub hub, IAsyncEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Asynchronously writes some content to output stream. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteAsync(this IIOHub hub, IEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteLineAsync(this IIOHub hub,IAsyncEnumerable<(string, ConsoleColor?, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public static string? ReadLine(this IIOHub hub, string? prompt, bool newLine, ConsoleColor? customPromptColor = null);

        /// <summary>
        ///     Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public static string? ReadLine(this IIOHub hub, string? prompt, ConsoleColor? customPromptColor);

        /// <summary>
        ///     Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public static string? ReadLine(this IIOHub hub, string? prompt, string? defaultValue,
            ConsoleColor? customPromptColor);

        /// <summary>
        ///     Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public static string? ReadLine(this IIOHub hub, string? prompt = null, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null);

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public static Task<string?> ReadLineAsync(this IIOHub hub, string? prompt, bool newLine,
            ConsoleColor? customPromptColor = null);

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public static Task<string?> ReadLineAsync(this IIOHub hub, string? prompt, ConsoleColor? customPromptColor);

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public static Task<string?> ReadLineAsync(this IIOHub hub, string? prompt, string? defaultValue,
            ConsoleColor? customPromptColor);

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public static Task<string?> ReadLineAsync(this IIOHub hub, string? prompt = null, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null);
    }
}
