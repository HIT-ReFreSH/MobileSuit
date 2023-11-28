using HitRefresh.MobileSuit;

namespace HitRefresh.MobileSuitDemo
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