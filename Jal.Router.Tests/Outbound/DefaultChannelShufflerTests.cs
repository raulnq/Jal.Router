using Jal.Router.Impl;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Jal.Router.Tests
{
    [TestClass]
    public class DefaultChannelShufflerTests
    {
        [TestMethod]
        public void Shuffle_With_ShouldBeTheSame()
        {
            var sut = new DefaultChannelShuffler();

            var channels = sut.Shuffle(new Channel[] { Builder.CreateChannel(), Builder.CreateChannel(ChannelType.PublishSubscribe, subscription: "subscription") });

            channels.ShouldNotBeNull();

            channels.ShouldNotBeEmpty();

            channels.Length.ShouldBe(2);

            channels[0].Type.ShouldBe(ChannelType.PointToPoint);

            channels[1].Type.ShouldBe(ChannelType.PublishSubscribe);
        }
    }
}
