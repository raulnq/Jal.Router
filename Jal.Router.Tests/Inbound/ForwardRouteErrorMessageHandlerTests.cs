using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{

    [TestClass]
    public class ForwardRouteErrorMessageHandlerTests
    {
        private ForwardRouteErrorMessageHandler Build(IComponentFactoryFacade factory)
        {
            return new ForwardRouteErrorMessageHandler(factory, new NullLogger());
        }

        [TestMethod]
        public async Task Handle_WithNoEndpoint_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var messagecontext = Builder.CreateMessageContext();

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { }, false));

            factorymock.CreateMessageSerializerWasNotExecuted();
        }

        [TestMethod]
        public async Task Handle_WithEmptyEndpoint_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var messagecontext = Builder.CreateMessageContext();

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "" } }, false));

            factorymock.CreateMessageSerializerWasNotExecuted();
        }

        [TestMethod]
        public async Task Handle_WithNoException_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var messagecontext = Builder.CreateMessageContext();

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, null, new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "" } }, false));

            factorymock.CreateMessageSerializerWasNotExecuted();
        }

        [TestMethod]
        public async Task Handle_WithEndpointAndException_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var busmock = new Mock<IBus>();

            var messagecontext = Builder.CreateMessageContext(busmock: busmock);

            var factory = factorymock.Object;

            var sut = Build(factory);

            var handled = await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "queue" } }, true));

            factorymock.CreateMessageSerializerWasExecuted();

            busmock.SendWasExecuted<object>(op=>op.Headers.ContainsKey("exceptionmessage") && op.Headers.ContainsKey("exceptionstacktrace") && op.EndPointName== "queue");

            handled.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Handle_WithEndpointAndInnerException_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var busmock = new Mock<IBus>();

            var messagecontext = Builder.CreateMessageContext(busmock: busmock);

            var factory = factorymock.Object;

            var sut = Build(factory);

            var handled = await sut.Handle(messagecontext, new System.Exception("exception message",new System.Exception()), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "queue" } }, false));

            factorymock.CreateMessageSerializerWasExecuted();

            busmock.SendWasExecuted<object>(op => op.Headers.ContainsKey("exceptionmessage") && op.Headers.ContainsKey("exceptionstacktrace") && op.Headers.ContainsKey("innerexceptionmessage") && op.Headers.ContainsKey("innerexceptionstacktrace") && op.EndPointName == "queue");

            handled.ShouldBeFalse();
        }

    }
    
}