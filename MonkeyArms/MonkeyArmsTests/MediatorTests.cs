using MonkeyArms;
using NUnit.Framework;
using System;
using Should;

namespace MonkeyArmsTests
{
	[TestFixture]
	public class MediatorTests
	{
		[Test (Description = "Assert request mediator returns mediator")]
		public void TestRequestMediator ()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			var target = new TestMediatorTarget ();
			DI.RequestMediator (target).ShouldNotBeNull ();
		}

		[Test (Description = "Assert request mediator invokes Registor on Mediator")]
		public void TestRequestMediatorInvokesRegister ()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			var target = new TestMediatorTarget ();
			var testMediator = (TestMediator)DI.RequestMediator (target);
			testMediator.ShouldNotBeNull ();
			testMediator.RegisterInvoked.ShouldBeTrue ();

		}

		[Test (Description = "Assert RequestMediator throws exception when Target has already been mapped")]
		public void TestRequestMediatorThrowsExceptionIfTargetAlreadyMapped ()
		{
			Assert.Throws<ArgumentException> (TryRequestingAMediatorForTargetTwice);
		}

		[Test (Description = "Assert RequestMediator throws exception when Target type has not been mapped")]
		public void TestRequestMediatorThrowsExceptionIfMediatorNotRegistered ()
		{
			Assert.Throws<ArgumentException> (RequestMediatorForUnRegisteredTarget);
		}

		protected void RequestMediatorForUnRegisteredTarget ()
		{
			var target = new UnRegisteredMediatorTarget ();
			DI.RequestMediator (target);
		}

		[Test (Description = "Assert DestoryMediator doesn't blow errors when invoked")]
		public void TestDestroyMediator ()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			var target = new TestMediatorTarget ();
			DI.RequestMediator (target);
			DI.DestroyMediator (target);
		}

		[Test (Description = "Assert DestoryMediator invokes Unregister on Mediator")]
		public void TestDestroyMediatorInvokesUnregister ()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			var target = new TestMediatorTarget ();
			var mediator = (TestMediator)DI.RequestMediator (target);
			DI.DestroyMediator (target);
			mediator.ShouldNotBeNull ();
			mediator.UnregisterInvoked.ShouldBeTrue ();

		}

		[Test (Description = "Assert Mediator Injections work")]
		public void TestInjectionsWorkWithMediators ()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			var target = new TestMediatorTarget ();
			var m = (TestMediator)DI.RequestMediator (target);
			m.ShouldNotBeNull ();
			m.PM.ShouldNotBeNull ();
		}

		protected void TryRequestingAMediatorForTargetTwice ()
		{
			var target = new TestMediatorTarget ();
			DI.RequestMediator (target);
			DI.RequestMediator (target);
		}
		/*
         * Test Classes
         */
		public interface ITestTargetInteface
		{
			void DoSomething ();

			void DoAnotherThing ();
		}
		// ReSharper disable once InconsistentNaming
		public class TestPM
		{
		}

		public class TestMediatorTarget : IMediatorTarget, ITestTargetInteface
		{
			public void DoSomething ()
			{
			}

			public void DoAnotherThing ()
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

			public TestMediator (IMediatorTarget target)
                : base (target)
			{
				Target = target as ITestTargetInteface;
			}

			public override void Register ()
			{
				RegisterInvoked = true;
			}

			public override void Unregister ()
			{
				UnregisterInvoked = true;
			}
		}
	}
}