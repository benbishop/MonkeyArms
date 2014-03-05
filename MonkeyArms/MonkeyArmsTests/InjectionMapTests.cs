using NUnit.Framework;
using System;
using MonkeyArms;
using Should;

namespace MonkeyArmsTests
{
	[TestFixture ()]
	public class InjectionMapTests
	{
		[Test (Description = "Injection map should allow a developer to specify what instance of a class type should be used")]
		public void VerifyAddGet ()
		{
			TestClass testClassInstance = new TestClass ();
			TestInjectionMap.HasTypeMapped (typeof(TestClass)).ShouldBeFalse ();
			TestInjectionMap.Add<TestClass> (() => testClassInstance);
			TestInjectionMap.HasTypeMapped (typeof(TestClass)).ShouldBeTrue ();
			TestInjectionMap.Get (typeof(TestClass)).ShouldEqual (testClassInstance);

		}

		protected InjectionMap TestInjectionMap = new InjectionMap ();

		public class TestClass
		{
		}
	}
}

