using System;
using NUnit.Framework;
using MonkeyArms;
using System.Diagnostics;

namespace iOSTests
{
	[TestFixture]
    public class ActorTest
	{
		[Test]
        public void Pass ()
		{
			DI.MapSingleton<TestClassToInject> ();

			TestActor actor = new TestActor ();

			Debug.WriteLine ("test");
			Assert.True (true);
		}

		[Test]
		[Ignore ("another time")]
		public void Ignore ()
		{
			Assert.True (false);
		}
	}

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
