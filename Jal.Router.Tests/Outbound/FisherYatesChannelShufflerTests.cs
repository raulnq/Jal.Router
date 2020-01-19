using Jal.Router.Impl;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Jal.Router.Tests
{
    [TestClass]
    public class FisherYatesChannelShufflerTests
    {
        [TestMethod]
        public void Shuffle_With_ShouldBeTheSame()
        {
            var sut = new FisherYatesChannelShuffler();

            var channels = sut.Shuffle(new Channel[] { new Channel(ChannelType.PointToPoint, typeof(object), null, null, null), new Channel(ChannelType.PublishSubscribe, typeof(object), null, null, null) });

            channels.ShouldNotBeNull();

            channels.ShouldNotBeEmpty();

            channels.Length.ShouldBe(2);
        }
    }
}
