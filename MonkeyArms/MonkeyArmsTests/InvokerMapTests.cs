using NUnit.Framework;
using System;
using MonkeyArms;

namespace MonkeyArmsTests
{
	[TestFixture ()]
	public class InvokerMapTests
	{
		InvokerMap TestMap;

		Invoker TestInvoker;

		bool InvokerHandled;

		[Test (Description = "Add should map a handler to an invoker")]
		public void Verify_Add ()
		{
			TestInvoker.Invoke ();
			Assert.True (InvokerHandled);
		}

		[Test(Description = "RemoveAll should remove all mappings from map")]
		public void Verify_RemoveAll()
		{
			TestMap.RemoveAll ();
			TestInvoker.Invoke ();
			Assert.False (InvokerHandled);
		}

		[Test]
		public void Verify_Remove()
		{
			TestMap.Add (TestInvoker, TestHandler);
			Assert.AreEqual (TestMap.GetInvokerHandlerCount (TestInvoker), 2);
			TestMap.Remove (TestInvoker, TestHandler);
			Assert.AreEqual (TestMap.GetInvokerHandlerCount (TestInvoker), 1);
		}

		[Test]
		public void Verify_Handler_Count()
		{
			Assert.AreEqual (TestMap.GetInvokerHandlerCount (TestInvoker), 1);
			TestMap.Add (TestInvoker, (sender, e) => Console.WriteLine("Another handler"));
			Assert.AreEqual (TestMap.GetInvokerHandlerCount (TestInvoker), 2);
		}

		[Test]
		public void Verify_IsInvokerMapped()
		{
			Assert.True (TestMap.IsInvokerMapped (TestInvoker));
			Assert.False(TestMap.IsInvokerMapped(new Invoker()));
		}

		[Test]
		public void Verify_InvokerHasHandler()
		{
			Assert.False (TestMap.InvokerHasHandler(TestInvoker, TestHandler));
			TestMap.Add (TestInvoker, TestHandler);
			Assert.True (TestMap.InvokerHasHandler (TestInvoker, TestHandler));
		}

		void TestHandler(object sender, EventArgs args)
		{

		}

		[SetUp]
		public void Init()
		{
			TestMap = new InvokerMap ();
			TestInvoker = new Invoker ();
			InvokerHandled = false;
			TestMap.Add (TestInvoker, (sender, e) => InvokerHandled = true);
		}
	}
}

