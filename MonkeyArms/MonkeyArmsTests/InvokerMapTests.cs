using MonkeyArms;
using NUnit.Framework;
using System;

namespace MonkeyArmsTests
{
    [TestFixture]
    public class InvokerMapTests
    {
        private InvokerMap _testMap;

        private Invoker _testInvoker;

        private bool _invokerHandled;

        [Test(Description = "Add should map a handler to an invoker")]
        public void Verify_Add()
        {
            _testInvoker.Invoke();
            Assert.True(_invokerHandled);
        }

        [Test(Description = "RemoveAll should remove all mappings from map")]
        public void Verify_RemoveAll()
        {
            _testMap.RemoveAll();
            _testInvoker.Invoke();
            Assert.False(_invokerHandled);
        }

        [Test]
        public void Verify_Remove()
        {
            _testMap.Add(_testInvoker, TestHandler);
            Assert.AreEqual(_testMap.GetInvokerHandlerCount(_testInvoker), 2);
            _testMap.Remove(_testInvoker, TestHandler);
            Assert.AreEqual(_testMap.GetInvokerHandlerCount(_testInvoker), 1);
        }

        [Test]
        public void Verify_Handler_Count()
        {
            Assert.AreEqual(_testMap.GetInvokerHandlerCount(_testInvoker), 1);
            _testMap.Add(_testInvoker, (sender, e) => Console.WriteLine("Another handler"));
            Assert.AreEqual(_testMap.GetInvokerHandlerCount(_testInvoker), 2);
        }

        [Test]
        public void Verify_IsInvokerMapped()
        {
            Assert.True(_testMap.IsInvokerMapped(_testInvoker));
            Assert.False(_testMap.IsInvokerMapped(new Invoker()));
        }

        [Test]
        public void Verify_InvokerHasHandler()
        {
            Assert.False(_testMap.InvokerHasHandler(_testInvoker, TestHandler));
            _testMap.Add(_testInvoker, TestHandler);
            Assert.True(_testMap.InvokerHasHandler(_testInvoker, TestHandler));
        }

        private void TestHandler(object sender, EventArgs args)
        {
        }

        [SetUp]
        public void Init()
        {
            _testMap = new InvokerMap();
            _testInvoker = new Invoker();
            _invokerHandled = false;
            _testMap.Add(_testInvoker, (sender, e) => _invokerHandled = true);
        }
    }
}