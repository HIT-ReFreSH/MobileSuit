namespace PlasticMetal.MobileSuitDemo
{
    internal class Program
    {
        private static void Main()
        {
            var ms = new PlasticMetal.MobileSuit.SuitHost(new Client(),MobileSuit.Future.PowerLineThemePromptServer.CreatePowerLineThemeConfiguration()) {UseTraceBack = true};
            ms.Run();
        }
    }
}
