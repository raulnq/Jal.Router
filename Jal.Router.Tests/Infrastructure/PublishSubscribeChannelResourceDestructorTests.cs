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
    public class PublishSubscribeChannelResourceDestructorTests
    {
        private PublishSubscribeChannelResourceDestructor Build(IComponentFactoryGateway factory)
        {
            return new PublishSubscribeChannelResourceDestructor(factory, new NullLogger());
        }

        [TestMethod]

        public async Task Run_WithError_ShouldThrowException()
        {
            var channelresource = new Mock<IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>();

            channelresource.Setup(x => x.DeleteIfExist(It.IsAny<PublishSubscribeChannelResource>())).Throws(new Exception());

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreatePublishSubscribeChannelResourceManager()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new PublishSubscribeChannelResource("path", "connectionstring", new Dictionary<string, string>());

            factory.Configuration.Runtime.PublishSubscribeChannelResources.Add(resource);

            var sut = Build(factory);

            await Should.ThrowAsync<ApplicationException>(() => sut.Run());

            factorymock.Verify(x => x.CreatePublishSubscribeChannelResourceManager(), Times.Once);

            channelresource.Verify(x => x.DeleteIfExist(It.IsAny<PublishSubscribeChannelResource>()), Times.Once);
        }

        [TestMethod]
        public async Task Run_With_ShouldBeCreated()
        {
            var channelresource = new Mock<IChannelResourceManager<PublishSubscribeChannelResource, PublishSubscribeChannelStatistics>>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreatePublishSubscribeChannelResourceManager()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new PublishSubscribeChannelResource("path", "connectionstring", new Dictionary<string, string>());

            factory.Configuration.Runtime.PublishSubscribeChannelResources.Add(resource);

            var sut = Build(factory);

            await sut.Run();

            factorymock.Verify(x => x.CreatePublishSubscribeChannelResourceManager(), Times.Once);

            channelresource.Verify(x => x.DeleteIfExist(It.IsAny<PublishSubscribeChannelResource>()), Times.Once);
        }
    }
}
