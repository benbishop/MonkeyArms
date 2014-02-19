using MonkeyArms;
using NUnit.Framework;

namespace MonkeyArmsTests
{
    [TestFixture]
    public class InvokerTests
    {
        protected Invoker Invoker = new TestInvoker();
        protected TestCommand1 Command1 = new TestCommand1();
        protected TestCommand2 Command2 = new TestCommand2();

        [Test(Description = "Assert each command gets executed when Invoke is called")]
        public void TestInvoke()
        {
            DI.MapSingleton<TestViewModel>();
            Invoker.AddCommand<TestCommand1>();
            Invoker.AddCommand<TestCommand2>();
            Invoker.Invoke(new TestInvokerArgs("Hello World"));

            Assert.True(DI.Get<TestViewModel>().Changed);
            Assert.True(DI.Get<TestViewModel>().SomethingElseChanged);
        }

        [Test(Description = "Assert invoker passes InvokerArgs payload")]
        public void TestInvokerArgsBeingPassed()
        {
            DI.MapSingleton<TestViewModel>();
            Invoker.AddCommand<TestCommand1>();
            Invoker.Invoke(new TestInvokerArgs("Hello World"));

            Assert.AreEqual("Hello World", DI.Get<TestViewModel>().Title);
        }

        [Test(Description = "Assert invoker calls Invoked event once Invoke complets")]
        public void TestInvokerCallsInvoked()
        {
            var wasInvoked = false;
            Invoker.Invoked += (sender, e) =>
            {
                wasInvoked = true;
            };
            Invoker.Invoke(new TestInvokerArgs("Hello World"));

            Assert.IsTrue(wasInvoked);
        }

        [Test(Description = "Assert an injected invoker calls Invoked event once Invoke complets")]
        public void TestInjetedInvokerCallsInvoked()
        {
            var wasInvoked = false;
            DI.MapSingleton<TestInvoker>();
            DI.Get<TestInvoker>().Invoked += (sender, e) =>
            {
                wasInvoked = true;
            };

            DI.Get<TestInvoker>().Invoke(new TestInvokerArgs("Hello World"));

            Assert.IsTrue(wasInvoked);
        }

        /*
         * Test Classes
         *
        */

        public class TestInvokerArgs : InvokerArgs
        {
            private readonly string _newTitle;

            public string NewTitle
            {
                get
                {
                    return _newTitle;
                }
            }

            public TestInvokerArgs(string title)
            {
                _newTitle = title;
            }
        }

        public class TestViewModel
        {
            public bool Changed = false;

            public bool SomethingElseChanged = false;

            public string Title;
        }

        public class TestInvoker : Invoker
        {
        }

        public class TestCommand1 : Command
        {
            [Inject]
            public TestViewModel VM;

            public override void Execute(InvokerArgs args)
            {
                VM.Changed = true;
                var testInvokerArgs = args as TestInvokerArgs;
                if (testInvokerArgs != null) VM.Title = testInvokerArgs.NewTitle;
            }
        }

        public class TestCommand2 : Command
        {
            [Inject]
            public TestViewModel VM;

            public override void Execute(InvokerArgs args)
            {
                VM.SomethingElseChanged = true;
            }
        }
    }
}