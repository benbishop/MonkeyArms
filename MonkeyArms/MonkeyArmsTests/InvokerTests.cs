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



		[Test(Description="Assert each command gets exectuted when Invoke is called")]
		public void TestInvoke ()
		{
			Invoker.AddCommand (Command1);
			Invoker.AddCommand (Command2);

			Invoker.Invoke ();

			Assert.True (Command1.Executed);
			Assert.True (Command2.Executed);
		}


		/*
		 * Test Classes
		 * 
		*/

		public class TestInvoker:Invoker{
			public TestInvoker():base()
			{

			}
		}

		public class TestCommand1:Command{

			public bool Executed = false;

			public TestCommand1():base(){

			}

			public override void Execute (InvokerArgs args)
			{
				base.Execute (args);
				Executed = true;
			}
		}

		public class TestCommand2:Command{

			public bool Executed = false;

			public TestCommand2():base(){

			}

			public override void Execute (InvokerArgs args)
			{
				base.Execute (args);
				Executed = true;
			}
		}
	}
}

