using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel.Future;
using PlasticMetal.MobileSuit.ObjectModel.Premium;

namespace PlasticMetal.MobileSuitDemo
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            return Suit.GetBuilder()
                .UseLog(ILogger.OfDirectory(@"D:\\"))
                .UsePrompt<PowerLineThemedPromptServer>()
                .UseBuildInCommand<DiagnosticBuildInCommandServer>()
                .Build<Client>().Run(args);

        }
    }
}