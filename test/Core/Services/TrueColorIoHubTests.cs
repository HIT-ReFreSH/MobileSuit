﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using HitRefresh.MobileSuit.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitRefresh.MobileSuit.Core.Services.Tests
{
	[TestClass]
	public class TrueColorIoHubTests
	{
		protected virtual TrueColorIOHub getInstance(IOOptions options = IOOptions.None)
		{
			return new TrueColorIOHub(PromptFormatters.BasicPromptFormatter, (IIOHub hub) => { hub.Options = options; });
		}
		[TestMethod]
		public void IOHubTest()
		{
			Assert.IsNotNull(getInstance());
		}

		[TestMethod]
		public void ResetInputTest()
		{
			TrueColorIOHub hub = getInstance();
			hub.Input = TextReader.Null;
			hub.ResetInput();
			Assert.AreEqual(Console.In, hub.Input);
		}

		[TestMethod]
		public void ReadLineTest()
		{
			TrueColorIOHub hub = getInstance();
			StringReader reader = new StringReader("123\nabc\n\n");
			hub.Input = reader;
			Assert.AreEqual("123", hub.ReadLine());
			Assert.AreEqual("abc", hub.ReadLine());
			Assert.AreEqual("", hub.ReadLine());
			Assert.AreEqual(null, hub.ReadLine());
			Assert.AreEqual(null, hub.ReadLine());
		}

		[TestMethod]
		public void ReadLineAsyncTest()
		{
			TrueColorIOHub hub = getInstance();
			StringReader reader = new StringReader("123\nabc\n\n");
			hub.Input = reader;
			Assert.AreEqual("123", hub.ReadLineAsync().Result);
			Assert.AreEqual("abc", hub.ReadLineAsync().Result);
			Assert.AreEqual("", hub.ReadLineAsync().Result);
			Assert.AreEqual(null, hub.ReadLineAsync().Result);
			Assert.AreEqual(null, hub.ReadLineAsync().Result);
		}

		[TestMethod]
		public void PeekTest()
		{
			TrueColorIOHub hub = getInstance();
			StringReader reader = new StringReader("123\nabc\n\n");
			hub.Input = reader;
			Assert.AreEqual('1', hub.Peek());
			Assert.AreEqual('1', hub.Peek());
		}

		[TestMethod]
		public void ReadTest()
		{
			TrueColorIOHub hub = getInstance();
			StringReader reader = new StringReader("123\nabc\n\n");
			hub.Input = reader;
			Assert.AreEqual('1', hub.Read());
			Assert.AreEqual('2', hub.Read());
		}

		[TestMethod]
		public void ReadToEndTest()
		{
			TrueColorIOHub hub = getInstance();
			StringReader reader = new StringReader("123\nabc\n\n");
			hub.Input = reader;
			Assert.AreEqual("123\nabc\n\n", hub.ReadToEnd());
		}

		[TestMethod]
		public void ReadToEndAsyncTest()
		{
			TrueColorIOHub hub = getInstance();
			StringReader reader = new StringReader("123\nabc\n\n");
			hub.Input = reader;
			Assert.AreEqual("123\nabc\n\n", hub.ReadToEndAsync().Result);
		}

		[TestMethod]
		public void ResetErrorTest()
		{
			TrueColorIOHub hub = getInstance();
			hub.ErrorStream = TextWriter.Null;
			hub.ResetError();
			Assert.AreEqual(Console.Error, hub.ErrorStream);
		}

		[TestMethod]
		public void ResetOutputTest()
		{
			TrueColorIOHub hub = getInstance();
			hub.Output = TextWriter.Null;
			hub.ResetOutput();
			Assert.AreEqual(Console.Out, hub.Output);
		}

		[TestMethod]
		public void AppendWriteLinePrefixTest()
		{
			TrueColorIOHub hub = getInstance();
			PrintUnit pu = new PrintUnit { Text = "\t\t\t" };
			hub.AppendWriteLinePrefix(pu);
			CollectionAssert.AreEqual(new List<PrintUnit> { pu }, new List<PrintUnit>(hub.GetLinePrefix(OutputType.Default)));
		}
		[TestMethod]
		public void GetLinePrefixTest1()
		{
			TrueColorIOHub hub = getInstance(IOOptions.DisableLinePrefix | IOOptions.DisableTag);
			PrintUnit pu = new PrintUnit { Text = "\t\t\t" };
			hub.AppendWriteLinePrefix(pu);
			CollectionAssert.AreEqual(new List<PrintUnit> { }, 
				new List<PrintUnit>(hub.GetLinePrefix(OutputType.Default)));
		}
		[TestMethod]
		public void GetLinePrefixTest2()
		{
			TrueColorIOHub hub = getInstance(IOOptions.DisableLinePrefix);
			PrintUnit pu = new PrintUnit { Text = "\t\t\t" };
			hub.AppendWriteLinePrefix(pu);
			CollectionAssert.AreEqual(new List<PrintUnit> { ("[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "]", null) },
				new List<PrintUnit>(hub.GetLinePrefix(OutputType.Default)));
		}
		[TestMethod]
		public void GetLinePrefixTest3()
		{
			TrueColorIOHub hub = getInstance(IOOptions.DisableLinePrefix);
			PrintUnit pu = new PrintUnit { Text = "\t\t\t" };
			hub.AppendWriteLinePrefix(pu);
			CollectionAssert.AreEqual(new List<PrintUnit> { ("[" + DateTime.Now.ToString(CultureInfo.InvariantCulture) + "][Error]", null) },
				new List<PrintUnit>(hub.GetLinePrefix(OutputType.Error)));
		}
		[TestMethod]
		public void SubtractWriteLinePrefixTest()
		{
			TrueColorIOHub hub = getInstance();
			PrintUnit pu1 = new PrintUnit { Text = "\t\t\t" };
			PrintUnit pu2 = new PrintUnit { Text = "123" };
			hub.AppendWriteLinePrefix(pu1);
			hub.AppendWriteLinePrefix(pu2);
			hub.SubtractWriteLinePrefix();
			CollectionAssert.AreEqual(new List<PrintUnit> { pu1 }, new List<PrintUnit>(hub.GetLinePrefix(OutputType.Default)));
		}

		[TestMethod]
		public void ClearWriteLinePrefixTest()
		{
			TrueColorIOHub hub = getInstance();
			PrintUnit pu1 = new PrintUnit { Text = "\t\t\t" };
			PrintUnit pu2 = new PrintUnit { Text = "123" };
			hub.AppendWriteLinePrefix(pu1);
			hub.AppendWriteLinePrefix(pu2);
			hub.ClearWriteLinePrefix();
			CollectionAssert.AreEqual(new List<PrintUnit> { }, new List<PrintUnit>(hub.GetLinePrefix(OutputType.Default)));
		}

		[TestMethod]
		public virtual void WriteTest()
		{
			TrueColorIOHub hub = getInstance();
			StringWriter writer = new StringWriter();
			hub.Output = writer;
			PrintUnit pu = new PrintUnit { Text = "test write", Foreground = Color.FromArgb(0, 1, 2), Background = Color.FromArgb(3, 4, 5) };
			hub.Write(pu);
			Assert.AreEqual("\u001b[38;2;0;1;2m\u001b[48;2;3;4;5mtest write\u001b[0m", writer.ToString());
		}

		[TestMethod]
		public virtual void WriteAsyncTest()
		{
			TrueColorIOHub hub = getInstance();
			StringWriter writer = new StringWriter();
			hub.Output = writer;
			PrintUnit pu = new PrintUnit { Text = "test write", Foreground = Color.FromArgb(0, 1, 2), Background = Color.FromArgb(3, 4, 5) };
			hub.WriteAsync(pu).Wait();
			Assert.AreEqual("\u001b[38;2;0;1;2m\u001b[48;2;3;4;5mtest write\u001b[0m", writer.ToString());
		}
	}

	[TestClass]
	public class PureTextIoHubTests : TrueColorIoHubTests
	{
		protected override PureTextIOHub getInstance(IOOptions options = IOOptions.None)
		{
			return new PureTextIOHub(PromptFormatters.BasicPromptFormatter, (IIOHub hub) => { hub.Options = options; });
		}
		[TestMethod]
		public override void WriteTest()
		{
			TrueColorIOHub hub = getInstance();
			StringWriter writer = new StringWriter();
			hub.Output = writer;
			PrintUnit pu = new PrintUnit { Text = "test write", Foreground = Color.FromArgb(0, 1, 2), Background = Color.FromArgb(3, 4, 5) };
			hub.Write(pu);
			Assert.AreEqual("test write", writer.ToString());
		}
		[TestMethod]
		public override void WriteAsyncTest()
		{
			TrueColorIOHub hub = getInstance();
			StringWriter writer = new StringWriter();
			hub.Output = writer;
			PrintUnit pu = new PrintUnit { Text = "test write", Foreground = Color.FromArgb(0, 1, 2), Background = Color.FromArgb(3, 4, 5) };
			hub.WriteAsync(pu).Wait();
			Assert.AreEqual("test write", writer.ToString());
		}
	}
	[TestClass]
	public class FourBitIoHubTests : TrueColorIoHubTests
	{
		protected override FourBitIOHub getInstance(IOOptions options = IOOptions.None)
		{
			return new FourBitIOHub(PromptFormatters.BasicPromptFormatter, (IIOHub hub) => { hub.Options = options; });
		}
		[TestMethod]
		public override void WriteTest()
		{
			TrueColorIOHub hub = getInstance();
			StringWriter writer = new StringWriter();
			hub.Output = writer;
			PrintUnit pu = new PrintUnit { Text = "test write", Foreground = Color.FromArgb(255, 0, 0), Background = Color.FromArgb(0, 255, 0) };
			hub.Write(pu);
			Assert.AreEqual($"\u001b[91m\u001b[102mtest write\u001b[0m", writer.ToString());
		}
		[TestMethod]
		public override void WriteAsyncTest()
		{
			TrueColorIOHub hub = getInstance();
			StringWriter writer = new StringWriter();
			hub.Output = writer;
			PrintUnit pu = new PrintUnit { Text = "test write", Foreground = Color.FromArgb(255, 0, 0), Background = Color.FromArgb(0, 255, 0) };
			hub.WriteAsync(pu).Wait();
			Assert.AreEqual($"\u001b[91m\u001b[102mtest write\u001b[0m", writer.ToString());
		}
	}
}