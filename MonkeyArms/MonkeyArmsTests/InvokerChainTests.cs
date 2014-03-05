using MonkeyArms;
using Moq;
using NUnit.Framework;
using Should;

namespace MonkeyArmsTests
{
	[TestFixture]
	public class InvokerChainTests
	{
		public class TwoInvokerChain : BaseChainTest
		{
			[Test]
			public void VerifyChainOfTwoInvokers ()
			{
				SetupTwoInvokerChain ();
				Chain.Start (StartInvoker, TestInvokerArgs);
				MockInvoker.Verify (invoker => invoker.InvokeFrom (It.Is<InvokerArgs> (args => args == TestInvokerArgs)), Times.Once ());
			}

			[Test]
			public void VerifyGetArgsIsZero ()
			{
				SetupTwoInvokerChain ();
				Chain.Start (StartInvoker, TestInvokerArgs);
				Chain.GetArgsOfType<InvokerArgs> ().Count.ShouldEqual (0);
			}

			[Test]
			public void VerifyGetArgsIsOne ()
			{
				SetupTwoInvokerChain ();
				// ReSharper disable once CoVariantArrayConversion
				Chain.CollectArgsForInvokers (new[] { StartInvoker });
				Chain.Start (StartInvoker, TestInvokerArgs);
				Chain.GetArgsOfType<InvokerArgs> ().Count.ShouldEqual (1);
			}

			public void SetupTwoInvokerChain ()
			{
				Chain = new InvokerChain ();
				Chain.Add (StartInvoker, MockInvoker.Object);
			}
		}

		public class ThreeInvokerChain : BaseChainTest
		{
			[SetUp]
			public void SetupThreeInvokerChain ()
			{
				Chain = new InvokerChain ();
				Chain.Add (StartInvoker, MiddleInvoker);
				Chain.Add (MiddleInvoker, MockInvoker.Object);
			}

			[Test]
			public void VerifyChainOfThreeInvokers ()
			{
				Chain.Start (StartInvoker, TestInvokerArgs);
				MockInvoker.Verify (invoker => invoker.InvokeFrom (It.IsAny<InvokerArgs> ()), Times.Once ());
			}

			[Test]
			public void VerifyGetArgsIsTwo ()
			{
				SetupThreeInvokerChain ();
				Chain.CollectArgsForInvokers (new IInvoker[] { StartInvoker, MiddleInvoker });
				Chain.Start (StartInvoker, TestInvokerArgs);
				Chain.GetArgsOfType<InvokerArgs> ().Count.ShouldEqual (2);
			}

			[Test]
			public void VerifyGetArgsReturnsCorrectNumberOfType ()
			{
				SetupThreeInvokerChain ();
				Chain.CollectArgsForInvokers (new IInvoker[] { StartInvoker, MiddleInvoker });
				Chain.Start (StartInvoker, TestInvokerArgs);
				Chain.GetArgsOfType<MiddleInvokerArgs> ().Count.ShouldEqual (1);
			}

			public void SetupTwoInvokerChain ()
			{
				Chain = new InvokerChain ();
				Chain.Add (StartInvoker, MockInvoker.Object);
			}
		}
	}

	public class BaseChainTest
	{
		protected Invoker StartInvoker = new Invoker ();
		protected Mock<IChainableInvoker> MockInvoker = new Mock<IChainableInvoker> ();
		protected IChainableInvoker MiddleInvoker = new MiddleInvoker ();
		protected InvokerArgs TestInvokerArgs = new InvokerArgs ();
		protected InvokerChain Chain;
	}

	internal class MiddleInvoker : Invoker, IChainableInvoker
	{
		public void InvokeFrom (InvokerArgs args)
		{
			Invoke (new MiddleInvokerArgs ());
		}
	}

	internal class MiddleInvokerArgs : InvokerArgs
	{
	}
}