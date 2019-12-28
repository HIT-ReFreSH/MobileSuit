using System;
using System.Collections.Generic;
using System.Text;
using MobileSuit.ObjectModel;

namespace MobileSuit
{
    [MobileSuitInfo("Test")]
    public class MobileSuitTest : IInfoProvider
    {
        public string Prompt { get; set; } = "Test";
        public TestC TestCC { get; set; } = new TestC();
        public TestC tc = new TestC();
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
