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
        private Producer Build(IComponentFactoryFacade factory, ISenderContextLifecycle lifecycle)
        {
            return new Producer(factory, new NullLogger(), lifecycle);
        }

        [TestMethod]
        public async Task Bus_With_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var lifecyclemock = new Mock<ISenderContextLifecycle>();

            var messagecontext = Builder.CreateMessageContext();

            var senderchannelmock = new Mock<ISenderChannel>();

            var factory = factorymock.Object;

            messagecontext.SetChannel(Builder.CreateChannel());

            lifecyclemock.Setup(x => x.Get(It.IsAny<Channel>())).Returns(new SenderContext(Builder.CreateChannel(), senderchannelmock.Object, null, factory.CreateMessageAdapter(It.IsAny<Type>())));

            var sut = Build(factory, lifecyclemock.Object);

            await sut.Produce(messagecontext);

            senderchannelmock.WasExecuted();


        }
    }
}
