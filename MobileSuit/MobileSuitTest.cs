using System;
using System.Collections.Generic;
using System.Text;

using PlasticMetal.MobileSuit;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit
{
    [MobileSuitInfo("Test")]
    public class MobileSuitTest : IInfoProvider
    {
        
        public string Prompt { get; set; } = "Test";
        public TestC TestCC { get; set; } = new TestC();
        public TestC tc = new TestC();
        [Alias("slnm")]
        [Alias("nmsl")]
        public void Test()
        {
            Console.WriteLine("Test!!!!");
        }
        [MobileSuitInfo("TestC")]
        public class TestC
        {
            public void T()
            {
                Console.WriteLine("t");
            }
        }
    }
}
