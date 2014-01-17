using NUnit.Framework;
using System;
using MonkeyArms;
using Moq;

namespace MonkeyArmsTests
{
	[TestFixture ()]
	public class InvokerChainTests
	{

		public class BaseChainTest{
			protected Invoker startInvoker = new Invoker ();

			protected Mock<IChainableInvoker> mockInvoker  = new Mock<IChainableInvoker> ();

			protected IChainableInvoker middleInvoker = new MiddleInvoker ();

			protected InvokerArgs testInvokerArgs = new InvokerArgs ();

			protected InvokerChain chain;
		}


		public class TwoInvokerChain:BaseChainTest{


			public void SetupTwoInvokerChain()
			{
				chain = new InvokerChain ();
				chain.Add (startInvoker, mockInvoker.Object);

			}

			[Test ()]
			public void VerifyChainOfTwoInvokers ()
			{
				SetupTwoInvokerChain ();
				chain.Start (startInvoker, testInvokerArgs);
				mockInvoker.Verify (invoker => invoker.InvokeFrom(It.Is<InvokerArgs>(args => args == testInvokerArgs)), Times.Once ());

			}

			[Test]
			public void VerifyGetArgsIsZero()
			{
				SetupTwoInvokerChain ();
				chain.Start (startInvoker, testInvokerArgs);
				Assert.AreEqual (0, chain.GetArgsOfType<InvokerArgs> ().Count);

			}

			[Test]
			public void VerifyGetArgsIsOne()
			{
				SetupTwoInvokerChain ();
				chain.CollectArgsForInvokers (new Invoker[]{ startInvoker });
				chain.Start (startInvoker, testInvokerArgs);
				Assert.AreEqual (1, chain.GetArgsOfType<InvokerArgs> ().Count);

			}



		}

		public class ThreeInvokerChain:BaseChainTest{

			[SetUp]
			public void SetupThreeInvokerChain(){
				chain = new InvokerChain ();
				chain.Add (startInvoker, middleInvoker);
				chain.Add (middleInvoker, mockInvoker.Object);


			}

			[Test ()]
			public void VerifyChainOfThreeInvokers ()
			{
				chain.Start (startInvoker, testInvokerArgs);
				mockInvoker.Verify (invoker => invoker.InvokeFrom(It.IsAny<InvokerArgs>()), Times.Once ());

			}

			[Test]
			public void VerifyGetArgsIsTwo()
			{
				SetupThreeInvokerChain ();
				chain.CollectArgsForInvokers (new IInvoker[]{ startInvoker, middleInvoker });
				chain.Start (startInvoker, testInvokerArgs);
				Assert.AreEqual (2, chain.GetArgsOfType<InvokerArgs> ().Count);

			}

			[Test]
			public void VerifyGetArgsReturnsCorrectNumberOfType()
			{
				SetupThreeInvokerChain ();
				chain.CollectArgsForInvokers (new IInvoker[]{ startInvoker, middleInvoker });
				chain.Start (startInvoker, testInvokerArgs);
				Assert.AreEqual (1, chain.GetArgsOfType<MiddleInvokerArgs> ().Count);

			}
		}


	}

	class MiddleInvoker:Invoker,IChainableInvoker{
		public void InvokeFrom (InvokerArgs args)
		{
			Invoke (new MiddleInvokerArgs());
		}
	}

	class MiddleInvokerArgs:InvokerArgs{

	}
}

