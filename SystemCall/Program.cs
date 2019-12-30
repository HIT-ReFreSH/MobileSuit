using System;
using System.Diagnostics;

namespace SystemCall
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = "MobileSuitCLI.exe",
                    WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                }
            };
            p.OutputDataReceived += P_OutputDataReceived;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            var r = Console.ReadLine();
            while (true)
            {
                p.StandardInput.WriteLine(r);
                r = Console.ReadLine();
            }

            p.Dispose();
        }

        private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
