using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.Core.Services
{
    
    /// <summary>
    ///     A entity, which serves the input/output of a mobile suit.
    /// </summary>
    public class IOHub : IIOHub
    {

        /// <summary>
        ///     Initialize a IOServer.
        /// </summary>
        public IOHub(PromptFormatter promptFormatter, IIOHubConfigurer configurer)
        {
            ColorSetting = IColorSetting.DefaultColorSetting;
            Input = Console.In;
            Output=Console.Out;
            ErrorStream = Console.Error;
            FormatPrompt = promptFormatter;
            configurer(this);
        }


        /// <inheritdoc />
        public IColorSetting ColorSetting { get; set; }
        /// <inheritdoc />
        public PromptFormatter FormatPrompt{ get; }
        /// <inheritdoc />
        public TextReader Input { get; set; }

        /// <inheritdoc />
        public bool IsInputRedirected => !Console.In.Equals(Input);

        /// <inheritdoc />
        public void ResetInput()
        {
            Input = Console.In;
        }

        /// <inheritdoc />
        public string? ReadLine()
            => Input.ReadLine();
        /// <inheritdoc />
        public async Task<string?> ReadLineAsync()
            => await Input.ReadLineAsync();

        /// <inheritdoc />
        public int Peek()
        {
            return Input.Peek();
        }

        /// <inheritdoc />
        public int Read()
        {
            return Input.Read();
        }

        /// <inheritdoc />
        public string ReadToEnd()
        {
            return Input.ReadToEnd();
        }

        /// <inheritdoc />
        public async Task<string> ReadToEndAsync()
        {
            return await Input.ReadToEndAsync().ConfigureAwait(false);
        }
        /// <inheritdoc />
        public bool DisableTags { get; set; }

        /// <inheritdoc />
        public bool IsErrorRedirected => !Console.Error.Equals(ErrorStream);

        /// <inheritdoc />
        public bool IsOutputRedirected => !Console.Out.Equals(Output);

        /// <inheritdoc />
        public TextWriter ErrorStream { get; set; }

        /// <inheritdoc />
        public TextWriter Output { get; set; }

        private List<PrintUnit> Prefix { get; } = new();
        /// <inheritdoc />
        public void ResetError()
        {
            ErrorStream = Console.Error;
        }
        /// <inheritdoc />
        public void ResetOutput()
        {
            Output = Console.Out;
        }
        /// <inheritdoc/>
        public void AppendWriteLinePrefix(PrintUnit prefix)
        {
            Prefix.Add(prefix);
        }


        /// <inheritdoc/>
        public void SubtractWriteLinePrefix()
        {
            Prefix.RemoveAt(Prefix.Count - 1);
        }
        /// <inheritdoc/>
        public void ClearWriteLinePrefix()
        {
            Prefix.Clear();
        }
        /// <inheritdoc />
        public void Write(PrintUnit content)
        {
            if (content.Foreground is { } f)
            {
                Output.Write($"\u001b[38;2;{f.R};{f.G};{f.B}m");
            }
            if (content.Background is { } b)
            {
                Output.Write($"\u001b[48;2;{b.R};{b.G};{b.B}m");
            }
            Output.Write(content.Text);
            if (content.Foreground is { } || content.Background is { })
            {
                Output.Write("\u001b[0m");
            }
        }
        /// <inheritdoc />
        public async Task WriteAsync(PrintUnit content)
        {
            if (content.Foreground is { } f)
            {
                await Output.WriteAsync($"\u001b[38;2;{f.R};{f.G};{f.B}m");
            }
            if (content.Background is { } b)
            {
                await Output.WriteAsync($"\u001b[48;2;{b.R};{b.G};{b.B}m");
            }
            await Output.WriteAsync(content.Text);
            if (content.Foreground is { } || content.Background is { })
            {
                await Output.WriteAsync("\u001b[0m");
            }
        }



        /// <inheritdoc/>
        public IEnumerable<PrintUnit> GetLinePrefix(OutputType type)
        {
            if (IsOutputRedirected)
            {
                if (DisableTags) return Array.Empty<PrintUnit>();
                var sb = new StringBuilder();
                if (DisableTags) return new PrintUnit[] { ("", null) };
                AppendTimeStamp(sb);
                AppendTimeStamp(sb);
                sb.Append(IIOHub.GetLabel(type));
                return new PrintUnit[] { (sb.ToString(), null) };
            }

            return Prefix;

        }
        private static void AppendTimeStamp(StringBuilder sb)
        {
            sb.Append('[');
            sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
            sb.Append(']');
        }
    }
}