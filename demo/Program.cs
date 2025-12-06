using System;
using System.Threading.Tasks;
using HitRefresh.MobileSuit;

namespace HitRefresh.MobileSuitDemo
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("=== MOBILESUIT COMMAND TEST ===");
            Console.WriteLine("If you see this, MobileSuit started but commands may not be registered.");

            // 创建一个非常简单的构建器，只添加必要的服务
            var builder = Suit.CreateBuilder(args)
                .HasName("test")
                .MapClient<Client>();

            var host = builder.Build();

            // 添加一个事件来检查命令
            Console.WriteLine("Starting MobileSuit host...");

            await host.StartAsync();
        }
    }
}