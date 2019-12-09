#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace MobileSuit
{
    public class MobileSuitIoInterface
    {
        public void ResetError() => Error = Console.Error;
        public void ResetInput() => Input = Console.In;
        public void ResetOutput() => Output = Console.Out;
        public void Write(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
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
                        OutputType.CustomInfo => CustomInfoColor,
                        OutputType.MobileSuitInfo => MobileSuitInfoColor,
                        _ => DefaultColor
                    };
                }

                var dColor = Console.ForegroundColor;

                Console.ForegroundColor = (ConsoleColor) customColor;

                Console.Write(content);

                Console.ForegroundColor = dColor;
            }
            else
            {
                if (type != OutputType.Prompt)
                    Output?.Write(content);
            }
        }

        public async Task WriteAsync(string content, OutputType type = OutputType.Default,
            ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
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
                        OutputType.CustomInfo => CustomInfoColor,
                        OutputType.MobileSuitInfo => MobileSuitInfoColor,
                        _ => DefaultColor
                    };
                }

                var dColor = Console.ForegroundColor;

                Console.ForegroundColor = (ConsoleColor)customColor;

                await Output.WriteAsync(content);

                Console.ForegroundColor = dColor;
            }
            else
            {
                if (type != OutputType.Prompt)
                    await Output.WriteAsync(content);
            }
        }
        public void WriteLine(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
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
                        OutputType.CustomInfo => CustomInfoColor,
                        OutputType.MobileSuitInfo => MobileSuitInfoColor,
                        _ => DefaultColor,
                    };
                }

                var dColor = Console.ForegroundColor;

                Console.ForegroundColor = (ConsoleColor) customColor;

                Console.WriteLine(Prefix + content);

                Console.ForegroundColor = dColor;
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
        public async Task WriteLineAsync(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!IsOutputRedirected)
            {
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
                        OutputType.CustomInfo => CustomInfoColor,
                        OutputType.MobileSuitInfo => MobileSuitInfoColor,
                        _ => DefaultColor,
                    };
                }

                var dColor = Console.ForegroundColor;

                Console.ForegroundColor = (ConsoleColor)customColor;

                await Output.WriteLineAsync(Prefix + content);
                Console.ForegroundColor = dColor;
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

        public string? ReadLine()
            => Input.ReadLine();

        public async Task<string?> ReadLineAsync() 
            => await Input.ReadLineAsync();

        public int Peek() => Input.Peek();
        public int Read() => Input.Read();
        public string ReadToEnd() => Input.ReadToEnd();
        public async Task<string> ReadToEndAsync() => await Input.ReadToEndAsync();
        public MobileSuitIoInterface()
        {
            DefaultColor = ConsoleColor.White;
            ErrorColor = ConsoleColor.Red;
            PromptColor = ConsoleColor.Magenta;
            AllOkColor = ConsoleColor.Green;
            ListTitleColor = ConsoleColor.Yellow;
            CustomInfoColor = ConsoleColor.DarkCyan;
            MobileSuitInfoColor = ConsoleColor.DarkBlue;
            Console.InputEncoding = new UTF8Encoding();
            Console.OutputEncoding = new UTF8Encoding();
            Input = Console.In;
            Output = Console.Out;
            Error = Console.Error;
        }

        public bool IsOutputRedirected => !Console.Out.Equals(Output);

        public bool IsInputRedirected => !Console.In.Equals(Input);
        public bool IsErrorRedirected => !Console.Error.Equals(Error);

        public ConsoleColor DefaultColor { get; set; }
        public ConsoleColor PromptColor { get; set; }
        public ConsoleColor ErrorColor { get; set; }
        public ConsoleColor AllOkColor { get; set; }
        public ConsoleColor ListTitleColor { get; set; }
        public ConsoleColor CustomInfoColor { get; set; }
        public ConsoleColor MobileSuitInfoColor { get; set; }
        public TextWriter Error { get; set; } 
        public TextWriter Output { get; set; } 
        public TextReader Input { get; set; } 
        public string ConsoleTitle
        {
            get => Console.Title;
            set => Console.Title = value;
        }
        public enum OutputType
        {
            Default = 0,
            Prompt = 1,
            Error = 2,
            AllOk = 3,
            ListTitle = 4,
            CustomInfo = 5,
            MobileSuitInfo = 6
        }

        private void ProgressBarUpdate(object sender, EventArgs e)
        {
            Console.SetCursorPosition(0, Console.CursorTop - ProgressBars.Count + 1);
            foreach (var progressBar in ProgressBars)
            {
                if (!progressBar.Equals(sender))
                {
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
                else
                {
                    var thisProgressBar = sender as TextProgressBar;
                    Console.WriteLine(thisProgressBar?.Progress);
                    if (!(thisProgressBar is null) && thisProgressBar.CurrentProgress == thisProgressBar.MaxProgress)
                        ProgressBars.Remove(thisProgressBar);
                }
            }
        }

        private string Clear { get; } = new string(' ', Console.BufferWidth);
        private List<TextProgressBar> ProgressBars{ get; set; } = new List<TextProgressBar>();
        internal class TextProgressBar
        {
            public delegate void ProgressChangedEventHandler(object sender, EventArgs e);

            public event ProgressChangedEventHandler ProgressChanged;
            public string Label { get; }
            public int MaxProgress { get; }
            private int _currentProgress = 0;
            public string Progress => new string(ProgressArray);
            private char[] ProgressArray { get; }
            private int ProgressLength { get; }
            private int ProgressPrintBoarder { get; }
            private int PrefixLength { get; }
            public TextProgressBar(int maxProgress, int textBufferSize, ProgressChangedEventHandler progressChangedEventHandler, string label = "")
            {
                MaxProgress = maxProgress;
                Label = label;
                ProgressChanged += progressChangedEventHandler;
                var suffix = $" {CurrentProgress.ToString().PadLeft(MaxProgress.ToString().Length)}/{MaxProgress}";
                var prefix = label == "" ? "" : $"{label} ";
                PrefixLength = prefix.Length;
                ProgressLength = textBufferSize - 2 - PrefixLength - suffix.Length;
                var builder = new StringBuilder();
                builder.Append(prefix);
                builder.Append('[');
                builder.Append(' ', ProgressLength);
                builder.Append(']');
                ProgressPrintBoarder = builder.Length;
                builder.Append(suffix);
                ProgressArray = builder.ToString().ToCharArray();

            }
            public int CurrentProgress
            {
                get => _currentProgress > MaxProgress ? MaxProgress : _currentProgress;
                private set => _currentProgress = value;
            }

            public void AppendProgress()
            {
                var currentPrintStart = PrefixLength + CurrentProgress * ProgressLength / MaxProgress + 1;
                CurrentProgress++;
                var newCurrentNumber = CurrentProgress.ToString().PadLeft(MaxProgress.ToString().Length);
                var newCurrentPrintStart = ProgressPrintBoarder + 1;
                var currentPrintEnd = PrefixLength + CurrentProgress * ProgressLength / MaxProgress + 1;
                for (; currentPrintStart < currentPrintEnd && currentPrintStart < ProgressPrintBoarder; currentPrintStart++)
                {
                    ProgressArray[currentPrintStart] = '=';
                }
                if (CurrentProgress != MaxProgress) ProgressArray[currentPrintStart] = '>';
                foreach (var i in newCurrentNumber)
                {
                    ProgressArray[newCurrentPrintStart++] = i;
                }

                ProgressChanged?.Invoke(this, new EventArgs());
            }
            public static TextProgressBar operator ++(TextProgressBar bar)
            {
                bar.AppendProgress();
                return bar;
            }
        }
    }
}