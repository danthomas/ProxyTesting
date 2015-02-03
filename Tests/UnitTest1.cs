using System;
using System.Security.Cryptography.X509Certificates;
using Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProxyTesting;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Mock<IMainControllerService> mainControllerService = Mocker.Create<IMainControllerService>();

            mainControllerService.When(t => t.Method1()).Return("one");
            mainControllerService.When(t => t.Method2()).Return("two");

            MainController mainController = new MainController(mainControllerService.Object);

            mainController.Method1();

            Assert.AreEqual("one", mainController.Data);

            mainController.Method2();

            Assert.AreEqual("two", mainController.Data);

        }

        [TestMethod]
        public void Returns_StringArg()
        {
            Mock<IThing> thingMock = Mocker.Create<IThing>();

            thingMock.When(thing => thing.Method1("abc")).Return("ABC");
            thingMock.When(thing => thing.Method1("def")).Return("DEF");
            thingMock.When(thing => thing.Method1("abc")).Return("XYZ");

            Assert.AreEqual("ABC", thingMock.Object.Method1("abc"));
            Assert.AreEqual("DEF", thingMock.Object.Method1("def"));
            Assert.AreEqual("XYZ", thingMock.Object.Method1("abc"));
            Assert.AreEqual(null, thingMock.Object.Method1("abc"));
        }

        [TestMethod]
        public void Returns_Int32Arg()
        {
            Mock<IThing> thingMock = Mocker.Create<IThing>();

            thingMock.When(thing => thing.Method2(1)).Return("ABC");
            thingMock.When(thing => thing.Method2(2)).Return("DEF");
            thingMock.When(thing => thing.Method2(1)).Return("XYZ");

            Assert.AreEqual("ABC", thingMock.Object.Method2(1));
            Assert.AreEqual("DEF", thingMock.Object.Method2(2));
            Assert.AreEqual("XYZ", thingMock.Object.Method2(1));
            Assert.AreEqual(null, thingMock.Object.Method2(1));
        }

        [TestMethod]
        public void Xxx()
        {
            Mock<IThing> thingMock = Mocker.Create<IThing>();
            
            string arg = null;
            thingMock.Log<string>(thing => thing.Method1(""), (arg1) =>
            {
                arg = arg1;
            });

            thingMock.Object.Method1("abcd");

            Assert.AreEqual("abcd", arg);
        }
    }

    public interface IThing
    {
        string Method1(string arg);
        string Method2(int arg);
        string Method3(string arg);
        string Method4(string arg);
    }
}
