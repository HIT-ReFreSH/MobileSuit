namespace MobileSuitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var ms = new PlasticMetal.MobileSuit.SuitHost(new Client());
            ms.UseTraceBack = false;
            ms.Run();
        }
    }
}
