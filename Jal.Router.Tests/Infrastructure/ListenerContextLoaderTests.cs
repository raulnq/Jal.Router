using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace Jal.Router.Tests.Infrastructure
{
    [TestClass]
    public class ListenerContextLoaderTests
    {
        [TestMethod]
        public void AddPointToPointChannel_With_ShouldBeAdded()
        {
            var creatormock = new Mock<IListenerContextCreator>();

            creatormock.Setup(x => x.Create(It.IsAny<Channel>())).Returns(new ListenerContext(null, null, null));

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var sut = new ListenerContextLoader(creatormock.Object, factory);

            sut.AddPointToPointChannel<object, object, object>("name", "connectionstring", "path");

            Verify(creatormock, factory, ChannelType.PointToPoint);
        }

        [TestMethod]
        public void AddPublishSubscribeChannel_With_ShouldBeAdded()
        {
            var creatormock = new Mock<IListenerContextCreator>();

            creatormock.Setup(x => x.Create(It.IsAny<Channel>())).Returns(new ListenerContext(null, null, null));

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var sut = new ListenerContextLoader(creatormock.Object, factory);

            sut.AddPublishSubscribeChannel<object, object, object>("name", "connectionstring", "path", "subscription");

            Verify(creatormock, factory, ChannelType.PublishSubscribe);

            factory.Configuration.Runtime.ListenerContexts[0].Routes[0].Channels[0].Subscription.ShouldBe("subscription");
        }

        private static void Verify(Mock<IListenerContextCreator> creatormock, IComponentFactoryGateway factory, ChannelType channeltype)
        {
            creatormock.Verify(x => x.Create(It.IsAny<Channel>()), Times.Once);

            creatormock.Verify(x => x.Open(It.IsAny<ListenerContext>()), Times.Once);

            factory.Configuration.Runtime.ListenerContexts.ShouldNotBeEmpty();

            factory.Configuration.Runtime.ListenerContexts[0].Routes.ShouldNotBeEmpty();

            factory.Configuration.Runtime.ListenerContexts[0].Routes[0].Channels.ShouldNotBeEmpty();

            factory.Configuration.Runtime.ListenerContexts[0].Routes[0].Name.ShouldBe("name");

            factory.Configuration.Runtime.ListenerContexts[0].Routes[0].ContentType.ShouldBe(typeof(object));

            factory.Configuration.Runtime.ListenerContexts[0].Routes[0].Channels[0].ConnectionStringProvider.ShouldNotBeNull();

            factory.Configuration.Runtime.ListenerContexts[0].Routes[0].Channels[0].Type.ShouldBe(channeltype);

            factory.Configuration.Runtime.ListenerContexts[0].Routes[0].Channels[0].Path.ShouldBe("path");
        }
    }
}
