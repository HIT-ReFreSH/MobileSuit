#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuit.IO
{
    public partial class IoInterface
    {
        public IoInterface()
        {
            DefaultColor = ConsoleColor.White;
            ErrorColor = ConsoleColor.Red;
            PromptColor = ConsoleColor.Magenta;
            AllOkColor = ConsoleColor.Green;
            ListTitleColor = ConsoleColor.Yellow;
            CustomInformationColor = ConsoleColor.DarkCyan;
            InformationColor = ConsoleColor.DarkBlue;
            //Console.InputEncoding = new UTF8Encoding();
            //Console.OutputEncoding = new UTF8Encoding();
            Input = Console.In;
            Output = Console.Out;
            Error = Console.Error;
        }
        public ConsoleColor DefaultColor { get; set; }
        public ConsoleColor PromptColor { get; set; }
        public ConsoleColor ErrorColor { get; set; }
        public ConsoleColor AllOkColor { get; set; }
        public ConsoleColor ListTitleColor { get; set; }
        public ConsoleColor CustomInformationColor { get; set; }
        public ConsoleColor InformationColor { get; set; }
        private (int, int) LastCursorLocation { get; set; }

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


    }
}