using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Jal.Router.Tests.Infrastructure
{
    [TestClass]
    public class ComponentFactoryTests
    {

    }
    [TestClass]
    public class ParameterProviderTests
    {

    }
    [TestClass]
    public class HeartBeatLoggerTests
    {
    }
    [TestClass]
    public class ListenerMonitorTests
    {
    }
    [TestClass]
    public class ListenerRestartMonitorTests
    {
    }
    [TestClass]
    public class PointToPointChannelMonitorTests
    {
    }
    [TestClass]
    public class SubscriptionToPublishSubscribeChannelMonitorTests
    {
    }
    [TestClass]
    public class ListenerShutdownTaskTests
    {
    }
    [TestClass]
    public class PointToPointChannelResourceDestructorTests
    {
    }
    [TestClass]
    public class PublishSubscribeChannelResourceDestructorTests
    {
    }
    [TestClass]
    public class SenderShutdownTaskTests
    {
    }
    [TestClass]
    public class ShutdownTaskTests
    {
    }
    [TestClass]
    public class SubscriptionToPublishSubscribeChannelResourceDestructorTests
    {
    }
    [TestClass]
    public class EndpointsInitializerTests
    {
    }
    [TestClass]
    public class ListenerLoaderTests
    {
    }
    [TestClass]
    public class PointToPointChannelResourceCreatorTests
    {
    }
    [TestClass]
    public class PublishSubscribeChannelResourceCreatorTests
    {
    }
    [TestClass]
    public class RoutesInitializerTests
    {
        [DataTestMethod]
        [DataRow("","","","")]
        [DataRow("partitionconnectionstring", "", "", "")]
        [DataRow("", "partitionpath", "", "")]
        [DataRow("", "", "connectionstring", "")]
        [DataRow("", "", "", "path")]
        public async Task Run_WithMissingData_ShouldThrowException(string partitionconnectionstring, string partitionpath, string connectionstring, string path)
        {
            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var route = Builder.CreateRoute();

            route.Channels.Add(Builder.CreateChannel(connectionstring: partitionconnectionstring, path: partitionpath));

            factory.Configuration.Runtime.Routes.Add(route);

            var partition = new Partition("name");

            partition.UpdateChannel(Builder.CreateChannel(connectionstring: connectionstring, path: path));

            factory.Configuration.Runtime.Partitions.Add(partition);

            var sut = new RoutesInitializer(factory, new NullLogger());

            var exception = false;

            try
            {
                await sut.Run();
            }
            catch (ApplicationException)
            {
                exception = true;
            }

            exception.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Run_WithRouteAndPartition_ShouldBeInitialized()
        {
            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var route = Builder.CreateRoute();

            route.Channels.Add(Builder.CreateChannel());

            factory.Configuration.Runtime.Routes.Add(route);

            var partition = new Partition("name");

            partition.UpdateChannel(Builder.CreateChannel());

            factory.Configuration.Runtime.Partitions.Add(partition);

            var sut = new RoutesInitializer(factory, new NullLogger());

            await sut.Run();
        }
    }

    [TestClass]
    public class StartupTests
    {
        [TestMethod]
        public async Task Run_WithType_ShouldRun()
        {
            var startuptask = new Mock<IStartupTask>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateStartupTask(It.IsAny<Type>())).Returns(startuptask.Object);

            var factory = factorymock.Object;

            factory.Configuration.StartupTaskTypes.Clear();

            factory.Configuration.StartupTaskTypes.Add(typeof(string));

            var sut = new Startup(factory, new NullLogger());

            await sut.Run();

            factorymock.Verify(x => x.CreateStartupTask(It.IsAny<Type>()), Times.Once);

            startuptask.Verify(x => x.Run(), Times.Once);
        }

        [TestMethod]
        public async Task Run_WithException_ShouldThrowException()
        {
            var startuptask = new Mock<IStartupTask>();

            startuptask.Setup(x => x.Run()).Throws(new Exception());

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateStartupTask(It.IsAny<Type>())).Returns(startuptask.Object);

            var factory = factorymock.Object;

            factory.Configuration.StartupTaskTypes.Clear();

            factory.Configuration.StartupTaskTypes.Add(typeof(string));

            var sut = new Startup(factory, new NullLogger());

            await Should.ThrowAsync<Exception>(()=>sut.Run());

            factorymock.Verify(x => x.CreateStartupTask(It.IsAny<Type>()), Times.Once);

            startuptask.Verify(x => x.Run(), Times.Once);
        }

        [TestMethod]
        public async Task Run_WithNoTypes_ShouldNotRun()
        {
            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            factory.Configuration.StartupTaskTypes.Clear();

            var sut = new Startup(factory, new NullLogger());

            await sut.Run();

            factorymock.Verify(x => x.CreateStartupTask(It.IsAny<Type>()), Times.Never);
        }
    }
}
