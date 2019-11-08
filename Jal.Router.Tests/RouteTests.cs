using Jal.ChainOfResponsability.Fluent.Impl;
using Jal.ChainOfResponsability.Fluent.Interfaces;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Impl;
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
        [TestMethod]
        public async Task Route_WithoutWhen_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var pipelinemock = CreatePipelineMock();

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.Route.MiddlewareTypes.Add(typeof(string));

            var factory = factorymock.Object;

            factory.Configuration.InboundMiddlewareTypes.Add(typeof(string));

            var sut = new Impl.Router(factory, new PipelineBuilder(pipelinemock.Object), new NullLogger());

            await sut.Route<NullMiddleware>(messagecontext);

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>()), Times.Once());
        }

        [TestMethod]
        public async Task Route_WithWhenEqualsFalse_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var pipelinemock = CreatePipelineMock();

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.Route.UpdateWhen(x => false);

            var sut = new Impl.Router(factorymock.Object, new PipelineBuilder(pipelinemock.Object), new NullLogger());

            await sut.Route<NullMiddleware>(messagecontext);

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>()), Times.Never());
        }

        [TestMethod]
        public async Task Route_WithError_ShouldThrowException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var pipelinemock = CreatePipelineMock(true);

            var messagecontext = Builder.CreateMessageContext();

            var sut = new Impl.Router(factorymock.Object, new PipelineBuilder(pipelinemock.Object), new NullLogger());

            await Should.ThrowAsync<Exception>(sut.Route<NullMiddleware>(messagecontext));

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>()), Times.Once());
        }

        private static Mock<IPipeline> CreatePipelineMock(bool throwexception = false)
        {
            var pipelinemock = new Mock<IPipeline>();

            if (throwexception)
            {
                pipelinemock.Setup(x => x.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>())).Throws(new System.Exception());
            }
            else
            {
                pipelinemock.Setup(x => x.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>())).Returns(Task.CompletedTask);
            }

            return pipelinemock;
        }



    }

    public class Handler
    {

    }
}
