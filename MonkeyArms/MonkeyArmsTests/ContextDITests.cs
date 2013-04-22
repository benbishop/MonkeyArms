using System;
using NUnit.Framework;
using MonkeyArms;

namespace MonkeyArmsTests
{
	[TestFixture()]
	public class ContextDITests
	{
		protected TestContext Context = new TestContext();

		[Test(Description="Assert DI does not return null for Get")]
		public void TestGetReturnsNotNull ()
		{
			Assert.NotNull(Context.Get<TestClass>());
		}

		[Test(Description="Assert Get returns new instances of a class by default")]
		public void TestGetReturnsNewInstanceByDefault()
		{
			Assert.AreNotEqual(Context.Get<TestClass>(), Context.Get<TestClass>());
		}

		[Test(Description="Assert when a class is registered as a Singleton the same instance is returned")]
		public void TestGetReturnsSingletonCorrectly()
		{
			Context.MapSingleton<TestClass>();
			Assert.AreEqual(Context.Get<TestClass>(), Context.Get<TestClass>());
		}

		[Test(Description="Assert when a class is registered via interface Get by interface returns correct class")]
		public void TestRegisterInterface()
		{
			Context.MapClassToInterface<ITestClass, TestClass>();
			Assert.IsNotNull(Context.Get<ITestClass>() as TestClass);
		}
	}

	public class TestContext:AppContext{
		public TestContext():base(){

		}
	}

	public interface ITestClass{
		void DoSomething();
	}

	public class TestClass:ITestClass{

		public TestClass(){

		}

		public void DoSomething()
		{

		}
	}
}

