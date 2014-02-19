using MonkeyArms;
using NUnit.Framework;

namespace iOSTests
{
    [TestFixture]
    public class ActorTest
    {
        [Test]
        public void Pass()
        {
            DI.MapSingleton<TestClassToInject>();

            //			TestActor actor = new TestActor ();

            //			Debug.WriteLine ("test");
            Assert.True(true);
        }

        [Test]
        [Ignore("another time")]
        public void Ignore()
        {
            Assert.True(false);
        }
    }

    public class TestClassToInject
    {
    }

    // ReSharper disable once InconsistentNaming
    public class TestPM
    {
        public string Title = "This is the title";
    }

    public class TestActor : Actor
    {
        [Inject]
        public TestClassToInject TestVar { get; set; }

        [Inject]
        // ReSharper disable once InconsistentNaming
        public TestPM PM { get; set; }
    }
}