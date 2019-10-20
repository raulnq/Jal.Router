using Jal.ChainOfResponsability.Fluent.Impl;
using Jal.ChainOfResponsability.Fluent.Interfaces;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class RouterTests
    {
        [TestMethod]
        public async Task Route_WithNullWhen_ShouldBeExecuted()
        {
            var factorymock = CreateFactoryMock();

            var pipelinemock = CreatePipelineMock();

            var messagecontext = CreateMessageContext(CreateRoute());

            var sut = new Impl.Router(factorymock.Object, new PipelineBuilder(pipelinemock.Object), new NullLogger());

            await sut.Route<NullMiddleware>(messagecontext);

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>()), Times.Once());
        }

        [TestMethod]
        public async Task Route_WithWhenEqualsFalse_ShouldNotBeExecuted()
        {
            var factorymock = CreateFactoryMock();

            var pipelinemock = CreatePipelineMock();

            var messagecontext = CreateMessageContext(CreateRoute());

            messagecontext.Route.UpdateWhen(x => false);

            var sut = new Impl.Router(factorymock.Object, new PipelineBuilder(pipelinemock.Object), new NullLogger());

            await sut.Route<NullMiddleware>(messagecontext);

            pipelinemock.Verify(mock => mock.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>()), Times.Never());
        }

        private static Mock<IPipeline> CreatePipelineMock()
        {
            var pipelinemock = new Mock<IPipeline>();

            pipelinemock.Setup(x => x.ExecuteAsync(It.IsAny<MiddlewareMetadata<MessageContext>[]>(), It.IsAny<MessageContext>())).Returns(Task.CompletedTask);

            return pipelinemock;
        }

        private static Mock<IComponentFactoryGateway> CreateFactoryMock()
        {
            var factorymock = new Mock<IComponentFactoryGateway>();

            factorymock.Setup(m => m.CreateRouterInterceptor()).Returns(new NullRouterInterceptor());

            factorymock.Setup(m => m.Configuration).Returns(new Configuration());

            return factorymock;
        }

        public Route CreateRoute()
        {
            return new Route("route", typeof(string), typeof(string), new List<Channel>());
        }

        public MessageContext CreateMessageContext(Route route)
        {
            var mock = new Mock<IBus>();

            var messagecontext = new MessageContext(mock.Object);

            messagecontext.UpdateRoute(route);

            return messagecontext;
        }
    }
}
