using System;
using NUnit.Framework;
using MonkeyArms;

namespace MonkeyArmsTests
{
	[TestFixture()]
	public class MediatorTests
	{
		[Test(Description="Assert request mediator returns mediator")]
		public void TestRequestMediator ()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			TestMediatorTarget target = new TestMediatorTarget ();
			Assert.NotNull(DI.RequestMediator(target));
		}

		[Test(Description="Assert request mediator invokes Registor on Mediator")]
		public void TestRequestMediatorInvokesRegister ()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			TestMediatorTarget target = new TestMediatorTarget ();
			Assert.True((DI.RequestMediator(target) as TestMediator).RegisterInvoked);
		}


		[Test(Description="Assert RequestMediator throws exception when Target has already been mapped")]
		public void TestRequestMediatorThrowsExceptionIfTargetAlreadyMapped ()
		{

			Assert.Throws<ArgumentException>(TryRequestingAMediatorForTargetTwice);
		}

		void TryRequestingAMediatorForTargetTwice()
		{
			TestMediatorTarget target = new TestMediatorTarget ();
			DI.RequestMediator (target);
			DI.RequestMediator (target);
		}


		[Test(Description="Assert RequestMediator throws exception when Target type has not been mapped")]
		public void TestRequestMediatorThrowsExceptionIfMediatorNotRegistered ()
		{
			Assert.Throws<ArgumentException>(RequestMediatorForUnRegisteredTarget);
		}

		protected void RequestMediatorForUnRegisteredTarget(){
			UnRegisteredMediatorTarget target = new UnRegisteredMediatorTarget ();
			DI.RequestMediator (target);
		}

		[Test(Description="Assert DestoryMediator doesn't blow errors when invoked")]
		public void TestDestroyMediator()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			TestMediatorTarget target = new TestMediatorTarget ();
			DI.RequestMediator(target);
			DI.DestroyMediator (target);
		}

		[Test(Description="Assert DestoryMediator invokes Unregister on Mediator")]
		public void TestDestroyMediatorInvokesUnregister()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			TestMediatorTarget target = new TestMediatorTarget ();
			var m = DI.RequestMediator(target) as TestMediator;
			DI.DestroyMediator (target);
			Assert.True (m.UnregisterInvoked);
		}

		[Test(Description="Assert Mediator Injections work")]
		public void TestInjectionsWorkWithMediators()
		{
			DI.MapMediatorToClass<TestMediator, TestMediatorTarget> ();
			TestMediatorTarget target = new TestMediatorTarget ();
			var m = DI.RequestMediator(target) as TestMediator;
			Assert.NotNull (m.PM);
		}

		/*
		 * Test Classes
		 */

		public interface ITestTargetInteface{
			void DoSomething();
			void DoAnotherThing();
		}

		public class TestPM{
			public TestPM(){

			}
		}

		public class TestMediatorTarget:IMediatorTarget,ITestTargetInteface{


			public void DoSomething()
			{

			}

			public void DoAnotherThing()
			{

			}
		}

		public class UnRegisteredMediatorTarget:IMediatorTarget{
			public UnRegisteredMediatorTarget()
			{

			}
		}

		public class TestMediator:Mediator{

			[Inject]
			public TestPM PM;

			public bool RegisterInvoked = false;

			public bool UnregisterInvoked = false;

			protected ITestTargetInteface Target;

			public TestMediator(IMediatorTarget target):base(target){
				Target = target as ITestTargetInteface;
			}

			public override void Register ()
			{
				base.Register ();
				RegisterInvoked = true;
			}

			public override void Unregister ()
			{
				base.Unregister ();
				UnregisterInvoked = true;
			}
		}
	}
}

