using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.Logging;

namespace PlasticMetal.MobileSuitDemo
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return Suit.GetBuilder()
                //.UseLog(ISuitLogger.CreateFileByDirectory(@"D:\\"))
                .UsePowerLine()
                /*.UseBuildInCommand<DiagnosticBuildInCommandServer>()*/
                .Build<Client>()
                //.Build<AutoArgumentCliClient>()
                /*.Build<LoggerTest>()*/
                .Run(args);
        }
    }
}