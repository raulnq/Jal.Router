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
using Jal.Router.Extensions;

namespace Jal.Router.Tests
{
    [TestClass]
    public class BusTests
    {
        [TestMethod]
        public async Task Bus_With_ShouldBeExecuted()
        {
            var endpointprovidermock = Builder.CreateEnpointProviderMock();

            var factorymock = Builder.CreateFactoryMock();

            var channel = Builder.CreateChannel();

            factorymock.AddChannelShuffler(channel);

            var pipelinemock = Builder.CreatePipelineMock();

            var messagecontext = Builder.CreateMessageContextFromListen();

            var factory = factorymock.Object;

            var lifecyclemock = Builder.CreateSenderContextLifecycleMock(factory: factory, channel: channel);

            factory.Configuration.EndpointMiddlewareTypes.Add(typeof(string));

            var sut = Build(endpointprovidermock, pipelinemock, factory, lifecyclemock);

            var options = messagecontext.CreateOptions("endpoint");

            await sut.Send(new object(), options);

            pipelinemock.WasExecuted();
        }

        private static Bus Build(Mock<IEndPointProvider> endpointprovidermock, Mock<IPipeline> pipelinemock, IComponentFactoryFacade factory, Mock<ISenderContextLifecycle> lifecyclemock)
        {
            return new Bus(endpointprovidermock.Object, factory, new PipelineBuilder(pipelinemock.Object), lifecyclemock.Object, new NullLogger());
        }

        [TestMethod]
        public async Task Bus_WithError_ShouldThrowException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var channel = Builder.CreateChannel();

            factorymock.AddChannelShuffler(channel);

            var factory = factorymock.Object;

            var lifecyclemock = Builder.CreateSenderContextLifecycleMock(factory: factory, channel: channel);

            var pipelinemock = Builder.CreatePipelineMock(true);

            var messagecontext = Builder.CreateMessageContextFromListen();

            var endpointprovidermock = Builder.CreateEnpointProviderMock();

            var sut = Build(endpointprovidermock, pipelinemock, factory, lifecyclemock);

            var options = messagecontext.CreateOptions("endpoint");

            await Should.ThrowAsync<Exception>(sut.Send(new object(), options));

            pipelinemock.WasExecuted();
        }
    }
}
