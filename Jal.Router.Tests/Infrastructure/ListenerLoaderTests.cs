using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class ListenerLoaderTests
    {
        [TestMethod]
        public async Task Run_WithRoute_ShouldBeLoaded()
        {
            var lifecyclemock = new Mock<IListenerContextLifecycle>();

            lifecyclemock.Setup(x => x.AddOrGet(It.IsAny<Channel>())).Returns(Builder.CreateListenerContext());

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var router = Builder.CreateRoute();

            router.Channels.Add(Builder.CreateChannel());

            factory.Configuration.Runtime.Routes.Add(router);

            var sut = new ListenerLoader(factory, new NullLogger(), lifecyclemock.Object);

            await sut.Run();

            lifecyclemock.Verify(x => x.AddOrGet(It.IsAny<Channel>()), Times.Once);
        }
    }
}
