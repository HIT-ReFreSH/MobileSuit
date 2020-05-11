using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.Future;

namespace PlasticMetal.MobileSuitDemo
{
    internal class Program
    {
        private static void Main()
        {
            var ms = new SuitHost(new Client(), PowerLineThemedPromptServer.CreatePowerLineThemeConfiguration())
                {UseTraceBack = true};
            ms.Run();
        }
    }
}