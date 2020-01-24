using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class ShutdownTests
    {
        [TestMethod]
        public async Task Run_WithType_ShouldRun()
        {
            var shutdowntask = new Mock<IShutdownTask>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateShutdownTask(It.IsAny<Type>())).Returns(shutdowntask.Object);

            var factory = factorymock.Object;

            factory.Configuration.ShutdownTaskTypes.Clear();

            factory.Configuration.ShutdownTaskTypes.Add(typeof(string));

            var sut = new Shutdown(factory, new NullLogger());

            await sut.Run();

            factorymock.Verify(x => x.CreateShutdownTask(It.IsAny<Type>()), Times.Once);

            shutdowntask.Verify(x => x.Run(), Times.Once);
        }

        [TestMethod]
        public async Task Run_WithException_ShouldThrowException()
        {
            var shutdowntask = new Mock<IShutdownTask>();

            shutdowntask.Setup(x => x.Run()).Throws(new Exception());

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateShutdownTask(It.IsAny<Type>())).Returns(shutdowntask.Object);

            var factory = factorymock.Object;

            factory.Configuration.ShutdownTaskTypes.Clear();

            factory.Configuration.ShutdownTaskTypes.Add(typeof(string));

            var sut = new Shutdown(factory, new NullLogger());

            await Should.ThrowAsync<Exception>(() => sut.Run());

            factorymock.Verify(x => x.CreateShutdownTask(It.IsAny<Type>()), Times.Once);

            shutdowntask.Verify(x => x.Run(), Times.Once);
        }

        [TestMethod]
        public async Task Run_WithNoTypes_ShouldNotRun()
        {
            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            factory.Configuration.ShutdownTaskTypes.Clear();

            var sut = new Shutdown(factory, new NullLogger());

            await sut.Run();

            factorymock.Verify(x => x.CreateShutdownTask(It.IsAny<Type>()), Times.Never);
        }
    }
}
