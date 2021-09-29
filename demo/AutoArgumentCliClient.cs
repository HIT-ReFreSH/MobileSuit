/*using System;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.Attributes;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.Parsing;

namespace PlasticMetal.MobileSuitDemo
{
    internal class IOSet : AutoDynamicParameter
    {
        [Option("i")] public string Input { set; get; }

        [Option("o")] public string Output { set; get; }
    }

    internal class AutoArgumentCliClient : CommandLineApplication<IOSet>, IStartingInteractive
    {
        public void OnInitialized()
        {
            IO.PrintAssemblyInformation("Demo", new Version("1.0.0"), true, "Ferdinand Sukhoi", "https://ms.ifers.xyz",
                false);
            //IO.PrintMobileSuitInformation();
        }

        public async Task At()
        {
            await IO.WriteLineAsync("Async Test");
        }

        public override void SuitShowUsage()
        {
            IO.WriteLine("Show Usage");
        }

        public override int SuitStartUp(IOSet arg)
        {
            IO.WriteLine(arg.Input);
            return 0;
        }
    }
}*/