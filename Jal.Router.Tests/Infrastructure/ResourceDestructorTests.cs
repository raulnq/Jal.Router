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
    public class ResourceDestructorTests
    {
        private ResourceDestructor Build(IComponentFactoryFacade factory)
        {
            return new ResourceDestructor(factory, new NullLogger());
        }

        [TestMethod]

        public async Task Run_WithError_ShouldThrowException()
        {
            var resourcemanager = new Mock<IResourceManager>();

            resourcemanager.Setup(x => x.DeleteIfExist(It.IsAny<Resource>())).Throws(new Exception());

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateResourceManager(It.IsAny<ChannelType>())).Returns(resourcemanager.Object);

            var factory = factorymock.Object;

            var resource = new Resource(ChannelType.PointToPoint, "path", "connectionstring", new Dictionary<string, string>());

            factory.Configuration.Runtime.Resources.Add(resource);

            var sut = Build(factory);

            await Should.ThrowAsync<ApplicationException>(() => sut.Run());

            factorymock.Verify(x => x.CreateResourceManager(It.IsAny<ChannelType>()), Times.Once);

            resourcemanager.Verify(x => x.DeleteIfExist(It.IsAny<Resource>()), Times.Once);
        }

        [TestMethod]
        public async Task Run_With_ShouldBeCreated()
        {
            var resourcemanager = new Mock<IResourceManager>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateResourceManager(It.IsAny<ChannelType>())).Returns(resourcemanager.Object);

            var factory = factorymock.Object;

            var resource = new Resource(ChannelType.PointToPoint, "path", "connectionstring", new Dictionary<string, string>());

            factory.Configuration.Runtime.Resources.Add(resource);

            var sut = Build(factory);

            await sut.Run();

            factorymock.Verify(x => x.CreateResourceManager(It.IsAny<ChannelType>()), Times.Once);

            resourcemanager.Verify(x => x.DeleteIfExist(It.IsAny<Resource>()), Times.Once);
        }
    }
}
