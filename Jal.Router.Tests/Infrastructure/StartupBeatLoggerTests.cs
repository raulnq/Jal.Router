using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class StartupBeatLoggerTests
    {
        [TestMethod]
        public async Task Run_WithNoLoggers_ShouldNotLog()
        {
            var loggermock = new Mock<ILogger<Beat>>();

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            factorymock.Setup(x => x.CreateLogger<Beat>(It.IsAny<Type>())).Returns(loggermock.Object);

            factory.Configuration.LoggerTypes.Clear();

            var sut = new StartupBeatLogger(factory, new NullLogger());

            await sut.Run();

            factorymock.Verify(x => x.CreateLogger<Beat>(It.IsAny<Type>()), Times.Never);

            loggermock.Verify(x => x.Log(It.IsAny<Beat>(), It.IsAny<DateTime>()), Times.Never);
        }

        [TestMethod]
        public async Task Run_WithLoggers_ShouldLog()
        {
            var loggermock = new Mock<ILogger<Beat>>();

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            factorymock.Setup(x => x.CreateLogger<Beat>(It.IsAny<Type>())).Returns(loggermock.Object);

            factory.Configuration.LoggerTypes.Clear();

            factory.Configuration.LoggerTypes.Add(typeof(Beat), new List<Type>() { typeof(object) });

            var sut = new StartupBeatLogger(factory, new NullLogger());

            await sut.Run();

            factorymock.Verify(x => x.CreateLogger<Beat>(It.IsAny<Type>()), Times.Once);

            loggermock.Verify(x => x.Log(It.IsAny<Beat>(), It.IsAny<DateTime>()), Times.Once);
        }
    }
}
