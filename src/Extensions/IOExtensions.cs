using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit
{
    using static SuitTools;
    /// <summary>
    /// A basic unit of output print, contains foreground, background and text.
    /// </summary>
    public struct PrintUnit
    {
        /// <summary>
        /// Text of output.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Foreground color of output.
        /// </summary>
        public ConsoleColor? Foreground { get; set; }
        /// <summary>
        /// Background color of output.
        /// </summary>
        public ConsoleColor? Background { get; set; }
        /// <summary>
        /// Convert from print unit to a tuple
        /// </summary>
        /// <param name="pu">The print unit</param>
        public static implicit operator (string, ConsoleColor?, ConsoleColor?)(PrintUnit pu)
        {
            return (pu.Text, pu.Foreground, pu.Background);

        }
        /// <summary>
        /// Convert from print unit back to a tuple
        /// </summary>
        /// <param name="tp">The print unit</param>
        public static implicit operator PrintUnit((string, ConsoleColor?, ConsoleColor?) tp)
        {
            return new()
            {
                Text = tp.Item1,
                Foreground = tp.Item2,
                Background = tp.Item3
            };

        }
        /// <summary>
        /// Convert from print unit to a tuple
        /// </summary>
        /// <param name="pu">The print unit</param>
        public static implicit operator (string, ConsoleColor?)(PrintUnit pu)
        {
            return (pu.Text, pu.Foreground);

        }
        /// <summary>
        /// Convert from print unit back to a tuple
        /// </summary>
        /// <param name="tp">The print unit</param>
        public static implicit operator PrintUnit((string, ConsoleColor?) tp)
        {
            return new()
            {
                Text = tp.Item1,
                Foreground = tp.Item2,
                Background = null
            };

        }
    }
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
        public static async Task WriteAsync(this IIOHub hub, string content, ConsoleColor? customColor)
            => await hub.WriteAsync((content, customColor));

        /// <summary>
        ///     Asynchronously writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like.</param>
        /// <param name="customColor">Optional. Customized color in console</param>
        public static async Task WriteAsync(this IIOHub hub, string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null)
        {
            var selColor = IColorSetting.SelectColor(hub.ColorSetting, type, customColor);
            if (type == OutputType.Prompt)
                await hub.WriteAsync(hub.Prompt.FormatPrompt(CreateContentArray((content, selColor))));
        }

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
        public static void Write(this IIOHub hub, IEnumerable<PrintUnit> contentArray,
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
        public static void WriteLine(this IIOHub hub, IEnumerable<PrintUnit> contentArray,
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
        /// <param name="hub">IOHub to write to</param>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteLineAsync(this IIOHub hub, IEnumerable<PrintUnit> contentArray,
            OutputType type = OutputType.Default);

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="hub">IOHub to write to</param>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static async Task WriteAsync(this IIOHub hub, IAsyncEnumerable<PrintUnit> contentArray,
            OutputType type = OutputType.Default)
        {
            if (type != OutputType.Prompt)
            {
                await foreach (var unit in contentArray)    
                {
                    await hub.WriteAsync(unit);
                }
            }
            else
            {
                var carr = new List<PrintUnit>();
                await foreach (var unit in contentArray)
                {
                    carr.Add(unit);
                }

                await hub.WriteAsync(carr, type);
            }
        }

        /// <summary>
        ///     Asynchronously writes some content to output stream. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="hub">IOHub to write to</param>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static async Task WriteAsync(this IIOHub hub, IEnumerable<PrintUnit> contentArray,
            OutputType type = OutputType.Default)
        {
            if (type == OutputType.Prompt)
                contentArray = hub.Prompt.FormatPrompt(contentArray);
            foreach (var unit in contentArray)
            {
                await hub.WriteAsync(unit);
            }
        }

        /// <summary>
        ///     Asynchronously writes some content to output stream, with line break. With certain color for each part of content
        ///     in console.
        /// </summary>
        /// <param name="hub">IOHub to write to</param>
        /// <param name="contentArray">
        ///     TupleArray.
        ///     FOR EACH Tuple, first is a part of content;
        ///     second is optional, the foreground color of output (in console),
        ///     third is optional, the background color of output.
        /// </param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public static Task WriteLineAsync(this IIOHub hub, IAsyncEnumerable<PrintUnit> contentArray,
            OutputType type = OutputType.Default)
        {
            
        }

        /// <summary>
        ///     Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="hub">IOHub to read from</param>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public static string? ReadLine(this IIOHub hub, string prompt, bool newLine, ConsoleColor? customPromptColor = null)
        =>hub.ReadLine(prompt, null, newLine, customPromptColor);
        /// <summary>
        ///     Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="hub">IOHub to read from</param>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public static string? ReadLine(this IIOHub hub, string prompt, ConsoleColor? customPromptColor)
        =>hub.ReadLine(prompt, null, false, customPromptColor);

        /// <summary>
        ///     Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="hub">IOHub to read from</param>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public static string? ReadLine(this IIOHub hub, string prompt, string? defaultValue,
            ConsoleColor? customPromptColor)=>hub.ReadLine(prompt, defaultValue, false, customPromptColor);

        /// <summary>
        ///     Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="hub">IOHub to read from</param>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public static string? ReadLine(this IIOHub hub, string prompt, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null)
        {
            hub.Write(CreateReadLinePrompt(hub, prompt, defaultValue, customPromptColor), OutputType.Prompt);
            if (newLine)
                hub.Write("\n");

            var r = hub.ReadLine();
            if (r is null) return null;
            StringBuilder stringBuilder = new(r);
            while (stringBuilder.Length > 0 && stringBuilder[^1] == '%')
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                r = hub.ReadLine();
                if (r == null) break;
                stringBuilder.Append(r);
            }

            return stringBuilder.Length == 0 ? defaultValue : stringBuilder.ToString();
        }


        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="hub">IOHub to read from</param>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public static async Task<string?> ReadLineAsync(this IIOHub hub, string prompt, bool newLine,
            ConsoleColor? customPromptColor = null)=>
            await hub.ReadLineAsync(prompt, null, newLine, customPromptColor).ConfigureAwait(false);

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="hub">IOHub to read from</param>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public static async Task<string?> ReadLineAsync(this IIOHub hub, string prompt, ConsoleColor? customPromptColor)
        => await hub.ReadLineAsync(prompt, null, false, customPromptColor).ConfigureAwait(false);

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="hub">IOHub to read from</param>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public static async Task<string?> ReadLineAsync(this IIOHub hub, string prompt, string? defaultValue,
            ConsoleColor? customPromptColor)=> await hub.ReadLineAsync(prompt, defaultValue, false, customPromptColor).ConfigureAwait(false);

        private static IEnumerable<PrintUnit> CreateReadLinePrompt(IIOHub hub,string prompt, string? defaultValue , ConsoleColor? customPromptColor)
        {
            var printUnit0 = (prompt, customPromptColor ?? hub.ColorSetting.PromptColor);
            return defaultValue is null? 
                CreateContentArray(printUnit0):
                CreateContentArray(
                printUnit0,
                (defaultValue, hub.ColorSetting.SystemColor));
        }

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="hub">IOHub to read from</param>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public static async Task<string?> ReadLineAsync(this IIOHub hub, string prompt, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null)
        {
            await hub.WriteAsync(CreateReadLinePrompt(hub,prompt,defaultValue,customPromptColor), OutputType.Prompt);
            if (newLine)
                await hub.WriteAsync("\n");

            var r = await hub.ReadLineAsync().ConfigureAwait(false);
            if (r is null) return null;
            StringBuilder stringBuilder = new(r);
            while (stringBuilder.Length > 0 && stringBuilder[^1] == '%')
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                r = await hub.ReadLineAsync().ConfigureAwait(false);
                if (r == null) break;
                stringBuilder.Append(r);
            }

            return stringBuilder.Length == 0 ? defaultValue : stringBuilder.ToString();
        }
    }
}
