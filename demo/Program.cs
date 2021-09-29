using Microsoft.Extensions.Hosting;
using PlasticMetal.MobileSuit;

namespace PlasticMetal.MobileSuitDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Suit.CreateBuilder(args)
                .UsePowerLine()
                .MapClient<Client>()
                .Build()
                .Run();
        }
    }
}