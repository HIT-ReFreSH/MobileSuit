using System;
using System.Reflection;
using System.Resources;
using PlasticMetal.MobileSuit.ObjectModel;
using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit
{
    /// <summary>
    /// A test class for basic Mobile Suit Test.
    /// </summary>
    [MsInfo("Test")]
    public class MsTest : MsClient
    {
        /// <summary>
        /// Initialize a MsTest class.
        /// </summary>
        public MsTest()
        {
            Text = "MsTest";
        }
        /// <summary>
        /// A test Field for MsTest class.
        /// </summary>
        [MsAlias("TscF")]
        public TestSubClass TscField = new TestSubClass();
        /// <summary>
        /// A test Property for MsTest class.
        /// </summary>
        [MsAlias("TscP")]
        public TestSubClass TscProperty { get; set; } = new TestSubClass();
        /// <summary>
        /// A test Method for MsTest class, with two aliases.
        /// </summary>
        [MsAlias("alias1")]
        [MsAlias("alias2")]
        public void Test()
        {
            Io.WriteLine("Test() function executed.");
        }
        /// <summary>
        /// A subclass for MsTest.
        /// </summary>
        [MsInfo("TestSubClass")]
        public class TestSubClass
        {
            /// <summary>
            /// Subclass' test method.
            /// </summary>
            public void T()
            {
                Console.WriteLine("TestSubClass.T() function executed.");
            }
        }
    }
}