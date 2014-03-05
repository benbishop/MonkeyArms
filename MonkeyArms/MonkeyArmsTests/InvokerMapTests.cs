using MonkeyArms;
using NUnit.Framework;
using System;
using Should;

namespace MonkeyArmsTests
{
	[TestFixture]
	public class InvokerMapTests
	{
		protected InvokerMap TestMap;
		protected Invoker TestInvoker;
		protected bool InvokerHandled;

		[Test (Description = "Add should map a handler to an invoker")]
		public void Verify_Add ()
		{
			TestInvoker.Invoke ();
			InvokerHandled.ShouldBeTrue ();
		}

		[Test (Description = "RemoveAll should remove all mappings from map")]
		public void Verify_RemoveAll ()
		{
			TestMap.RemoveAll ();
			TestInvoker.Invoke ();
			InvokerHandled.ShouldBeFalse ();
		}

		[Test (Description = "Remove should remove the passed handler from passed invoker")]
		public void Verify_Remove ()
		{
			TestMap.Add (TestInvoker, TestHandler);
			TestMap.GetInvokerHandlerCount (TestInvoker).ShouldEqual (2);
			TestMap.Remove (TestInvoker, TestHandler);
			TestMap.GetInvokerHandlerCount (TestInvoker).ShouldEqual (1);
		}

		[Test (Description = "GetInvokerHandlerCount should return the correct number of handlers added to invoker.")]
		public void Verify_Handler_Count ()
		{
			TestMap.GetInvokerHandlerCount (TestInvoker).ShouldEqual (1);
			TestMap.Add (TestInvoker, (sender, e) => Console.WriteLine ("Another handler"));
			TestMap.GetInvokerHandlerCount (TestInvoker).ShouldEqual (2);
		}

		[Test (Description = "IsInvokerMapped should return a boolean on whether or not the invoker is in the map.")]
		public void Verify_IsInvokerMapped ()
		{
			TestMap.IsInvokerMapped (TestInvoker).ShouldBeTrue ();
			TestMap.IsInvokerMapped (new Invoker ()).ShouldBeFalse ();
		}

		[Test (Description = "InvokerHasHandler should return a boolean on whether or not the invoker has a handler")]
		public void Verify_InvokerHasHandler ()
		{
			TestMap.InvokerHasHandler (TestInvoker, TestHandler).ShouldBeFalse ();
			TestMap.Add (TestInvoker, TestHandler);
			TestMap.InvokerHasHandler (TestInvoker, TestHandler).ShouldBeTrue ();
		}

		private void TestHandler (object sender, EventArgs args)
		{
		}

		[SetUp]
		public void Init ()
		{
			TestMap = new InvokerMap ();
			TestInvoker = new Invoker ();
			InvokerHandled = false;
			TestMap.Add (TestInvoker, (sender, e) => InvokerHandled = true);
		}
	}
}