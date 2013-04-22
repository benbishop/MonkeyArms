using System;
using NUnit.Framework;
using MonkeyArms;

namespace MonkeyArmsTests
{
	[TestFixture()]
	public class DITests
	{


		[Test(Description="Assert DI does not return null for Get")]
		public void TestGetReturnsNotNull ()
		{
			Assert.NotNull(DI.Get<TestClass>());
		}

		[Test(Description="Assert Get returns new instances of a class by default")]
		public void TestGetReturnsNewInstanceByDefault()
		{
			Assert.AreNotEqual(DI.Get<TestClass>(), DI.Get<TestClass>());
		}

		[Test(Description="Assert when a class is registered as a Singleton the same instance is returned")]
		public void TestGetReturnsSingletonCorrectly()
		{
			DI.MapSingleton<TestClass>();
			Assert.AreEqual(DI.Get<TestClass>(), DI.Get<TestClass>());
		}

		[Test(Description="Assert when a class is registered via interface Get by interface returns correct class")]
		public void TestRegisterInterface()
		{
			DI.MapClassToInterface<ITestClass, TestClass>();
			Assert.IsNotNull(DI.Get<ITestClass>() as TestClass);
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

