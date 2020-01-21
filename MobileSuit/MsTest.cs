using System;
using System.Collections.Generic;
using System.Text;

using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;
using PlasticMetal.MobileSuit.ObjectModel.Interfaces;

namespace PlasticMetal.MobileSuit
{
    [MsInfo("Test")]
    public class MsTest : MsClient
    {
        public TestC TestCc { get; set; } = new TestC();
        public TestC Tc = new TestC();
        [MsAlias("slnm")]
        [MsAlias("nmsl")]
        public void Test()
        {
            Io.WriteLine("Test!!!!");
        }
        [MsInfo("TestC")]
        public class TestC
        {
            public void T()
            {
                Console.WriteLine("t");
            }
        }

    }
}
