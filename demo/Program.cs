using Microsoft.Extensions.Hosting;
using PlasticMetal.MobileSuit;

namespace PlasticMetal.MobileSuitDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Suit.GetBuilder(args)
                .UsePowerLine()
                .AddObject<Client>()
                .Build()
                .Run();
        }
    }
}