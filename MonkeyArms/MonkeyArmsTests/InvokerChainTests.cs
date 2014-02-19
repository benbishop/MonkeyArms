using MonkeyArms;
using Moq;
using NUnit.Framework;

namespace MonkeyArmsTests
{
    [TestFixture]
    public class InvokerChainTests
    {
        public class BaseChainTest
        {
            protected Invoker StartInvoker = new Invoker();

            protected Mock<IChainableInvoker> MockInvoker = new Mock<IChainableInvoker>();

            protected IChainableInvoker MiddleInvoker = new MiddleInvoker();

            protected InvokerArgs TestInvokerArgs = new InvokerArgs();

            protected InvokerChain Chain;
        }

        public class TwoInvokerChain : BaseChainTest
        {
            public void SetupTwoInvokerChain()
            {
                Chain = new InvokerChain();
                Chain.Add(StartInvoker, MockInvoker.Object);
            }

            [Test]
            public void VerifyChainOfTwoInvokers()
            {
                SetupTwoInvokerChain();
                Chain.Start(StartInvoker, TestInvokerArgs);
                MockInvoker.Verify(invoker => invoker.InvokeFrom(It.Is<InvokerArgs>(args => args == TestInvokerArgs)), Times.Once());
            }

            [Test]
            public void VerifyGetArgsIsZero()
            {
                SetupTwoInvokerChain();
                Chain.Start(StartInvoker, TestInvokerArgs);
                Assert.AreEqual(0, Chain.GetArgsOfType<InvokerArgs>().Count);
            }

            [Test]
            public void VerifyGetArgsIsOne()
            {
                SetupTwoInvokerChain();
                // ReSharper disable once CoVariantArrayConversion
                Chain.CollectArgsForInvokers(new[] { StartInvoker });
                Chain.Start(StartInvoker, TestInvokerArgs);
                Assert.AreEqual(1, Chain.GetArgsOfType<InvokerArgs>().Count);
            }
        }

        public class ThreeInvokerChain : BaseChainTest
        {
            [SetUp]
            public void SetupThreeInvokerChain()
            {
                Chain = new InvokerChain();
                Chain.Add(StartInvoker, MiddleInvoker);
                Chain.Add(MiddleInvoker, MockInvoker.Object);
            }

            [Test]
            public void VerifyChainOfThreeInvokers()
            {
                Chain.Start(StartInvoker, TestInvokerArgs);
                MockInvoker.Verify(invoker => invoker.InvokeFrom(It.IsAny<InvokerArgs>()), Times.Once());
            }

            [Test]
            public void VerifyGetArgsIsTwo()
            {
                SetupThreeInvokerChain();
                Chain.CollectArgsForInvokers(new IInvoker[] { StartInvoker, MiddleInvoker });
                Chain.Start(StartInvoker, TestInvokerArgs);
                Assert.AreEqual(2, Chain.GetArgsOfType<InvokerArgs>().Count);
            }

            [Test]
            public void VerifyGetArgsReturnsCorrectNumberOfType()
            {
                SetupThreeInvokerChain();
                Chain.CollectArgsForInvokers(new IInvoker[] { StartInvoker, MiddleInvoker });
                Chain.Start(StartInvoker, TestInvokerArgs);
                Assert.AreEqual(1, Chain.GetArgsOfType<MiddleInvokerArgs>().Count);
            }
        }
    }

    internal class MiddleInvoker : Invoker, IChainableInvoker
    {
        public void InvokeFrom(InvokerArgs args)
        {
            Invoke(new MiddleInvokerArgs());
        }
    }

    internal class MiddleInvokerArgs : InvokerArgs
    {
    }
}