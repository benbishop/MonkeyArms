using System;
using NUnit.Framework;
using MonkeyArms;

namespace MonkeyArmsTests
{
	[TestFixture()]
	public class InvokerTests
	{

		protected Invoker Invoker = new TestInvoker();
		protected TestCommand1 Command1 = new TestCommand1 ();
		protected TestCommand2 Command2 = new TestCommand2 ();



		[Test(Description="Assert each command gets executed when Invoke is called")]
		public void TestInvoke ()
		{
			DI.MapSingleton<TestViewModel> ();
			Invoker.AddCommand<TestCommand1>();
			Invoker.AddCommand<TestCommand2>();
			Invoker.Invoke (new TestInvokerArgs("Hello World"));

			Assert.True (DI.Get<TestViewModel>().Changed);
			Assert.True (DI.Get<TestViewModel>().SomethingElseChanged);

		}

		[Test(Description="Assert invoker passes InvokerArgs payload")]
		public void TestInvokerArgsBeingPassed ()
		{
			DI.MapSingleton<TestViewModel> ();
			Invoker.AddCommand<TestCommand1>();
			Invoker.Invoke (new TestInvokerArgs("Hello World"));

			Assert.AreEqual("Hello World", DI.Get<TestViewModel>().Title);


		}

	
		/*
		 * Test Classes
		 * 
		*/

		public class TestInvokerArgs:InvokerArgs{

			private string newTitle;

			public string NewTitle {
				get {
					return newTitle;
				}
			}

			public TestInvokerArgs(string title):base(){
				newTitle = title;
			}
		}

		public class TestViewModel{

			public bool Changed = false;

			public bool SomethingElseChanged = false;

			public string Title;

			public TestViewModel(){

			}
		}

		public class TestInvoker:Invoker{
			public TestInvoker():base()
			{

			}
		}

		public class TestCommand1:Command{

			[Inject]
			public TestViewModel VM;

			public TestCommand1():base(){

			}

			public override void Execute (InvokerArgs args)
			{
				base.Execute (args);
				VM.Changed = true;
				VM.Title = (args as TestInvokerArgs).NewTitle;
			}
		}

		public class TestCommand2:Command{

			[Inject]
			public TestViewModel VM;

			public TestCommand2():base(){

			}

			public override void Execute (InvokerArgs args)
			{
				base.Execute (args);
				VM.SomethingElseChanged = true;

			}
		}
	}
}

