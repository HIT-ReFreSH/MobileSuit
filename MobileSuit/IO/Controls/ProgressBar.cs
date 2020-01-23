using System;
using System.Text;

namespace PlasticMetal.MobileSuit.IO.Controls
{
    public class ProgressBar
    {
        public delegate void ProgressChangedEventHandler(object sender, EventArgs e);

        private int _currentProgress;

        public ProgressBar(int maxProgress, int textBufferSize, string label = "")
        {
            MaxProgress = maxProgress;
            Label = label;
            //ProgressChanged += progressChangedEventHandler;
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

        public string Label { get; }
        public int MaxProgress { get; }
        public string Progress => new string(ProgressArray);
        private char[] ProgressArray { get; }
        private int ProgressLength { get; }
        private int ProgressPrintBoarder { get; }
        private int PrefixLength { get; }

        public int CurrentProgress
        {
            get => _currentProgress > MaxProgress ? MaxProgress : _currentProgress;
            private set => _currentProgress = value;
        }

        //public delegate void AppendProgressHandler();
        public event ProgressChangedEventHandler ProgressChanged;

        public void AppendProgress()
        {
            var currentPrintStart = PrefixLength + CurrentProgress * ProgressLength / MaxProgress + 1;
            CurrentProgress++;
            var newCurrentNumber = CurrentProgress.ToString().PadLeft(MaxProgress.ToString().Length);
            var newCurrentPrintStart = ProgressPrintBoarder + 1;
            var currentPrintEnd = PrefixLength + CurrentProgress * ProgressLength / MaxProgress + 1;
            for (; currentPrintStart < currentPrintEnd && currentPrintStart < ProgressPrintBoarder; currentPrintStart++)
                ProgressArray[currentPrintStart] = '=';
            if (CurrentProgress != MaxProgress) ProgressArray[currentPrintStart] = '>';
            foreach (var i in newCurrentNumber) ProgressArray[newCurrentPrintStart++] = i;

            ProgressChanged?.Invoke(this, new EventArgs());
        }

        public static ProgressBar operator ++(ProgressBar bar)
        {
            bar.AppendProgress();
            return bar;
        }
    }
}