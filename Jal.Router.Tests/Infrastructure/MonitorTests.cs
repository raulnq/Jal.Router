using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using Monitor = Jal.Router.Impl.Monitor;

namespace Jal.Router.Tests
{
    [TestClass]
    public class MonitorTests
    {
        [TestMethod]
        public void Run_WithType_ShouldRun()
        {
            var startuptask = new Mock<IMonitoringTask>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateMonitoringTask(It.IsAny<Type>())).Returns(startuptask.Object);

            var factory = factorymock.Object;

            factory.Configuration.MonitoringTaskTypes.Clear();

            factory.Configuration.MonitoringTaskTypes.Add(new Model.MonitorTask(typeof(string), 250, false));

            var sut = new Monitor(factory, new NullLogger());

            sut.Run();

            Thread.Sleep(500);

            factorymock.Verify(x => x.CreateMonitoringTask(It.IsAny<Type>()), Times.Once);

            startuptask.Verify(x => x.Run(It.IsAny<DateTime>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Run_WithNoTypes_ShouldNotRun()
        {
            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            factory.Configuration.StartupTaskTypes.Clear();

            var sut = new Monitor(factory, new NullLogger());

            sut.Run();

            factorymock.Verify(x => x.CreateStartupTask(It.IsAny<Type>()), Times.Never);
        }
    }
}
