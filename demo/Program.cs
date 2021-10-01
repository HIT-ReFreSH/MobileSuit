using PlasticMetal.MobileSuit;

namespace PlasticMetal.MobileSuitDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*Suit.CreateBuilder(args)
                .HasName("demo")
                .UsePowerLine()
                .Use4BitColorIO()
                .MapClient<Client>()
                .Build()
                .Run();*/
            Suit.QuickStart4BitPowerLine<Client>(args);
        }
    }
}