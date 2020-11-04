using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using PlasticMetal.MobileSuit.Core;
using PlasticMetal.MobileSuit;
using System.Threading.Tasks;

namespace PlasticMetal.MobileSuitDemo
{
    class IOSet : AutoDynamicParameter
    {
        [Option("i")]
        public string Input { set; get; }
        [Option("o")]
        public string Output { set; get; }
    }
    class AutoArgumentCliClient : CommandLineApplication<IOSet>, IStartingInteractive
    {
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

        public void OnInitialized()
        {
            IO.PrintAssemblyInformation("Demo", new Version("1.0.0"),true, "Ferdinand Sukhoi", "https://ms.ifers.xyz", false);
            //IO.PrintMobileSuitInformation();
        }
    }
}
