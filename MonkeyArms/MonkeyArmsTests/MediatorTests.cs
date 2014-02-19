using MonkeyArms;
using NUnit.Framework;
using System;

namespace MonkeyArmsTests
{
    [TestFixture]
    public class MediatorTests
    {
        [Test(Description = "Assert request mediator returns mediator")]
        public void TestRequestMediator()
        {
            DI.MapMediatorToClass<TestMediator, TestMediatorTarget>();
            var target = new TestMediatorTarget();
            Assert.NotNull(DI.RequestMediator(target));
        }

        [Test(Description = "Assert request mediator invokes Registor on Mediator")]
        public void TestRequestMediatorInvokesRegister()
        {
            DI.MapMediatorToClass<TestMediator, TestMediatorTarget>();
            var target = new TestMediatorTarget();
            var testMediator = DI.RequestMediator(target) as TestMediator;
            Assert.True(testMediator != null && testMediator.RegisterInvoked);
        }

        [Test(Description = "Assert RequestMediator throws exception when Target has already been mapped")]
        public void TestRequestMediatorThrowsExceptionIfTargetAlreadyMapped()
        {
            Assert.Throws<ArgumentException>(TryRequestingAMediatorForTargetTwice);
        }

        private void TryRequestingAMediatorForTargetTwice()
        {
            var target = new TestMediatorTarget();
            DI.RequestMediator(target);
            DI.RequestMediator(target);
        }

        [Test(Description = "Assert RequestMediator throws exception when Target type has not been mapped")]
        public void TestRequestMediatorThrowsExceptionIfMediatorNotRegistered()
        {
            Assert.Throws<ArgumentException>(RequestMediatorForUnRegisteredTarget);
        }

        protected void RequestMediatorForUnRegisteredTarget()
        {
            var target = new UnRegisteredMediatorTarget();
            DI.RequestMediator(target);
        }

        [Test(Description = "Assert DestoryMediator doesn't blow errors when invoked")]
        public void TestDestroyMediator()
        {
            DI.MapMediatorToClass<TestMediator, TestMediatorTarget>();
            var target = new TestMediatorTarget();
            DI.RequestMediator(target);
            DI.DestroyMediator(target);
        }

        [Test(Description = "Assert DestoryMediator invokes Unregister on Mediator")]
        public void TestDestroyMediatorInvokesUnregister()
        {
            DI.MapMediatorToClass<TestMediator, TestMediatorTarget>();
            var target = new TestMediatorTarget();
            var m = DI.RequestMediator(target) as TestMediator;
            DI.DestroyMediator(target);
            Assert.True(m != null && m.UnregisterInvoked);
        }

        [Test(Description = "Assert Mediator Injections work")]
        public void TestInjectionsWorkWithMediators()
        {
            DI.MapMediatorToClass<TestMediator, TestMediatorTarget>();
            var target = new TestMediatorTarget();
            var m = DI.RequestMediator(target) as TestMediator;
            if (m != null) Assert.NotNull(m.PM);
        }

        /*
         * Test Classes
         */

        public interface ITestTargetInteface
        {
            void DoSomething();

            void DoAnotherThing();
        }

        // ReSharper disable once InconsistentNaming
        public class TestPM
        {
        }

        public class TestMediatorTarget : IMediatorTarget, ITestTargetInteface
        {
            public void DoSomething()
            {
            }

            public void DoAnotherThing()
            {
            }
        }

        public class UnRegisteredMediatorTarget : IMediatorTarget
        {
        }

        public class TestMediator : Mediator
        {
            [Inject]
            // ReSharper disable once InconsistentNaming
            public TestPM PM;

            public bool RegisterInvoked = false;

            public bool UnregisterInvoked = false;

            protected ITestTargetInteface Target;

            public TestMediator(IMediatorTarget target)
                : base(target)
            {
                Target = target as ITestTargetInteface;
            }

            public override void Register()
            {
                RegisterInvoked = true;
            }

            public override void Unregister()
            {
                UnregisterInvoked = true;
            }
        }
    }
}