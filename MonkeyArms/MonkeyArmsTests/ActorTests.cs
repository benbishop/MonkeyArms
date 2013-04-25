using System;
using NUnit.Framework;
using MonkeyArms;
using System.Diagnostics;

namespace MonkeyArmsTests
{
	[TestFixture()]
	public class ActorTests
	{
		[Test(Description="Assert classes get injected into Actor prop")]
		public void TestActorInjectsPropsFromDI ()
		{
			DI.MapSingleton<TestClassToInject> ();
			TestActor actor = new TestActor ();
			Assert.NotNull (actor.TestVar);

		}


		[Test(Description="Assert classes get injected into Actor field")]
		public void TestActorInjectsFieldsFromDI ()
		{
			DI.MapSingleton<TestPM> ();
			TestActor actor = new TestActor ();
			Assert.NotNull (actor.PM);
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
			public TestPM PM;

			public TestActor ():base()
			{

			}
		}
	}
}

