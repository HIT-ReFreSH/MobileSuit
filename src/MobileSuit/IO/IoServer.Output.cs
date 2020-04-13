﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.IO
{

    public partial class IOServer
    {
        /// <summary>
        /// Check if this IOServer's error stream is redirected (NOT stderr)
        /// </summary>
        public bool IsErrorRedirected => !Console.Error.Equals(Error);
        /// <summary>
        /// Check if this IOServer's output stream is redirected (NOT stdout)
        /// </summary>
        public bool IsOutputRedirected => !Console.Out.Equals(Output);
        /// <summary>
        /// Error stream (default stderr)
        /// </summary>
        public TextWriter Error { get; set; }
        /// <summary>
        /// Output stream (default stdout)
        /// </summary>
        public TextWriter Output { get; set; }
        /// <summary>
        /// The prefix of WriteLine() output, usually used to make indentation.
        /// </summary>
        public string Prefix
        {
            get => PrefixBuilder.ToString();
            set
            {
                ResetWriteLinePrefix();
                PrefixBuilder.Append(value);
                PrefixLengthStack.Push(value.Length);
            }
        }

        private StringBuilder PrefixBuilder { get; } = new StringBuilder();
        private Stack<int> PrefixLengthStack { get; } = new Stack<int>();
        private ConsoleColor SelectColor(OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            return customColor
                   ?? type switch
                   {
                       OutputType.Default => ColorSetting.DefaultColor,
                       OutputType.Prompt => ColorSetting.PromptColor,
                       OutputType.Error => ColorSetting.ErrorColor,
                       OutputType.AllOk => ColorSetting.AllOkColor,
                       OutputType.ListTitle => ColorSetting.ListTitleColor,
                       OutputType.CustomInfo => ColorSetting.CustomInformationColor,
                       OutputType.MobileSuitInfo => ColorSetting.InformationColor,
                       _ => ColorSetting.DefaultColor
                   };
        }
        private static string GetLabel(OutputType type = OutputType.Default)
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
        /// Reset this IOServer's error stream to stderr
        /// </summary>
        public void ResetError()
        {
            Error = Console.Error;
        }
        /// <summary>
        /// Reset this IOServer's output stream to stdout
        /// </summary>
        public void ResetOutput()
        {
            Output = Console.Out;
        }

        private void ResetWriteLinePrefix()
        {
            PrefixBuilder.Clear();
            PrefixLengthStack.Clear();
        }
        /// <summary>
        /// Append a str to Prefix, usually used to increase indentation
        /// </summary>
        /// <param name="str">the str to append</param>
        public void AppendWriteLinePrefix(string str = "\t")
        {
            PrefixBuilder.Append(str);
            PrefixLengthStack.Push(str.Length);
        }
        /// <summary>
        /// Subtract a str from Prefix, usually used to decrease indentation
        /// </summary>
        public void SubtractWriteLinePrefix()
        {
            if (PrefixLengthStack.Count == 0) return;
            var l = PrefixLengthStack.Pop();
            PrefixBuilder.Remove(PrefixBuilder.Length - l, l);
        }
        /// <summary>
        /// Writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console</param>
        public void Write(string content, ConsoleColor? customColor) => Write(content, default, customColor);
        /// <summary>
        /// Writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like.</param>
        /// <param name="customColor">Optional. Customized color in console</param>
        public void Write(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
                if (type == OutputType.Error) Console.Beep();
                var dColor = Console.ForegroundColor;
                Console.ForegroundColor = SelectColor(type, customColor);
                Console.Write(content);
                Console.ForegroundColor = dColor;
            }
            else
            {
                if (type != OutputType.Prompt)
                    Output?.Write(content);
            }
        }
        /// <summary>
        /// Asynchronously writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console</param>
        public async Task WriteAsync(string content, ConsoleColor? customColor) => await WriteAsync(content, default, customColor);
        /// <summary>
        /// Asynchronously writes some content to output stream. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like.</param>
        /// <param name="customColor">Optional. Customized color in console</param>
        public async Task WriteAsync(string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
                if (type == OutputType.Error) Console.Beep();

                var dColor = Console.ForegroundColor;
                Console.ForegroundColor = SelectColor(type, customColor);
                await Output.WriteAsync(content);
                Console.ForegroundColor = dColor;
            }
            else
            {
                if (type != OutputType.Prompt)
                    await Output.WriteAsync(content);
            }
        }
        /// <summary>
        /// Write a blank line to output stream.
        /// </summary>
        public void WriteLine() => WriteLine("");
        /// <summary>
        /// Writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console.</param>
        public void WriteLine(string content, ConsoleColor customColor) => WriteLine(content, default, customColor);
        /// <summary>
        /// Writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        /// <param name="customColor">Optional. Customized color in console.</param>
        public void WriteLine(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
                if (type == OutputType.Error) Console.Beep();
                var dColor = Console.ForegroundColor;
                Console.ForegroundColor = SelectColor(type, customColor);
                Console.WriteLine(Prefix + content);
                Console.ForegroundColor = dColor;
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                sb.Append(GetLabel(type));
                sb.Append(content);
                Output?.WriteLine(sb.ToString());
            }
        }
        /// <summary>
        /// Writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of output (in console)</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public void WriteLine(IEnumerable<(string, ConsoleColor?)> contentArray, OutputType type = OutputType.Default)
        {
            if (!IsOutputRedirected)
            {
                if (type == OutputType.Error) Console.Beep();
                var dColor = Console.ForegroundColor;
                var defaultColor = SelectColor(type);

                Console.ForegroundColor = defaultColor;
                Console.Write(Prefix);
                foreach (var (content, color) in contentArray)
                {
                    Console.ForegroundColor = color ?? defaultColor;
                    Console.Write(content);
                }

                Console.Write("\n");
                Console.ForegroundColor = dColor;
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                sb.Append(GetLabel(type));

                foreach (var (content, _) in contentArray) sb.Append(content);

                Output?.WriteLine(sb.ToString());
            }
        }
        /// <summary>
        /// Asynchronously writes a blank line to output stream.
        /// </summary>
        public async Task WriteLineAsync() => await WriteLineAsync("");
        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">content to output.</param>
        /// <param name="customColor">Customized color in console.</param>
        public async Task WriteLineAsync(string content, ConsoleColor customColor) => await WriteLineAsync(content, default, customColor);
        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color in console.
        /// </summary>
        /// <param name="content">Content to output.</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        /// <param name="customColor">Optional. Customized color in console.</param>
        public async Task WriteLineAsync(string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
                if (type == OutputType.Error) Console.Beep();
                var dColor = Console.ForegroundColor;
                Console.ForegroundColor = SelectColor(type, customColor);
                await Output.WriteLineAsync(Prefix + content);
                Console.ForegroundColor = dColor;
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                sb.Append(GetLabel(type));
                sb.Append(Prefix);
                sb.Append(content);
                await Output.WriteLineAsync(sb.ToString());
            }
        }
        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of output (in console)</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public async Task WriteLineAsync(IEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            if (!IsOutputRedirected)
            {
                if (type == OutputType.Error) Console.Beep();
                var dColor = Console.ForegroundColor;
                var defaultColor = Console.ForegroundColor = SelectColor(type);
                Console.ForegroundColor = defaultColor;
                await Output.WriteAsync(Prefix);
                foreach (var (content, color) in contentArray)
                {
                    Console.ForegroundColor = color ?? defaultColor;
                    await Output.WriteAsync(content);
                }

                await Output.WriteAsync("\n");
                Console.ForegroundColor = dColor;
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                sb.Append(GetLabel(type));

                foreach (var (content, _) in contentArray) sb.Append(content);

                await Output.WriteLineAsync(sb.ToString());
            }
        }
        /// <summary>
        /// Asynchronously writes some content to output stream, with line break. With certain color for each part of content in console.
        /// </summary>
        /// <param name="contentArray">TupleArray. FOR EACH Tuple, first is a part of content; second is optional, the color of output (in console)</param>
        /// <param name="type">Optional. Type of this content, this decides how will it be like (color in Console, label in file).</param>
        public async Task WriteLineAsync(IAsyncEnumerable<(string, ConsoleColor?)> contentArray,
            OutputType type = OutputType.Default)
        {
            if (!IsOutputRedirected)
            {
                if (type == OutputType.Error) Console.Beep();
                var dColor = Console.ForegroundColor;
                var defaultColor = Console.ForegroundColor = SelectColor(type);
                Console.ForegroundColor = defaultColor;
                await Output.WriteAsync(Prefix);
                await foreach (var (content, color) in contentArray)
                {
                    Console.ForegroundColor = color ?? defaultColor;
                    await Output.WriteAsync(content);
                }

                await Output.WriteAsync("\n");
                Console.ForegroundColor = dColor;
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                sb.Append(GetLabel(type));

                await foreach (var (content, _) in contentArray) sb.Append(content);

                await Output.WriteLineAsync(sb.ToString());
            }
        }

    }
}