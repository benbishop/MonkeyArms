using MonkeyArms;
using NUnit.Framework;
using Should;

namespace MonkeyArmsTests
{
	[TestFixture]
	public class DITests
	{
		[Test (Description = "Assert DI does not return null for Get")]
		public void TestGetReturnsNotNull ()
		{
			DI.Get<TestClass> ().ShouldNotBeNull ();
		}

		[Test (Description = "Assert Get returns new instances of a class by default")]
		public void TestGetReturnsNewInstanceByDefault ()
		{
			DI.Get<TestClass> ().ShouldNotEqual (DI.Get<TestClass> ());
		}

		[Test (Description = "Assert when a class is registered as a Singleton the same instance is returned")]
		public void TestGetReturnsSingletonCorrectly ()
		{
			DI.MapSingleton<TestClass> ();
			DI.Get<TestClass> ().ShouldEqual (DI.Get<TestClass> ());
		}

		[Test (Description = "Assert when a class is registered as a Singleton and then unmapped as a Singleton a new instance is returned")]
		public void TestUnMapSingletonWorksCorrectly ()
		{
			DI.MapSingleton<TestClass> ();
			var origClass = DI.Get<TestClass> ();

			DI.UnMapSingleton<TestClass> ();
			var newClass = DI.Get<TestClass> ();

			origClass.ShouldNotEqual (newClass);
		}

		[Test (Description = "Assert when a class is registered via interface Get by interface returns correct class")]
		public void TestRegisterInterface ()
		{
			DI.MapClassToInterface<TestClass, ITestClass> ();

			(DI.Get<ITestClass> () is TestClass).ShouldBeTrue ();
		}

		[Test (Description = "Assert when an interface is registered as singleton it returns an instance of a mapped class to said interface")]
		public void TestMapInterfaceSingleton ()
		{
			DI.MapInterfaceSingleton<ITestClass, TestClass> ();
			var instance1 = DI.Get<ITestClass> ();
			var instance2 = DI.Get<ITestClass> ();
			instance1.ShouldBeSameAs (instance2);
		}

		[Test (Description = "Assert MapCommandToInvoker maps command to invoker and correctly executes command when invoker is invoked")]
		public void TestMapCommandToInvoker ()
		{
			DI.MapSingleton<TestPM> ();
			DI.MapCommandToInvoker<TestCommand1, TestInvoker> ();
			DI.Get<TestInvoker> ().Invoke ();
			DI.Get<TestPM> ().Executed.ShouldBeTrue ();
		}

		[Test (Description = "Assert MapCommandToInvoker maps command to invoker and correctly executes command when invoker is inline invoked")]
		public void TestMapCommandToInvokerInlineInvoke ()
		{
			DI.MapSingleton<TestPM> ();
			DI.MapCommandToInvoker<TestCommand1, TestInvoker> ().Invoke ();
			DI.Get<TestPM> ().Executed.ShouldBeTrue ();
		}

		[Test (Description = "Assert MapInstanceToSingleton returns instance of TestPM passed to it")]
		public void TestMapInstanceToSingleTon ()
		{
			DI.MapSingleton<TestPM> ();
			var pm = new TestPM ();
			DI.MapInstanceToSingleton<TestPM> (pm);
			pm.ShouldEqual (DI.Get<TestPM> ());
		}

		[Test (Description = "Assert MapInstanceToSingleton does not return previous instance of TestPM passed to it")]
		public void TestMapInstanceToSingleTonDoesNotReturnPreviousSignleton ()
		{
			DI.MapSingleton<TestPM> ();
			// ReSharper disable once InconsistentNaming
			var origPM = DI.Get<TestPM> ();
			DI.MapInstanceToSingleton<TestPM> (new TestPM ());
			origPM.ShouldNotEqual (DI.Get<TestPM> ());
		}

		[Test (Description = "Assert MapInstanceToSingleton does not return previous instance of TestPM passed to it after UnMapInstanceFromSingleton is invoked")]
		public void TestMapInstanceToSingleTonDoesNotReturnPreviousSignletonAferUnMapInstanceToSingletonIsInvoiked ()
		{
			// ReSharper disable once InconsistentNaming
			var origPM = new TestPM ();
			DI.MapInstanceToSingleton<TestPM> (new TestPM ());
			DI.UnMapInstanceFromSingleton<TestPM> ();
			origPM.ShouldNotEqual (DI.Get<TestPM> ());
		}

		[Test]
		public void TestDefaultPropertyOfInjectAttribute ()
		{
			var testCommand = new TestCommand3 ();
			DIUtil.InjectProps (testCommand);
			testCommand.TestClassInstance.ShouldBeType<TestClass> ();
			testCommand.Invoker.ShouldNotBeNull ();
		}
	}
	/*
         * Test Classes
         *
        */
	public class TestInvoker : Invoker
	{
	}
	// ReSharper disable once InconsistentNaming
	public class TestPM
	{
		public bool Executed = false;
	}

	public class TestCommand1 : Command
	{
		// ReSharper disable once InconsistentNaming
		[Inject]
		public TestPM PM { get; set; }

		public override void Execute (InvokerArgs args)
		{
			PM.Executed = true;
		}
	}

	public class TestCommand2 : Command
	{
		public bool Executed = false;

		public override void Execute (InvokerArgs args)
		{
			Executed = true;
		}
	}

	public class TestCommand3:Command
	{
		[Inject (Default = typeof(TestClass))]
		public ITestClass TestClassInstance;
		[Inject (Default = typeof(TestClass))]
		public ITestClass TestClassInstance2;
		[Inject]
		public TestPM PM;
		[Inject]
		public TestInvoker Invoker;

		public override void Execute (InvokerArgs args)
		{

		}
	}

	public interface ITestClass
	{
		void DoSomething ();
	}

	public class TestClass : ITestClass
	{
		public void DoSomething ()
		{
		}
	}
}