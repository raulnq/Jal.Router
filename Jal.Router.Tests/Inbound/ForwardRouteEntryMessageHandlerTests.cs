using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{

    [TestClass]
    public class ForwardRouteEntryMessageHandlerTests
    {
        private ForwardRouteEntryMessageHandler Build(IComponentFactoryFacade factory)
        {
            return new ForwardRouteEntryMessageHandler(new NullLogger());
        }

        [TestMethod]
        public async Task Handle_WithNoEndpoint_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var messagecontext = Builder.CreateMessageContextFromListen();

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { }));

            factorymock.CreateMessageSerializerWasNotExecuted();
        }

        [TestMethod]
        public async Task Handle_WithEmptyEndpoint_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var messagecontext = Builder.CreateMessageContextFromListen();

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { { "endpoint", "" } }));

            factorymock.CreateMessageSerializerWasNotExecuted();
        }

        [TestMethod]
        public async Task Handle_WithEndpoint_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var busmock = new Mock<IBus>();

            var messagecontext = Builder.CreateMessageContextFromListen(bus: busmock.Object);

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { { "endpoint", "queue" }, { "type", typeof(object) } }));

            factorymock.CreateMessageSerializerWasExecuted();

            busmock.SendWasExecuted<object>(op => op.EndPointName == "queue");
        }
    }
}