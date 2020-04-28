using Jal.ChainOfResponsability;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class BusTests
    {
        [TestMethod]
        public async Task Bus_With_ShouldBeExecuted()
        {
            var endpointprovidermock = Builder.CreateEnpointProvider();

            var factorymock = Builder.CreateFactoryMock();

            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { Builder.CreateChannel() });

            var lifecyclemock = new Mock<ISenderContextLifecycle>();

            lifecyclemock.Setup(x => x.Get(It.IsAny<Channel>())).Returns(Builder.CreateSenderContext());

            factorymock.Setup(m => m.CreateChannelShuffler()).Returns(shuffermock.Object);

            var pipelinemock = Builder.CreatePipelineMock();

            var messagecontext = Builder.CreateMessageContextFromListen();

            messagecontext.Route.Middlewares.Add(typeof(string));

            var factory = factorymock.Object;

            factory.Configuration.OutboundMiddlewareTypes.Add(typeof(string));

            var sut = new Bus(endpointprovidermock.Object, factory, new PipelineBuilder(pipelinemock.Object), lifecyclemock.Object, new NullLogger());

            var options = Options.CreateEmpty("endpointname");

            await sut.Send(new object(), options);

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<AsyncMiddlewareConfiguration<MessageContext>[]>(), It.IsAny<MessageContext>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task Bus_WithError_ShouldThrowException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { Builder.CreateChannel() });

            var lifecyclemock = new Mock<ISenderContextLifecycle>();

            lifecyclemock.Setup(x => x.Get(It.IsAny<Channel>())).Returns(Builder.CreateSenderContext());

            factorymock.Setup(m => m.CreateChannelShuffler()).Returns(shuffermock.Object);

            var pipelinemock = Builder.CreatePipelineMock(true);

            var messagecontext = Builder.CreateMessageContextFromListen();

            var endpointprovidermock = Builder.CreateEnpointProvider();

            var sut = new Bus(endpointprovidermock.Object, factorymock.Object, new PipelineBuilder(pipelinemock.Object), lifecyclemock.Object, new NullLogger());

            var options = Options.CreateEmpty("endpointname");

            await Should.ThrowAsync<Exception>(sut.Send(new object(), options));

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<AsyncMiddlewareConfiguration<MessageContext>[]>(), It.IsAny<MessageContext>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
