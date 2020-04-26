using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;

namespace Jal.Router.Tests
{
    [TestClass]
    public class SenderContextLifecycleTests
    {
        [TestMethod]
        public void Add_WithPointToPointChannel_ShouldBeCreated()
        {
            var channelmock = new Mock<IPointToPointChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>())).Returns((channelmock.Object,null));

            var sut = new SenderContextLifecycle(factorymock.Object);

            var sendercontext = sut.Add(new EndPoint("name"), Builder.CreateChannel(ChannelType.PointToPoint));

            factorymock.Verify(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>()), Times.Once);

            sendercontext.SenderChannel.ShouldNotBeNull();

            sendercontext.SenderChannel.ShouldBeAssignableTo<IPointToPointChannel>();

            sendercontext.ReaderChannel.ShouldBeNull();

            sendercontext.Channel.ShouldNotBeNull();

            sendercontext.Channel.Path.ShouldBe("path");

            sendercontext.Channel.ChannelType.ShouldBe(ChannelType.PointToPoint);
        }

        [TestMethod]
        public void Add_WithPublishSubscribeChannel_ShouldBeCreated()
        {
            var channelmock = new Mock<IPublishSubscribeChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>())).Returns((channelmock.Object, null));

            var sut = new SenderContextLifecycle(factorymock.Object);

            var sendercontext = sut.Add(new EndPoint("name"), Builder.CreateChannel(ChannelType.PublishSubscribe));

            factorymock.Verify(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>()), Times.Once);

            sendercontext.SenderChannel.ShouldNotBeNull();

            sendercontext.SenderChannel.ShouldBeAssignableTo<IPublishSubscribeChannel>();

            sendercontext.ReaderChannel.ShouldBeNull();

            sendercontext.Channel.ShouldNotBeNull();

            sendercontext.Channel.Path.ShouldBe("path");

            sendercontext.Channel.Subscription.ShouldBe("subscription");

            sendercontext.Channel.ChannelType.ShouldBe(ChannelType.PublishSubscribe);
        }

        [TestMethod]
        public void Add_WithRequestReplyToPointToPointChannel_ShouldBeCreated()
        {
            var channelmock = new Mock<IRequestReplyChannelFromPointToPointChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>())).Returns((channelmock.Object, channelmock.Object));

            var sut = new SenderContextLifecycle(factorymock.Object);

            var sendercontext = sut.Add(new EndPoint("name"), Builder.CreateChannel(ChannelType.RequestReplyToPointToPoint));

            factorymock.Verify(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>()), Times.Once);

            sendercontext.SenderChannel.ShouldNotBeNull();

            sendercontext.SenderChannel.ShouldBeAssignableTo<IRequestReplyChannelFromPointToPointChannel>();

            sendercontext.ReaderChannel.ShouldNotBeNull();

            sendercontext.ReaderChannel.ShouldBeAssignableTo<IRequestReplyChannelFromPointToPointChannel>();

            sendercontext.Channel.ShouldNotBeNull();

            sendercontext.Channel.Path.ShouldBe("path");

            sendercontext.Channel.ChannelType.ShouldBe(ChannelType.RequestReplyToPointToPoint);
        }

        [TestMethod]
        public void Add_WithRequestReplyToSubscriptionToPublishSubscribeChannel_ShouldBeCreated()
        {
            var channelmock = new Mock<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>())).Returns((channelmock.Object, channelmock.Object));

            var sut = new SenderContextLifecycle(factorymock.Object);

            var sendercontext = sut.Add(new EndPoint("name"), Builder.CreateChannel(ChannelType.RequestReplyToSubscriptionToPublishSubscribe));

            factorymock.Verify(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>()), Times.Once);

            sendercontext.SenderChannel.ShouldNotBeNull();

            sendercontext.SenderChannel.ShouldBeAssignableTo<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();

            sendercontext.ReaderChannel.ShouldNotBeNull();

            sendercontext.ReaderChannel.ShouldBeAssignableTo<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();

            sendercontext.Channel.ShouldNotBeNull();

            sendercontext.Channel.Path.ShouldBe("path");

            sendercontext.Channel.ChannelType.ShouldBe(ChannelType.RequestReplyToSubscriptionToPublishSubscribe);
        }
    }
}
