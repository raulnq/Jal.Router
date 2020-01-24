using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class ProducerTests
    {
        private Producer Build(IComponentFactoryGateway factory, ISenderContextCreator creator)
        {
            return new Producer(factory, new NullLogger(), creator);
        }

        [TestMethod]
        public async Task Bus_With_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var creatormock = new Mock<ISenderContextCreator>();

            var messagecontext = Builder.CreateMessageContext();

            var senderchannelmock = new Mock<ISenderChannel>();

            messagecontext.SetChannel(Builder.CreateChannel());

            var factory = factorymock.Object;

            factory.Configuration.Runtime.SenderContexts.Add(new SenderContext(Builder.CreateChannel(), senderchannelmock.Object, null));

            var sut = Build(factory, creatormock.Object);

            await sut.Produce(messagecontext);

            factorymock.CreateMessageAdapterWasExecuted();

            senderchannelmock.WasExecuted();


        }
    }
}
