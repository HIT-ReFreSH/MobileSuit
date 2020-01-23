using System;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit
{
    [MsInfo("Test")]
    public class MsTest : MsClient
    {
        public MsTest()
        {
            Text = "MsTest";
        }
        public TestC Tc = new TestC();
        public TestC TestCc { get; set; } = new TestC();

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