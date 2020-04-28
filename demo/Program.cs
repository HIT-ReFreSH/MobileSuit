namespace MobileSuitDemo
{
    internal class Program
    {
        private static void Main()
        {
            var ms = new PlasticMetal.MobileSuit.SuitHost(new Client()) {UseTraceBack = false};
            ms.Run();
        }
    }
}
