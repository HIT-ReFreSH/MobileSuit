#nullable enable
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.UI
{
    partial class IOHub
    {
        private class IOHubInputHelper : IInputHelper
        {
            public string? Expression { get; set; }
            public string? DefaultInput { get; set; }
        }
        private readonly IOHubInputHelper _inputHelper = new();
        /// <inheritdoc/>
        public IInputHelper InputHelper => _inputHelper;

        /// <summary>
        ///     Input stream (default stdin)
        /// </summary>
        public TextReader Input { get; set; }

        /// <summary>
        ///     Checks if this IOServer's input stream is redirected (NOT stdin)
        /// </summary>
        public bool IsInputRedirected => !Console.In.Equals(Input);

        /// <summary>
        ///     Reset this IOServer's input stream to stdin
        /// </summary>
        public void ResetInput()
        {
            Input = Console.In;
        }

        /// <summary>
        ///     Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public string? ReadLine(string? prompt, bool newLine, ConsoleColor? customPromptColor = null)
        {
            return ReadLine(prompt, null, newLine, customPromptColor);
        }

        /// <summary>
        ///     Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public string? ReadLine(string? prompt, ConsoleColor? customPromptColor)
        {
            return ReadLine(prompt, null, false, customPromptColor);
        }

        /// <summary>
        ///     Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public string? ReadLine(string? prompt, string? defaultValue,
            ConsoleColor? customPromptColor)
        {
            return ReadLine(prompt, defaultValue, false, customPromptColor);
        }

        /// <summary>
        ///     Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public string? ReadLine(string? prompt = null, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null)
        {

            _inputHelper.Expression = prompt;
            _inputHelper.DefaultInput = defaultValue;
            Write(Prompt.GeneratePrompt(
                p => ReferenceEquals(p.Tag, this)), OutputType.Prompt);

            _inputHelper.Expression= _inputHelper.DefaultInput=null;

            if (newLine)
                WriteLine();

            var r = Input.ReadLine();
            if (r == null) return null;
            StringBuilder stringBuilder = new StringBuilder(r);
            while (stringBuilder.Length > 0 && stringBuilder[^1] == '%')
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                if ((r = Input.ReadLine()) == null) break;
                stringBuilder.Append(r);
            }

            return stringBuilder.Length == 0 ? defaultValue : stringBuilder.ToString();
        }

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public async Task<string?> ReadLineAsync(string? prompt, bool newLine,
            ConsoleColor? customPromptColor = null)
        {
            return await ReadLineAsync(prompt, null, newLine, customPromptColor).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public async Task<string?> ReadLineAsync(string? prompt, ConsoleColor? customPromptColor)
        {
            return await ReadLineAsync(prompt, null, false, customPromptColor).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public async Task<string?> ReadLineAsync(string? prompt, string? defaultValue,
            ConsoleColor? customPromptColor)
        {
            return await ReadLineAsync(prompt, defaultValue, false, customPromptColor).ConfigureAwait(false);
        }

        /// <summary>
        ///     Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public async Task<string?> ReadLineAsync(string? prompt = null, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null)
        {
            _inputHelper.Expression = prompt;
            _inputHelper.DefaultInput = defaultValue;
            await WriteAsync(Prompt.GeneratePrompt(
                p => ReferenceEquals(p.Tag, this)),OutputType.Prompt);

            _inputHelper.Expression = _inputHelper.DefaultInput = null;

            if (newLine)
                await WriteLineAsync();

            var r = await Input.ReadLineAsync().ConfigureAwait(false);
            if (r == null) return null;
            StringBuilder stringBuilder = new StringBuilder(r);
            while (stringBuilder.Length > 0 && stringBuilder[^1] == '%')
            {
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
                r = await Input.ReadLineAsync().ConfigureAwait(false);
                if (r == null) break;
                stringBuilder.Append(r);
            }

            return stringBuilder.Length == 0 ? defaultValue : stringBuilder.ToString();
        }

        /// <summary>
        ///     Reads the next character from input stream without changing the state of the reader or the character source.
        /// </summary>
        /// <returns>The next available character.</returns>
        public int Peek()
        {
            return Input.Peek();
        }

        /// <summary>
        ///     Reads the next character from input stream.
        /// </summary>
        /// <returns>The next available character.</returns>
        public int Read()
        {
            return Input.Read();
        }


        /// <summary>
        ///     Reads all characters from the current position to the end of the input stream and returns them as one string.
        /// </summary>
        /// <returns>All characters from the current position to the end.</returns>
        public string ReadToEnd()
        {
            return Input.ReadToEnd();
        }

        /// <summary>
        ///     Asynchronously reads all characters from the current position to the end of the input stream and returns them as
        ///     one string.
        /// </summary>
        /// <returns>All characters from the current position to the end.</returns>
        public async Task<string> ReadToEndAsync()
        {
            return await Input.ReadToEndAsync().ConfigureAwait(false);
        }
    }
}