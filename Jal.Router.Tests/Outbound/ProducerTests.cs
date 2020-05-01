using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class ProducerTests
    {
        private Producer Build(ISenderContextLifecycle lifecycle)
        {
            return new Producer(new NullLogger(), lifecycle);
        }

        [TestMethod]
        public async Task Bus_With_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var lifecyclemock = new Mock<ISenderContextLifecycle>();

            var senderchannelmock = new Mock<ISenderChannel>();

            factorymock.Setup(x => x.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>())).Returns((senderchannelmock.Object, null, new NullPointToPointChannel()));

            var factory = factorymock.Object;

            var messagecontext = Builder.CreateMessageContextToSend(factory: factory);

            lifecyclemock.Setup(x => x.Get(It.IsAny<Channel>())).Returns(Builder.CreateSenderContext(factory));

            var sut = Build(lifecyclemock.Object);

            await sut.Produce(messagecontext);

            senderchannelmock.WasExecuted();


        }
    }
}
