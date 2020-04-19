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
    public class PointToPointChannelResourceDestructorTests
    {
        private PointToPointChannelResourceDestructor Build(IComponentFactoryFacade factory)
        {
            return new PointToPointChannelResourceDestructor(factory, new NullLogger());
        }

        [TestMethod]

        public async Task Run_WithError_ShouldThrowException()
        {
            var channelresource = new Mock<IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>>();

            channelresource.Setup(x => x.DeleteIfExist(It.IsAny<PointToPointChannelResource>())).Throws(new Exception());

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreatePointToPointChannelResourceManager()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new PointToPointChannelResource("path", "connectionstring", new Dictionary<string, string>());

            factory.Configuration.Runtime.PointToPointChannelResources.Add(resource);

            var sut = Build(factory);

            await Should.ThrowAsync<ApplicationException>(() => sut.Run());

            factorymock.Verify(x => x.CreatePointToPointChannelResourceManager(), Times.Once);

            channelresource.Verify(x => x.DeleteIfExist(It.IsAny<PointToPointChannelResource>()), Times.Once);
        }

        [TestMethod]
        public async Task Run_With_ShouldBeCreated()
        {
            var channelresource = new Mock<IChannelResourceManager<PointToPointChannelResource, PointToPointChannelStatistics>>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreatePointToPointChannelResourceManager()).Returns(channelresource.Object);

            var factory = factorymock.Object;

            var resource = new PointToPointChannelResource("path", "connectionstring", new Dictionary<string, string>());

            factory.Configuration.Runtime.PointToPointChannelResources.Add(resource);

            var sut = Build(factory);

            await sut.Run();

            factorymock.Verify(x => x.CreatePointToPointChannelResourceManager(), Times.Once);

            channelresource.Verify(x => x.DeleteIfExist(It.IsAny<PointToPointChannelResource>()), Times.Once);
        }
    }
}
