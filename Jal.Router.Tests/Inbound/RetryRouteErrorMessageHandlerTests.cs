using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class RetryRouteErrorMessageHandlerTests
    {
        private RetryRouteErrorMessageHandler Build(IComponentFactoryFacade factory)
        {
            return new RetryRouteErrorMessageHandler(new NullLogger());
        }

        [TestMethod]
        public async Task Handle_WithNoEndpoint_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var messagecontext = Builder.CreateMessageContextFromListen();

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { }, false));

            factorymock.CreateMessageSerializerWasNotExecuted();
        }

        [TestMethod]
        public async Task Handle_WithEmptyEndpoint_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var messagecontext = Builder.CreateMessageContextFromListen();

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "" } }, false));

            factorymock.CreateMessageSerializerWasNotExecuted();
        }

        [TestMethod]
        public async Task Handle_WithNoPolicy_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var messagecontext = Builder.CreateMessageContextFromListen();

            var factory = factorymock.Object;

            var sut = Build(factory);

            await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "queue" } }, false));

            factorymock.CreateMessageSerializerWasNotExecuted();
        }

        [TestMethod]
        public async Task Handle_WithEndpointAndPolicyCanRetry_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var busmock = new Mock<IBus>();

            var policymock = new Mock<IRetryPolicy>();

            policymock.Setup(x => x.CanRetry(It.IsAny<int>())).Returns(true);

            policymock.Setup(x => x.NextRetryInterval(It.IsAny<int>())).Returns(TimeSpan.FromSeconds(5));

            var messagecontext = Builder.CreateMessageContextFromListen(bus: busmock.Object);

            var factory = factorymock.Object;

            var sut = Build(factory);

            var handled = await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "queue" }, { "policy", policymock.Object }, { "type", typeof(object) } }, true));

            factorymock.CreateMessageSerializerWasExecuted();

            busmock.SendWasExecuted<object>(op => op.Headers.Any(x=>x.Key.Contains("_retrycount")) && op.ScheduledEnqueueDateTimeUtc!=null && op.EndPointName== "queue");

            handled.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Handle_WithEndpointAndPolicyCannotRetryNoFallback_ShouldNotBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var busmock = new Mock<IBus>();

            var policymock = new Mock<IRetryPolicy>();

            policymock.Setup(x => x.CanRetry(It.IsAny<int>())).Returns(false);

            var messagecontext = Builder.CreateMessageContextFromListen(bus: busmock.Object);

            var factory = factorymock.Object;

            var sut = Build(factory);

            var handled = await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "queue" }, { "policy", policymock.Object } }, true));

            factorymock.CreateMessageSerializerWasNotExecuted();

            busmock.SendWasNotExecuted<object>();

            handled.ShouldBeFalse();
        }

        [TestMethod]
        public async Task Handle_WithEndpointAndPolicyCannotRetryWithFallback_ShouldNBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var busmock = new Mock<IBus>();

            var policymock = new Mock<IRetryPolicy>();

            policymock.Setup(x => x.CanRetry(It.IsAny<int>())).Returns(false);

            var executed = false;

            Func<MessageContext, Exception, ErrorHandler, Task> fallback = (mc, e, eh) => { executed = true; return Task.CompletedTask; };

            var messagecontext = Builder.CreateMessageContextFromListen(bus: busmock.Object);

            var factory = factorymock.Object;

            var sut = Build(factory);

            var handled = await sut.Handle(messagecontext, new System.Exception(), new Model.ErrorHandler(typeof(object), new Dictionary<string, object>() { { "endpoint", "queue" }, { "policy", policymock.Object }, { "fallback", fallback }, { "type", typeof(object) } }, true));

            factorymock.CreateMessageSerializerWasNotExecuted();

            busmock.SendWasNotExecuted<object>();

            handled.ShouldBeTrue();

            executed.ShouldBeTrue();
        }

    }
    
}