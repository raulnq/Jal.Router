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
    public class ResourceCreatorTests
    {
        private ResourceCreator Build(IComponentFactoryFacade factory)
        {
            return new ResourceCreator(factory, new NullLogger());
        }

        [TestMethod]
        public async Task Run_With_ShouldBeCreated()
        {
            var resourcemock = new Mock<IResource>();

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var resource = new Resource(ChannelType.PointToPoint, "path", "connectionstring", new Dictionary<string, string>());

            var resourcecontext = new ResourceContext(resource, resourcemock.Object, new NullMessageSerializer());

            factory.Configuration.Runtime.ResourceContexts.Add(resourcecontext);

            var sut = Build(factory);

            await sut.Run();

            resourcemock.Verify(x => x.CreateIfNotExist(It.IsAny<ResourceContext>()), Times.Once);
        }
    }
}
