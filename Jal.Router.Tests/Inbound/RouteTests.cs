using Jal.ChainOfResponsability.Fluent.Impl;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{

    [TestClass]
    public class RouterTests
    {
        private Impl.Router Build(IComponentFactoryGateway factory, IPipeline pipeline)
        {
            return new Impl.Router(factory, new PipelineBuilder(pipeline), new NullLogger());
        }

        [TestMethod]
        public async Task Route_WithoutWhen_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var pipelinemock = Builder.CreatePipelineMock();

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.Route.MiddlewareTypes.Add(typeof(string));

            var factory = factorymock.Object;

            factory.Configuration.InboundMiddlewareTypes.Add(typeof(string));

            var sut = Build(factory, pipelinemock.Object);

            await sut.Route<NullMiddleware>(messagecontext);

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>()), Times.Once());
        }

        [TestMethod]
        public async Task Route_WithWhenEqualsFalse_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var pipelinemock = Builder.CreatePipelineMock();

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.Route.UpdateWhen(x => false);

            var sut = Build(factorymock.Object, pipelinemock.Object);

            await sut.Route<NullMiddleware>(messagecontext);

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>()), Times.Never());
        }

        [TestMethod]
        public async Task Route_WithError_ShouldThrowException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var pipelinemock = Builder.CreatePipelineMock(true);

            var messagecontext = Builder.CreateMessageContext();

            var sut = Build(factorymock.Object, pipelinemock.Object);

            await Should.ThrowAsync<Exception>(sut.Route<NullMiddleware>(messagecontext));

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>()), Times.Once());
        }



    }

    public class Handler
    {

    }
}