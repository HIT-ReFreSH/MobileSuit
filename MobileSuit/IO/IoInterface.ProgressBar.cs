using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.IO.Controls;

namespace PlasticMetal.MobileSuit.IO
{
    partial class IoInterface
    {
        private void ClearProgressBars()
        {
            if (!ProgressBars.Any()) return;
            if (Console.CursorLeft != 0)
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            for (var i = 0; i < ProgressBars.Count; i++)
            {
                Output.WriteLine(Clear);
            }
            Console.SetCursorPosition(LastCursorLocation.Item1, LastCursorLocation.Item2);

        }
        private void PrintProgressBars()
        {
            if (!ProgressBars.Any()) return;
            if (Console.CursorLeft != 0)
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            foreach (var progressBar in ProgressBars)
            {
                Console.WriteLine(progressBar.Progress);
            }
            Console.SetCursorPosition(LastCursorLocation.Item1, LastCursorLocation.Item2);
        }
        private async Task ClearProgressBarsAsync()
        {
            if (!ProgressBars.Any()) return;
            if (Console.CursorLeft != 0)
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            for (var i = 0; i < ProgressBars.Count; i++)
            {
                await Output.WriteLineAsync(Clear);
            }
            Console.SetCursorPosition(LastCursorLocation.Item1, LastCursorLocation.Item2);

        }
        private async Task PrintProgressBarsAsync()
        {
            if (!ProgressBars.Any()) return;
            if (Console.CursorLeft != 0)
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            foreach (var progressBar in ProgressBars)
            {
                await Output.WriteLineAsync(progressBar.Progress);
            }
            Console.SetCursorPosition(LastCursorLocation.Item1, LastCursorLocation.Item2);
        }
        private async void ProgressBarUpdate(object sender, EventArgs e)
        {
            if (IsOutputRedirected) return;
            if (Console.CursorLeft != 0)
                Console.SetCursorPosition(0, Console.CursorTop + 1);
            foreach (var progressBar in ProgressBars)
            {
                if (!progressBar.Equals(sender))
                {
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
                else
                {
                    var thisProgressBar = sender as ProgressBar;
                    await Output.WriteLineAsync(thisProgressBar?.Progress);
                    if (!(thisProgressBar is null) && thisProgressBar.CurrentProgress == thisProgressBar.MaxProgress)
                        ProgressBars.Remove(thisProgressBar);
                }
            }
            Console.SetCursorPosition(LastCursorLocation.Item1, LastCursorLocation.Item2);
        }
        private string Clear { get; } = new string(' ', Console.BufferWidth);
        private List<ProgressBar> ProgressBars { get;} = new List<ProgressBar>();

        public ProgressBar AddProgressBar(int maxProgress,string label="")
        {
            var progressBar = new ProgressBar(maxProgress, Console.BufferWidth, label);
            AddProgressBar(progressBar);
            return progressBar;
        }
        public void AddProgressBar(ProgressBar progressBar)
        {
            progressBar.ProgressChanged += ProgressBarUpdate;
            ProgressBars.Add(progressBar);

        }

        public void ClearProgressBar()
        {
            foreach (var progressBar in ProgressBars)
            {
                progressBar.ProgressChanged -= ProgressBarUpdate;
            }

            ProgressBars.Clear();
        }
        public void RemoveProgressBar(ProgressBar progressBar)
        {
            if (!ProgressBars.Contains(progressBar)) return;
            progressBar.ProgressChanged -= ProgressBarUpdate;
            ProgressBars.Remove(progressBar);
        }
    }
}
