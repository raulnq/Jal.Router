using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

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

            factorymock.Setup(x => x.CreatePointToPointChannel()).Returns(channelmock.Object);

            var sut = new SenderContextLifecycle(factorymock.Object, new NullLogger());

            var sendercontext = sut.Add(Builder.CreateChannel(ChannelType.PointToPoint));

            factorymock.Verify(x => x.CreatePointToPointChannel(), Times.Once);

            sendercontext.SenderChannel.ShouldNotBeNull();

            sendercontext.SenderChannel.ShouldBeAssignableTo<IPointToPointChannel>();

            sendercontext.ReaderChannel.ShouldBeNull();

            sendercontext.Channel.ShouldNotBeNull();

            sendercontext.Channel.Path.ShouldBe("path");

            sendercontext.Channel.Type.ShouldBe(ChannelType.PointToPoint);
        }

        [TestMethod]
        public void Add_WithPublishSubscribeChannel_ShouldBeCreated()
        {
            var channelmock = new Mock<IPublishSubscribeChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreatePublishSubscribeChannel()).Returns(channelmock.Object);

            var sut = new SenderContextLifecycle(factorymock.Object, new NullLogger());

            var sendercontext = sut.Add(Builder.CreateChannel(ChannelType.PublishSubscribe));

            factorymock.Verify(x => x.CreatePublishSubscribeChannel(), Times.Once);

            sendercontext.SenderChannel.ShouldNotBeNull();

            sendercontext.SenderChannel.ShouldBeAssignableTo<IPublishSubscribeChannel>();

            sendercontext.ReaderChannel.ShouldBeNull();

            sendercontext.Channel.ShouldNotBeNull();

            sendercontext.Channel.Path.ShouldBe("path");

            sendercontext.Channel.Subscription.ShouldBe("subscription");

            sendercontext.Channel.Type.ShouldBe(ChannelType.PublishSubscribe);
        }

        [TestMethod]
        public void Add_WithRequestReplyToPointToPointChannel_ShouldBeCreated()
        {
            var channelmock = new Mock<IRequestReplyChannelFromPointToPointChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateRequestReplyChannelFromPointToPointChannel()).Returns(channelmock.Object);

            var sut = new SenderContextLifecycle(factorymock.Object, new NullLogger());

            var sendercontext = sut.Add(Builder.CreateChannel(ChannelType.RequestReplyToPointToPoint));

            factorymock.Verify(x => x.CreateRequestReplyChannelFromPointToPointChannel(), Times.Once);

            sendercontext.SenderChannel.ShouldNotBeNull();

            sendercontext.SenderChannel.ShouldBeAssignableTo<IRequestReplyChannelFromPointToPointChannel>();

            sendercontext.ReaderChannel.ShouldNotBeNull();

            sendercontext.ReaderChannel.ShouldBeAssignableTo<IRequestReplyChannelFromPointToPointChannel>();

            sendercontext.Channel.ShouldNotBeNull();

            sendercontext.Channel.Path.ShouldBe("path");

            sendercontext.Channel.Type.ShouldBe(ChannelType.RequestReplyToPointToPoint);
        }

        [TestMethod]
        public void Add_WithRequestReplyToSubscriptionToPublishSubscribeChannel_ShouldBeCreated()
        {
            var channelmock = new Mock<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateRequestReplyFromSubscriptionToPublishSubscribeChannel()).Returns(channelmock.Object);

            var sut = new SenderContextLifecycle(factorymock.Object, new NullLogger());

            var sendercontext = sut.Add(Builder.CreateChannel(ChannelType.RequestReplyToSubscriptionToPublishSubscribe));

            factorymock.Verify(x => x.CreateRequestReplyFromSubscriptionToPublishSubscribeChannel(), Times.Once);

            sendercontext.SenderChannel.ShouldNotBeNull();

            sendercontext.SenderChannel.ShouldBeAssignableTo<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();

            sendercontext.ReaderChannel.ShouldNotBeNull();

            sendercontext.ReaderChannel.ShouldBeAssignableTo<IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel>();

            sendercontext.Channel.ShouldNotBeNull();

            sendercontext.Channel.Path.ShouldBe("path");

            sendercontext.Channel.Type.ShouldBe(ChannelType.RequestReplyToSubscriptionToPublishSubscribe);
        }
    }
}
