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

            var messagecontext = Builder.CreateMessageContext();

            var senderchannelmock = new Mock<ISenderChannel>();

            var factory = factorymock.Object;

            lifecyclemock.Setup(x => x.Get(It.IsAny<Channel>())).Returns(new SenderContext(new EndPoint("name"), Builder.CreateChannel(), senderchannelmock.Object, null, factory.CreateMessageAdapter(It.IsAny<Type>()), factory.CreateMessageSerializer(), factory.CreateMessageStorage()));

            var sut = Build(lifecyclemock.Object);

            await sut.Produce(messagecontext);

            senderchannelmock.WasExecuted();


        }
    }
}
