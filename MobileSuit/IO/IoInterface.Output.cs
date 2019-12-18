using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileSuit.IO
{
    public partial class IoInterface
    {
        public bool IsErrorRedirected => !Console.Error.Equals(Error);
        public void ResetError() => Error = Console.Error;
        public void ResetOutput() => Output = Console.Out;
        public TextWriter Error { get; set; }
        public TextWriter Output { get; set; }
        public string Prefix => PrefixBuilder.ToString();
        private StringBuilder PrefixBuilder { get; } = new StringBuilder();
        private Stack<int> PrefixLengthStack { get; } = new Stack<int>();
        public void SetWriteLinePrefix(string prefix)
        {
            ResetWriteLinePrefix();
            PrefixBuilder.Append(prefix);
            PrefixLengthStack.Push(prefix.Length);
        }
        public void ResetWriteLinePrefix()
        {
            PrefixBuilder.Clear();
            PrefixLengthStack.Clear();
        }
        public void AppendWriteLinePrefix(string prefix = "\t")
        {
            PrefixBuilder.Append(prefix);
            PrefixLengthStack.Push(prefix.Length);
        }
        public void SubtractWriteLinePrefix()
        {
            if (PrefixLengthStack.Count == 0) return;
            var l = PrefixLengthStack.Pop();
            PrefixBuilder.Remove(PrefixBuilder.Length - l, l);
        }
        public void Write(string content, ConsoleColor? customColor)
            => Write(content, default, customColor);
        public void Write(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
                ClearProgressBars();
                if (type == OutputType.Error) Console.Beep();
                if (customColor == null)
                {
                    customColor = type switch
                    {
                        OutputType.Default => DefaultColor,
                        OutputType.Prompt => PromptColor,
                        OutputType.Error => ErrorColor,
                        OutputType.AllOk => AllOkColor,
                        OutputType.ListTitle => ListTitleColor,
                        OutputType.CustomInfo => CustomInformationColor,
                        OutputType.MobileSuitInfo => InformationColor,
                        _ => DefaultColor
                    };
                }
                var dColor = Console.ForegroundColor;
                Console.ForegroundColor = (ConsoleColor)customColor;
                Console.Write(content);
                Console.ForegroundColor = dColor;
                LastCursorLocation = (Console.CursorLeft, Console.CursorTop);
                PrintProgressBars();
            }
            else
            {
                if (type != OutputType.Prompt)
                    Output?.Write(content);
            }

        }
        public async Task WriteAsync(string content, ConsoleColor? customColor)
            => await WriteAsync(content, default, customColor);
        public async Task WriteAsync(string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
                var tClear = ClearProgressBarsAsync();
                if (type == OutputType.Error) Console.Beep();
                if (customColor == null)
                {
                    customColor = type switch
                    {
                        OutputType.Default => DefaultColor,
                        OutputType.Prompt => PromptColor,
                        OutputType.Error => ErrorColor,
                        OutputType.AllOk => AllOkColor,
                        OutputType.ListTitle => ListTitleColor,
                        OutputType.CustomInfo => CustomInformationColor,
                        OutputType.MobileSuitInfo => InformationColor,
                        _ => DefaultColor
                    };
                }

                var dColor = Console.ForegroundColor;
                await tClear;
                Console.ForegroundColor = (ConsoleColor)customColor;
                await Output.WriteAsync(content);
                Console.ForegroundColor = dColor;
                LastCursorLocation = (Console.CursorLeft, Console.CursorTop);
                await PrintProgressBarsAsync();
            }
            else
            {
                if (type != OutputType.Prompt)
                    await Output.WriteAsync(content);
            }
        }
        public void WriteLine()
            => WriteLine("");
        public void WriteLine(string content, ConsoleColor customColor)
            => WriteLine(content, default, customColor);
        public void WriteLine(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
                ClearProgressBars();
                if (type == OutputType.Error) Console.Beep();
                if (customColor == null)
                {
                    customColor = type switch
                    {
                        OutputType.Default => DefaultColor,
                        OutputType.Prompt => PromptColor,
                        OutputType.Error => ErrorColor,
                        OutputType.AllOk => AllOkColor,
                        OutputType.ListTitle => ListTitleColor,
                        OutputType.CustomInfo => CustomInformationColor,
                        OutputType.MobileSuitInfo => InformationColor,
                        _ => DefaultColor,
                    };
                }

                var dColor = Console.ForegroundColor;

                Console.ForegroundColor = (ConsoleColor)customColor;

                Console.WriteLine(Prefix + content);

                Console.ForegroundColor = dColor;
                LastCursorLocation = (Console.CursorLeft, Console.CursorTop);
                PrintProgressBars();
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                switch (type)
                {
                    case OutputType.Default:
                        break;
                    case OutputType.Prompt:
                        sb.Append("[Prompt]");
                        break;
                    case OutputType.Error:
                        sb.Append("[Error]");
                        break;
                    case OutputType.AllOk:
                        sb.Append("[AllOk]");
                        break;
                    case OutputType.ListTitle:
                        sb.Append("[List]");
                        break;
                    case OutputType.CustomInfo:
                    case OutputType.MobileSuitInfo:
                        sb.Append("[Info]");
                        break;
                    default:
                        break;
                }

                sb.Append(content);
                Output?.WriteLine(sb.ToString());
            }
        }
        public void WriteLine(IEnumerable<(string, ConsoleColor?)> contentGroup, OutputType type = OutputType.Default)
        {
            if (!IsOutputRedirected)
            {
                ClearProgressBars();
                if (type == OutputType.Error) Console.Beep();
                var dColor = Console.ForegroundColor;
                var defaultColor =  type switch
                {
                    OutputType.Default => DefaultColor,
                    OutputType.Prompt => PromptColor,
                    OutputType.Error => ErrorColor,
                    OutputType.AllOk => AllOkColor,
                    OutputType.ListTitle => ListTitleColor,
                    OutputType.CustomInfo => CustomInformationColor,
                    OutputType.MobileSuitInfo => InformationColor,
                    _ => DefaultColor,
                };
                
                Console.ForegroundColor = defaultColor;
                Console.Write(Prefix);
                foreach (var (content,color) in contentGroup)
                {
                    Console.ForegroundColor = color ?? defaultColor;
                    Console.Write(content);
                }
                Console.Write("\n");
                Console.ForegroundColor = dColor;
                LastCursorLocation = (Console.CursorLeft, Console.CursorTop);
                PrintProgressBars();
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                switch (type)
                {
                    case OutputType.Default:
                        break;
                    case OutputType.Prompt:
                        sb.Append("[Prompt]");
                        break;
                    case OutputType.Error:
                        sb.Append("[Error]");
                        break;
                    case OutputType.AllOk:
                        sb.Append("[AllOk]");
                        break;
                    case OutputType.ListTitle:
                        sb.Append("[List]");
                        break;
                    case OutputType.CustomInfo:
                    case OutputType.MobileSuitInfo:
                        sb.Append("[Info]");
                        break;
                    default:
                        break;
                }

                foreach (var (content,_) in contentGroup)
                {
                    sb.Append(content);
                }

                Output?.WriteLine(sb.ToString());
            }
        }
        public async Task WriteLineAsync()
            => await WriteLineAsync("");
        public async Task WriteLineAsync(string content, ConsoleColor customColor)
            => await WriteLineAsync(content, default, customColor);
        public async Task WriteLineAsync(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
                var tClear = ClearProgressBarsAsync();
                if (type == OutputType.Error) Console.Beep();
                if (customColor == null)
                {
                    customColor = type switch
                    {
                        OutputType.Default => DefaultColor,
                        OutputType.Prompt => PromptColor,
                        OutputType.Error => ErrorColor,
                        OutputType.AllOk => AllOkColor,
                        OutputType.ListTitle => ListTitleColor,
                        OutputType.CustomInfo => CustomInformationColor,
                        OutputType.MobileSuitInfo => InformationColor,
                        _ => DefaultColor,
                    };
                }

                var dColor = Console.ForegroundColor;
                await tClear;
                Console.ForegroundColor = (ConsoleColor)customColor;

                await Output.WriteLineAsync(Prefix + content);
                Console.ForegroundColor = dColor;
                LastCursorLocation = (Console.CursorLeft, Console.CursorTop);
                await PrintProgressBarsAsync();
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                switch (type)
                {
                    case OutputType.Default:
                        break;
                    case OutputType.Prompt:
                        sb.Append("[Prompt]");
                        break;
                    case OutputType.Error:
                        sb.Append("[Error]");
                        break;
                    case OutputType.AllOk:
                        sb.Append("[AllOk]");
                        break;
                    case OutputType.ListTitle:
                        sb.Append("[List]");
                        break;
                    case OutputType.CustomInfo:
                    case OutputType.MobileSuitInfo:
                        sb.Append("[Info]");
                        break;
                    default:
                        break;
                }

                sb.Append(Prefix);
                sb.Append(content);
                await Output.WriteLineAsync(sb.ToString());
            }
        }
        public async Task WriteLineAsync(IEnumerable<(string, ConsoleColor?)> contentGroup, OutputType type = OutputType.Default)
        {
            if (!IsOutputRedirected)
            {
                var tClear=ClearProgressBarsAsync();
                if (type == OutputType.Error) Console.Beep();
                var dColor = Console.ForegroundColor;
                var defaultColor = type switch
                {
                    OutputType.Default => DefaultColor,
                    OutputType.Prompt => PromptColor,
                    OutputType.Error => ErrorColor,
                    OutputType.AllOk => AllOkColor,
                    OutputType.ListTitle => ListTitleColor,
                    OutputType.CustomInfo => CustomInformationColor,
                    OutputType.MobileSuitInfo => InformationColor,
                    _ => DefaultColor,
                };
                await tClear;
                Console.ForegroundColor = defaultColor;
                await Output.WriteAsync(Prefix);
                foreach (var (content, color) in contentGroup)
                {
                    Console.ForegroundColor = color ?? defaultColor;
                    await Output.WriteAsync(content);
                }
                await Output.WriteAsync("\n");
                Console.ForegroundColor = dColor;
                LastCursorLocation = (Console.CursorLeft, Console.CursorTop);
                await PrintProgressBarsAsync();
            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");
                switch (type)
                {
                    case OutputType.Default:
                        break;
                    case OutputType.Prompt:
                        sb.Append("[Prompt]");
                        break;
                    case OutputType.Error:
                        sb.Append("[Error]");
                        break;
                    case OutputType.AllOk:
                        sb.Append("[AllOk]");
                        break;
                    case OutputType.ListTitle:
                        sb.Append("[List]");
                        break;
                    case OutputType.CustomInfo:
                    case OutputType.MobileSuitInfo:
                        sb.Append("[Info]");
                        break;
                    default:
                        break;
                }

                foreach (var (content, _) in contentGroup)
                {
                    sb.Append(content);
                    
                }

                await Output.WriteLineAsync(sb.ToString());
            }
        }
        public void WriteLineError(string content)
        {
            if (!IsOutputRedirected)
            {
                Console.Beep();

                Error.WriteLine(Prefix + content);


            }
            else
            {
                var sb = new StringBuilder("[");
                sb.Append(DateTime.Now.ToString(CultureInfo.InvariantCulture));
                sb.Append("]");

                sb.Append(content);
                Error.WriteLine(sb.ToString());
            }
        }
        public bool IsOutputRedirected => !Console.Out.Equals(Output);
    }
}
