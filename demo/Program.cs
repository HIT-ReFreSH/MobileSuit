using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.Logging;
using PlasticMetal.MobileSuit.ObjectModel.Future;

namespace PlasticMetal.MobileSuitDemo
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return Suit.GetBuilder()
                .UseLog(ISuitLogger.CreateFileByDirectory(@"D:\\"))
                .UsePrompt<PowerLineThemedPromptServer>()
                /*.UseBuildInCommand<DiagnosticBuildInCommandServer>()*/
                .Build<Client>()
                //.Build<AutoArgumentCliClient>()
                /*.Build<LoggerTest>()*/
                .Run(args);
        }
    }
}