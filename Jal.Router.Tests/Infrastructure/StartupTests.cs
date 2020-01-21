using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
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
    }
    [TestClass]
    public class RuntimeConfigurationLoaderTests
    {
    }
    [TestClass]
    public class SenderLoaderTests
    {
    }
    [TestClass]
    public class StartupBeatLoggerTests
    {
    }
    [TestClass]
    public class SubscriptionToPublishSubscribeChannelResourceCreatorTests
    {
    }
    [TestClass]
    public class ListenerContextCreatorTests
    {
        [TestMethod]
        public void Open_With_ShouldBeOpened()
        {
            var factorymock = Builder.CreateFactoryMock();

            var listenermock = new Mock<IListenerChannel>();

            var factory = factorymock.Object;

            var sut = new ListenerContextCreator(factory, new NullLogger());

            sut.Open(new Model.ListenerContext(new Model.Channel(Model.ChannelType.PointToPoint, null, null, "path"), listenermock.Object, null));

            listenermock.Verify(x => x.Listen(It.IsAny<Model.ListenerContext>()), Times.Once);

            listenermock.Verify(x => x.Open(It.IsAny<Model.ListenerContext>()), Times.Once);
        }

        [TestMethod]
        public void Create_With_ShouldBeCreated()
        {
            var listenermock = new Mock<IListenerChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateListenerChannel(It.IsAny<Model.ChannelType>())).Returns(listenermock.Object);

            var factory = factorymock.Object;

            var sut = new ListenerContextCreator(factory, new NullLogger());

            var listenercontext = sut.Create(new Model.Channel(Model.ChannelType.PointToPoint, null, null, "path"));

            factorymock.Verify(x => x.CreateListenerChannel(It.IsAny<Model.ChannelType>()), Times.Once);

            listenercontext.Channel.ShouldNotBeNull();

            listenercontext.ListenerChannel.ShouldNotBeNull();

            listenercontext.ListenerChannel.ShouldBeAssignableTo<IListenerChannel>();

            listenercontext.Channel.Path.ShouldBe("path");

            listenercontext.Partition.ShouldBeNull();
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
