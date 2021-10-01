/*using System.Diagnostics;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuitDemo
{
    internal class LoggerTest 
    {
        public async Task Test1()
        {
            var t = new Stopwatch();
            t.Start();
            Task.Run(async () =>
            {
                for (var i = 0; i < 10000; i++) Log.LogInformation("aaa");
            });
            Task.Run(async () =>
            {
                for (var i = 0; i < 10000; i++) Log.LogInformation("bbb");
            });
            for (var i = 0; i < 10000; i++)
                //await Log.LogDebugAsync("ccc");
                Log.LogInformation("ccc");

            IO.WriteLine($"{t.ElapsedMilliseconds}ms");
        }

        public void Test2()
        {
        }
    }
}*/

