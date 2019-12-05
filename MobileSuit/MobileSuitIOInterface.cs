#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
namespace MobileSuit
{
    public class MobileSuitIoInterface
    {
        public void Write(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!RedirectOutput)
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

                Console.Write(content);

                Console.ForegroundColor = dColor;


            }
            else
            {
                if (type != OutputType.Prompt)
                    Output?.Write(content);
            }
        }
        public void WriteLine(string content, OutputType type = OutputType.Default, ConsoleColor? customColor = null)
        {
            if (!RedirectOutput)
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

        public void AppendWriteLinePrefix(string prefix="\t")
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
        {
            return Input != null ? Input?.ReadLine() : Console.ReadLine();
        }

        public MobileSuitIoInterface()
        {
            DefaultColor = ConsoleColor.White;
            ErrorColor = ConsoleColor.Red;
            PromptColor = ConsoleColor.Magenta;
            AllOkColor = ConsoleColor.Green;
            ListTitleColor = ConsoleColor.Yellow;
            CustomInfoColor = ConsoleColor.DarkCyan;
            MobileSuitInfoColor = ConsoleColor.DarkBlue;
        }
        public bool RedirectOutput => Output != null;
        public bool RedirectInput => Input != null;
        //public bool RedirectError => Error != null;

        public ConsoleColor DefaultColor { get; set; }
        public ConsoleColor PromptColor { get; set; }
        public ConsoleColor ErrorColor { get; set; }
        public ConsoleColor AllOkColor { get; set; }
        public ConsoleColor ListTitleColor { get; set; }
        public ConsoleColor CustomInfoColor { get; set; }
        public ConsoleColor MobileSuitInfoColor { get; set; }
        //public StreamWriter? Error { get; set; }
        public StreamWriter? Output { get; set; }
        public StreamReader? Input { get; set; }
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
    }
}
