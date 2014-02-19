using MonkeyArms;
using NUnit.Framework;

namespace MonkeyArmsTests
{
    [TestFixture]
    public class ActorTests
    {
        [Test(Description = "Assert classes get injected into Actor prop")]
        public void TestActorInjectsPropsFromDI()
        {
            DI.MapSingleton<TestClassToInject>();
            var actor = new TestActor();
            Assert.NotNull(actor.TestVar);
        }

        [Test(Description = "Assert classes get injected into Actor field")]
        public void TestActorInjectsFieldsFromDI()
        {
            DI.MapSingleton<TestPM>();
            var actor = new TestActor();
            Assert.NotNull(actor.PM);
        }

        /*
         * Test Classes
         *
         *
        */

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
            public TestPM PM;
        }
    }
}