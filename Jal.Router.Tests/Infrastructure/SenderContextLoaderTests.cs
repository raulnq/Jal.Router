using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace Jal.Router.Tests
{
    [TestClass]
    public class SenderContextLoaderTests
    {
        [TestMethod]
        public void Add_With_ShouldBeAdded()
        {
            var creatormock = new Mock<ISenderContextLifecycle>();

            creatormock.Setup(x => x.Add(It.IsAny<EndPoint>(), It.IsAny<Channel>())).Returns(Builder.CreateSenderContext());

            var sut = new SenderContextLoader(creatormock.Object, new NullLogger());

            var endpoint = new EndPoint("name");

            var channel = Builder.CreateChannel();

            endpoint.Channels.Add(channel);

            sut.Add(endpoint);

            creatormock.Verify(x => x.Add(It.IsAny<EndPoint>(), It.IsAny<Channel>()), Times.Once);

            creatormock.Verify(x => x.Get(It.IsAny<Channel>()), Times.Once);
        }
    }
}
