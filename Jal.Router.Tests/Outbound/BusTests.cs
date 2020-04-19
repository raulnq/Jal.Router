using Jal.ChainOfResponsability;
using Jal.Router.Impl;
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

            var pipelinemock = Builder.CreatePipelineMock();

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.Route.MiddlewareTypes.Add(typeof(string));

            var factory = factorymock.Object;

            factory.Configuration.OutboundMiddlewareTypes.Add(typeof(string));

            var sut = new Bus(endpointprovidermock.Object, factory, new PipelineBuilder(pipelinemock.Object));

            await sut.Send(new object(), new Options("endpointname", new System.Collections.Generic.Dictionary<string, string>() { }, messagecontext.SagaContext, messagecontext.TrackingContext, messagecontext.TracingContext, messagecontext.Route, messagecontext.Saga, messagecontext.Version, null));

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<AsyncMiddlewareConfiguration<MessageContext>[]>(), It.IsAny<MessageContext>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestMethod]
        public async Task Bus_WithError_ShouldThrowException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var pipelinemock = Builder.CreatePipelineMock(true);

            var messagecontext = Builder.CreateMessageContext();

            var endpointprovidermock = Builder.CreateEnpointProvider();

            var sut = new Bus(endpointprovidermock.Object, factorymock.Object, new PipelineBuilder(pipelinemock.Object));

            await Should.ThrowAsync<Exception>(sut.Send(new object(), new Options("endpointname", new System.Collections.Generic.Dictionary<string, string>() { }, messagecontext.SagaContext, messagecontext.TrackingContext, messagecontext.TracingContext, messagecontext.Route, messagecontext.Saga, messagecontext.Version, null)));

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<AsyncMiddlewareConfiguration<MessageContext>[]>(), It.IsAny<MessageContext>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
