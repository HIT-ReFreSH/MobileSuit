#nullable enable
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.Core;

namespace PlasticMetal.MobileSuit.Services
{
    partial class IOHub
    {
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
        ///     Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public string? ReadLine()
            => Input.ReadLine();
        /// <summary>
        ///     Reads a line from input stream, with prompt. Return something default if user input "".
        /// </summary>
        /// <returns>Content from input stream, null if EOF, if user input "", return defaultValue</returns>
        public async Task<string?> ReadLineAsync()
            => await Input.ReadLineAsync();

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