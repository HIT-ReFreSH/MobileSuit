using System;
using System.Threading;
using PlasticMetal.MobileSuit;

namespace SystemCall
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            for (;;)
            {
                //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
                //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
                var ms = new MsHost(new MsTest());
                ms.Run();
                var s = new[] {1};
                var a = s[1..];
                Console.WriteLine(a.Length);
                Console.Read();

                Console.Out.Flush();
            }
        }
    }
}