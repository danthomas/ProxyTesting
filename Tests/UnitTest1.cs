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

            Converter<>
        }

        [TestMethod]
        public void Test()
        {
            Mock<IThing> thingMock =Mocker.Create<IThing>();

            thingMock.When(thing => thing.Method1("abc")).Return("ABC");
            thingMock.When(thing => thing.Method1("def")).Return("DEF");

            Assert.AreEqual("ABC", thingMock.Object.Method1("abc"));
            Assert.AreEqual("DEF", thingMock.Object.Method1("def"));
        }
    }

    public interface IThing
    {
        string Method1(string arg);
        string Method2(string arg);
        string Method3(string arg);
        string Method4(string arg);
    }
}
