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

            var busmock = new Mock<IBus>();

            var factory = factorymock.Object;

            var messagecontext = Builder.CreateMessageContextFromListen(factory: factory, bus: busmock.Object);

            var sut = Build(factory);

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { }));

            busmock.SendWasNotExecuted<object>();
        }

        [TestMethod]
        public async Task Handle_WithEmptyEndpoint_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var busmock = new Mock<IBus>();

            var messagecontext = Builder.CreateMessageContextFromListen(factory: factory, bus: busmock.Object);

            var sut = Build(factory);

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { { "endpoint", "" } }));

            busmock.SendWasNotExecuted<object>();
        }

        [TestMethod]
        public async Task Handle_WithEndpoint_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var busmock = new Mock<IBus>();

            var factory = factorymock.Object;

            var messagecontext = Builder.CreateMessageContextFromListen(factory: factory, bus: busmock.Object);

            var sut = Build(factory);

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { { "endpoint", "queue" }, { "type", typeof(object) } }));

            busmock.SendWasExecuted<object>(op => op.EndPointName == "queue");
        }
    }
}