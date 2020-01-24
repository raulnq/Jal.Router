using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class SubscriptionToPublishSubscribeChannelResourceCreatorTests
    {
        private SubscriptionToPublishSubscribeChannelResourceCreator Build(IComponentFactoryGateway factory)
        {
            return new SubscriptionToPublishSubscribeChannelResourceCreator(factory, new NullLogger());
        }

        [TestMethod]
        public async Task Run_WithError_ShouldThrowException()
        {
            var channelresource = new Mock<IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>();

            channelresource.Setup(x => x.CreateIfNotExist(It.IsAny<SubscriptionToPublishSubscribeChannelResource>())).Throws(new Exception());

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSubscriptionToPublishSubscribeChannelResourceManager()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new SubscriptionToPublishSubscribeChannelResource("subscription", "path", "connectionstring", new Dictionary<string, string>());

            resource.Rules.Add(new SubscriptionToPublishSubscribeChannelResourceRule("", "", true));

            factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannelResources.Add(resource);

            var sut = Build(factory);

            await Should.ThrowAsync<ApplicationException>(()=>sut.Run());

            factorymock.Verify(x => x.CreateSubscriptionToPublishSubscribeChannelResourceManager(), Times.Once);

            channelresource.Verify(x => x.CreateIfNotExist(It.IsAny<SubscriptionToPublishSubscribeChannelResource>()), Times.Once);
        }

        [TestMethod]
        public async Task Run_With_ShouldBeCreated()
        {
            var channelresource = new Mock<IChannelResourceManager<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSubscriptionToPublishSubscribeChannelResourceManager()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new SubscriptionToPublishSubscribeChannelResource("subscription", "path", "connectionstring", new Dictionary<string, string>());

            resource.Rules.Add(new SubscriptionToPublishSubscribeChannelResourceRule("", "", true));

            factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannelResources.Add(resource);

            var sut = Build(factory);

            await sut.Run();

            factorymock.Verify(x => x.CreateSubscriptionToPublishSubscribeChannelResourceManager(), Times.Once);

            channelresource.Verify(x => x.CreateIfNotExist(It.IsAny<SubscriptionToPublishSubscribeChannelResource>()), Times.Once);
        }
    }
}
