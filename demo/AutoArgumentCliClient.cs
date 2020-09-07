using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlasticMetal.MobileSuitDemo
{
    class IOSet : AutoDynamicParameter
    {
        [Option("i")]
        public string Input { set; get; }
        [Option("o")]
        public string Output { set; get; }
    }
    class AutoArgumentCliClient : CommandLineApplication<IOSet>
    {
        public override void SuitShowUsage()
        {
            IO.WriteLine("");
        }

        public override int SuitStartUp(IOSet arg)
        {
            IO.WriteLine(arg.Input);
            return 0;
        }
    }
}
