using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace Jal.Router.Tests.Infrastructure
{
    [TestClass]
    public class SenderContextLoaderTests
    {
        [TestMethod]
        public void AddPointToPointChannel_With_ShouldBeAdded()
        {
            var creatormock = new Mock<ISenderContextCreator>();

            creatormock.Setup(x => x.Create(It.IsAny<Channel>())).Returns(Builder.CreateSenderContext());

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var sut = new SenderContextLoader(creatormock.Object, factory);

            sut.AddPointToPointChannel<object>("name","connectionstring","path");

            Verify(creatormock, factory, ChannelType.PointToPoint);
        }

        [TestMethod]
        public void AddPublishSubscribeChannel_With_ShouldBeAdded()
        {
            var creatormock = new Mock<ISenderContextCreator>();

            creatormock.Setup(x => x.Create(It.IsAny<Channel>())).Returns(Builder.CreateSenderContext());

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var sut = new SenderContextLoader(creatormock.Object, factory);

            sut.AddPublishSubscribeChannel<object>("name", "connectionstring", "path");

            Verify(creatormock, factory, ChannelType.PublishSubscribe);
        }

        private static void Verify(Mock<ISenderContextCreator> creatormock, IComponentFactoryGateway factory, ChannelType channeltype)
        {
            creatormock.Verify(x => x.Create(It.IsAny<Channel>()), Times.Once);

            creatormock.Verify(x => x.Open(It.IsAny<SenderContext>()), Times.Once);

            factory.Configuration.Runtime.SenderContexts.ShouldNotBeEmpty();

            factory.Configuration.Runtime.SenderContexts[0].Endpoints.ShouldNotBeEmpty();

            factory.Configuration.Runtime.SenderContexts[0].Endpoints[0].Channels.ShouldNotBeEmpty();

            factory.Configuration.Runtime.SenderContexts[0].Endpoints[0].Name.ShouldBe("name");

            factory.Configuration.Runtime.SenderContexts[0].Endpoints[0].ContentType.ShouldBe(typeof(object));

            factory.Configuration.Runtime.SenderContexts[0].Endpoints[0].Channels[0].Type.ShouldBe(channeltype);

            factory.Configuration.Runtime.SenderContexts[0].Endpoints[0].Channels[0].Path.ShouldBe("path");
        }
    }
}
