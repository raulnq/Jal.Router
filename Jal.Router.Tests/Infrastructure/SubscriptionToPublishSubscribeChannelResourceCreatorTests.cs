using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests.Infrastructure
{
    [TestClass]
    public class SubscriptionToPublishSubscribeChannelResourceCreatorTests
    {
        private SubscriptionToPublishSubscribeChannelResourceCreator Build(IComponentFactoryGateway factory)
        {
            return new SubscriptionToPublishSubscribeChannelResourceCreator(factory, new NullLogger());
        }

        [TestMethod]
        [DataRow("","","")]
        [DataRow("subscription", "", "")]
        [DataRow("", "path", "")]
        [DataRow("", "", "connectionstring")]
        public async Task Run_WithMissingData_ShouldThrowException(string subscription, string path, string connectionstring)
        {
            var channelresource = new Mock<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSubscriptionToPublishSubscribeChannelResource()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new SubscriptionToPublishSubscribeChannelResource(subscription, path, connectionstring, new Dictionary<string, string>());

            resource.Rules.Add(new SubscriptionToPublishSubscribeChannelRule("", "", true));

            factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannelResources.Add(resource);

            var sut = Build(factory);

            await Should.ThrowAsync<ApplicationException>(()=>sut.Run());

            factorymock.Verify(x => x.CreateSubscriptionToPublishSubscribeChannelResource(), Times.Once);

            channelresource.Verify(x => x.CreateIfNotExist(It.IsAny<SubscriptionToPublishSubscribeChannelResource>()), Times.Never);
        }

        [TestMethod]
        public async Task Run_WithValidResource_ShouldBeCreated()
        {
            var channelresource = new Mock<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSubscriptionToPublishSubscribeChannelResource()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new SubscriptionToPublishSubscribeChannelResource("subscription", "path", "connectionstring", new Dictionary<string, string>());

            resource.Rules.Add(new SubscriptionToPublishSubscribeChannelRule("", "", true));

            factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannelResources.Add(resource);

            var sut = Build(factory);

            await sut.Run();

            factorymock.Verify(x => x.CreateSubscriptionToPublishSubscribeChannelResource(), Times.Once);

            channelresource.Verify(x => x.CreateIfNotExist(It.IsAny<SubscriptionToPublishSubscribeChannelResource>()), Times.Once);
        }

        [TestMethod]
        public async Task Run_WithProvider_ShouldBeCreated()
        {
            var channelresource = new Mock<IChannelResource<SubscriptionToPublishSubscribeChannelResource, SubscriptionToPublishSubscribeChannelStatistics>>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateSubscriptionToPublishSubscribeChannelResource()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new SubscriptionToPublishSubscribeChannelResource("subscription", "path", "connectionstring", new Dictionary<string, string>());

            resource.Rules.Add(new SubscriptionToPublishSubscribeChannelRule("", "", true));

            factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannelResources.Add(resource);

            var sut = Build(factory);

            await sut.Run();

            factorymock.Verify(x => x.CreateSubscriptionToPublishSubscribeChannelResource(), Times.Once);

            channelresource.Verify(x => x.CreateIfNotExist(It.IsAny<SubscriptionToPublishSubscribeChannelResource>()), Times.Once);
        }
    }
}
