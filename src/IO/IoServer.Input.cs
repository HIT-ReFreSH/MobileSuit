#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.IO
{
    partial class IOServer
    {
        /// <summary>
        /// Input stream (default stdin)
        /// </summary>
        public TextReader Input { get; set; }
        /// <summary>
        /// Checks if this IOServer's input stream is redirected (NOT stdin)
        /// </summary>
        public bool IsInputRedirected => !Console.In.Equals(Input);
        /// <summary>
        /// Reset this IOServer's input stream to stdin
        /// </summary>
        public void ResetInput() => Input = Console.In;
        /// <summary>
        /// Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public string? ReadLine(string? prompt, bool newLine, ConsoleColor? customPromptColor = null)
            => ReadLine(prompt, null, newLine, customPromptColor);
        /// <summary>
        /// Reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public string? ReadLine(string? prompt, ConsoleColor? customPromptColor)
            => ReadLine(prompt, null, false, customPromptColor);
        /// <summary>
        /// Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public string? ReadLine(string? prompt, string? defaultValue,
            ConsoleColor? customPromptColor)
            => ReadLine(prompt, defaultValue, false, customPromptColor);
        /// <summary>
        /// Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public string? ReadLine(string? prompt = null, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null)
        {
            if (!string.IsNullOrEmpty(prompt))
            {
                if (newLine)
                    WriteLine(prompt + '>', OutputType.Prompt, customPromptColor);
                else
                    Write(prompt + '>', OutputType.Prompt, customPromptColor);
            }

            var r = Input.ReadLine();
            return string.IsNullOrEmpty(r) ? defaultValue : r;
        }

        /// <summary>
        /// Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="newLine">If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public async Task<string?> ReadLineAsync(string? prompt, bool newLine,
            ConsoleColor? customPromptColor = null)
            => await ReadLineAsync(prompt, null, newLine, customPromptColor).ConfigureAwait(false);

        /// <summary>
        /// Asynchronously reads a line from input stream, with prompt.
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="customPromptColor">Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF</returns>
        public async Task<string?> ReadLineAsync(string? prompt, ConsoleColor? customPromptColor) 
            => await ReadLineAsync(prompt, null, false, customPromptColor).ConfigureAwait(false);

        /// <summary>
        /// Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Default return value if user input ""</param>
        /// <param name="customPromptColor"></param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public async Task<string?> ReadLineAsync(string? prompt, string? defaultValue,
            ConsoleColor? customPromptColor)
            => await ReadLineAsync(prompt, defaultValue, false, customPromptColor).ConfigureAwait(false);

        /// <summary>
        /// Asynchronously reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <param name="prompt">Optional. The prompt display (output to output stream) before user input.</param>
        /// <param name="defaultValue">Optional. Default return value if user input ""</param>
        /// <param name="newLine">Optional. If the prompt will display in a single line</param>
        /// <param name="customPromptColor">Optional. Prompt's Color, ColorSetting.PromptColor as default.</param>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public async Task<string?> ReadLineAsync(string? prompt = null, string? defaultValue = null,
            bool newLine = false, ConsoleColor? customPromptColor = null)
        {
            if (!string.IsNullOrEmpty(prompt))
            {
                if (newLine)
                    await WriteLineAsync(prompt + '>', OutputType.Prompt, customPromptColor).ConfigureAwait(false);
                else
                    await WriteAsync(prompt + '>', OutputType.Prompt, customPromptColor).ConfigureAwait(false);
            }

            var r = await Input.ReadLineAsync().ConfigureAwait(false);
            return string.IsNullOrEmpty(r) ? defaultValue : r;
        }

        /// <summary>
        /// Reads the next character from input stream without changing the state of the reader or the character source. 
        /// </summary>
        /// <returns>The next available character.</returns>
        public int Peek() => Input.Peek();

        /// <summary>
        /// Reads the next character from input stream.
        /// </summary>
        /// <returns>The next available character.</returns>
        public int Read() => Input.Read();


        /// <summary>
        /// Reads all characters from the current position to the end of the input stream and returns them as one string.
        /// </summary>
        /// <returns>All characters from the current position to the end.</returns>
        public string ReadToEnd() => Input.ReadToEnd();
        /// <summary>
        /// Asynchronously reads all characters from the current position to the end of the input stream and returns them as one string.
        /// </summary>
        /// <returns>All characters from the current position to the end.</returns>
        public async Task<string> ReadToEndAsync() => await Input.ReadToEndAsync().ConfigureAwait(false);
    }
}