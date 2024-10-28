using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using HitRefresh.MobileSuit;

namespace MobileSuit.Tests
{
    [TestClass]
    public class SuitTests
    {
        // ģ���IOServer���ڲ�����IIOHub��صķ���
        private class MockIOServer : IIOHub
        {
            public ColorSetting ColorSetting { get; set; } = new ColorSetting();

            public void WriteLine(string value)
            {
                // ����������ʵ�ʵ������֤�߼�����ʱֻ��ģ�ⷽ��ʵ��
                Console.WriteLine(value);
            }

            public void WriteLine(IEnumerable<PrintUnit> contentArray)
            {
                // ����������ʵ�ʵ������֤�߼�����ʱֻ��ģ�ⷽ��ʵ��
                foreach (var unit in contentArray)
                {
                    Console.WriteLine(unit.Content);
                }
            }
        }

        [TestMethod]
        public async Task QuickStart4BitPowerLine_Test()
        {
            // ׼����������
            string[] args = { "testArg1", "testArg2" };
            var clientType = typeof(SomeClientClass); // ����������һ������Ŀͻ�����SomeClientClass���ڲ���

            // ִ��Ҫ���Եķ���
            await Suit.QuickStart4BitPowerLine<SomeClientClass>(args);

            // ���������Ӹ������֤�߼���������֤�Ƿ���ȷ���������������е�
            // ������ʵ�ʵĴ��������й��̿����漰���ܶ��ⲿ�����͸����߼���
            // ������Ҫ��һ��ģ���ʹ�ü��ɲ�������ȫ�����֤
        }

        [TestMethod]
        public void QuickStart_Test()
        {
            // ׼����������
            string[] args = { "testArg1", "testArg2" };
            var clientType = typeof(SomeClientClass);

            // ִ��Ҫ���Եķ���
            Suit.QuickStart<SomeClientClass>(args);

            // ͬ�������������Ӹ�����֤�߼����������Ƿ���ȷ���ú���������������
        }

        [TestMethod]
        public void QuickStartPowerLine_Test()
        {
            // ׼����������
            string[] args = { "testArg1", "testArg2" };
            var clientType = typeof(SomeClientClass);

            // ִ��Ҫ���Եķ���
            Suit.QuickStartPowerLine<SomeClientClass>(args);

            // ����Ӻ�����֤�߼�������ȷ����PowerLine��ص������Ƿ���ȷӦ�õ�
        }

        [TestMethod]
        public void CreateBuilder_Test()
        {
            // ׼����������
            string[] args = { "testArg1", "testArg2" };

            // ִ��Ҫ���Եķ���
            var builder = Suit.CreateBuilder(args);

            // ��֤���ص�builder�Ƿ�ΪԤ�ڵ�����
            Assert.IsInstanceOfType(builder, typeof(SuitHostBuilder));
        }

        [TestMethod]
        public void CreateContentArray_Tests()
        {
            // ���Բ�ͬ������ʽ��CreateContentArray����

            // ����(string, Color?)��ʽ�Ĳ���
            var contentArray1 = Suit.CreateContentArray(("content1", Color.Red), ("content2", null));
            Assert.IsNotNull(contentArray1);

            // ����(string, Color?, Color?)��ʽ�Ĳ���
            var contentArray2 = Suit.CreateContentArray(("content3", Color.Blue, Color.Green), ("content4", null, null));
            Assert.IsNotNull(contentArray2);

            // ����(string, ConsoleColor?)��ʽ�Ĳ���
            var contentArray3 = Suit.CreateContentArray(("content5", ConsoleColor.Yellow), ("content6", null));
            Assert.IsNotNull(contentArray3);

            // ����(string, ConsoleColor?, ConsoleColor?)��ʽ�Ĳ���
            var contentArray4 = Suit.CreateContentArray(("content7", ConsoleColor.Magenta, ConsoleColor.Cyan), ("content8", null, null));
            Assert.IsNotNull(contentArray4);
        }

        [TestMethod]
        public void PrintAssemblyInformation_Test()
        {
            // ׼����������
            var io = new MockIOServer();
            string assemblyName = "TestAssembly";
            Version assemblyVersion = new Version("1.0.0");
            bool showMobileSuitPowered = true;
            string owner = "TestOwner";
            string site = "https://test.site";
            bool showLsHelp = true;

            // ִ��Ҫ���Եķ���
            Suit.PrintAssemblyInformation(io, assemblyName, assemblyVersion, showMobileSuitPowered, owner, site, showLsHelp);

            // ������������֤�߼���������io.WriteLine�Ƿ���ȷ���ã���������Ƿ����Ԥ�ڵ�
            // ����ʵ�ʵ���֤�����漰����������ݵ���ϸ������������Ҫ��һ�����ƴ˴�����֤�߼�
        }

        [TestMethod]
        public void PrintMobileSuitInformation_Test()
        {
            // ׼����������
            var io = new MockIOServer();

            // ִ��Ҫ���Եķ���
            Suit.PrintMobileSuitInformation(io);

            // ͬ�������������֤�߼������io.WriteLine�ĵ����������������Ƿ���ȷ
        }
    }
}
