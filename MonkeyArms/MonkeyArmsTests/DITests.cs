using System;
using NUnit.Framework;
using MonkeyArms;

namespace MonkeyArmsTests
{
	[TestFixture()]
	public class DITests
	{


		[Test(Description="Assert DI does not return null for Get")]
		public void TestGetReturnsNotNull ()
		{
			Assert.NotNull (DI.Get<TestClass> ());
		}

		[Test(Description="Assert Get returns new instances of a class by default")]
		public void TestGetReturnsNewInstanceByDefault ()
		{
			Assert.AreNotEqual (DI.Get<TestClass> (), DI.Get<TestClass> ());
		}

		[Test(Description="Assert when a class is registered as a Singleton the same instance is returned")]
		public void TestGetReturnsSingletonCorrectly ()
		{
			DI.MapSingleton<TestClass> ();
			Assert.AreEqual (DI.Get<TestClass> (), DI.Get<TestClass> ());
		}

		[Test(Description="Assert when a class is registered as a Singleton and then unmapped as a Singleton a new instance is returned")]
		public void TestUnMapSingletonWorksCorrectly ()
		{
			DI.MapSingleton<TestClass> ();
			var origClass = DI.Get<TestClass> ();

			DI.UnMapSingleton<TestClass> ();
			var newClass = DI.Get<TestClass> ();

			Assert.AreNotEqual (origClass, newClass);
		}

		[Test(Description="Assert when a class is registered via interface Get by interface returns correct class")]
		public void TestRegisterInterface ()
		{
			DI.MapClassToInterface<ITestClass, TestClass> ();
			Assert.IsTrue (DI.Get<ITestClass> () is TestClass);
		}

		[Test(Description="Assert MapCommandToInvoker maps command to invoker and correctly executes command when invoker is invoked")]
		public void TestMapCommandToInvoker ()
		{
			DI.MapSingleton<TestPM> ();
			DI.MapCommandToInvoker<TestCommand1, TestInvoker> ();
			DI.Get<TestInvoker> ().Invoke ();
			Assert.True (DI.Get<TestPM> ().Executed);

		}

		[Test(Description="Assert MapCommandToInvoker maps command to invoker and correctly executes command when invoker is inline invoked")]
		public void TestMapCommandToInvokerInlineInvoke ()
		{
			DI.MapSingleton<TestPM> ();
			DI.MapCommandToInvoker<TestCommand1, TestInvoker> ().Invoke();
			Assert.True (DI.Get<TestPM> ().Executed);

		}

		[Test(Description="Assert MapInstanceToSingleton returns instance of TestPM passed to it")]
		public void TestMapInstanceToSingleTon()
		{
			DI.MapSingleton<TestPM> ();
			var pm = new TestPM ();
			DI.MapInstanceToSingleton<TestPM> (pm);
			Assert.AreEqual (pm, DI.Get<TestPM> ());
		}
	
		[Test(Description="Assert MapInstanceToSingleton does not return previous instance of TestPM passed to it")]
		public void TestMapInstanceToSingleTonDoesNotReturnPreviousSignleton()
		{
			DI.MapSingleton<TestPM> ();
			var origPM = DI.Get<TestPM> ();
			DI.MapInstanceToSingleton<TestPM> (new TestPM());
			Assert.AreNotEqual (origPM, DI.Get<TestPM> ());
		}

		[Test(Description="Assert MapInstanceToSingleton does not return previous instance of TestPM passed to it after UnMapInstanceFromSingleton is invoked")]
		public void TestMapInstanceToSingleTonDoesNotReturnPreviousSignletonAferUnMapInstanceToSingletonIsInvoiked()
		{

			var origPM = new TestPM ();
			DI.MapInstanceToSingleton<TestPM> (new TestPM());
			DI.UnMapInstanceFromSingleton<TestPM> ();
			Assert.AreNotEqual (origPM, DI.Get<TestPM> ());
		}

	}
	/*
		 * Test Classes
		 * 
		*/

	public class TestInvoker:Invoker
	{
		public TestInvoker ():base()
		{

		}
	}

	public class TestPM
	{

		public bool Executed = false;

		public TestPM ()
		{

		}
	}

	public class TestCommand1:Command
	{

		[Inject]
		public TestPM PM{ get; set; }

		public TestCommand1 ():base()
		{

		}

		public override void Execute (InvokerArgs args)
		{
			base.Execute (args);
			PM.Executed = true;
		
		}
	}

	public class TestCommand2:Command
	{

		public bool Executed = false;

		public TestCommand2 ():base()
		{

		}

		public override void Execute (InvokerArgs args)
		{
			base.Execute (args);
			Executed = true;
		}
	}

	public interface ITestClass
	{
		void DoSomething ();
	}

	public class TestClass:ITestClass
	{

		public TestClass ()
		{

		}

		public void DoSomething ()
		{

		}
	}
}

