using System;
using NUnit.Framework;
using MonkeyArms;
using System.Diagnostics;

namespace MonkeyArmsTests
{
	[TestFixture()]
	public class ActorTests
	{
		[Test(Description="Assert classes get injected into Actor")]
		public void TestActorInjectsPropsFromDI ()
		{
			DI.MapSingleton<TestClassToInject> ();
			TestActor actor = new TestActor ();
			Assert.NotNull (actor.TestVar);
		}

		/*
		 * Test Classes
		 * 
		 * 
		*/

		public class TestClassToInject
		{
			public TestClassToInject ()
			{
			}
		}

		public class TestPM
		{
			public string Title = "This is the title";

			public TestPM ()
			{

			}
		}

		public class TestActor:Actor
		{

			[Inject]
			public TestClassToInject TestVar{ get; set; }

			[Inject]
			public TestPM PM { get; set; }

			public TestActor ():base()
			{

			}
		}
	}
}

