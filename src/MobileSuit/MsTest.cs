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
        [MsAlias("TscF")]
        public TestSubClass TscField = new TestSubClass();
        [MsAlias("TscP")]
        public TestSubClass TscProperty { get; set; } = new TestSubClass();

        [MsAlias("alias1")]
        [MsAlias("alias2")]
        public void Test()
        {
            Io.WriteLine("Test() function executed.");
        }

        [MsInfo("TestSubClass")]
        public class TestSubClass
        {
            public void T()
            {
                Console.WriteLine("TestSubClass.T() function executed.");
            }
        }
    }
}