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
            var creatormock = new Mock<IListenerContextCreator>();

            creatormock.Setup(x => x.Create(It.IsAny<Channel>())).Returns(Builder.CreateListenerContext());

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var router = Builder.CreateRoute();

            router.Channels.Add(Builder.CreateChannel());

            factory.Configuration.Runtime.Routes.Add(router);

            var sut = new ListenerLoader(factory, new NullLogger(), creatormock.Object);

            await sut.Run();

            factory.Configuration.Runtime.ListenerContexts.Count.ShouldBe(1);

            factory.Configuration.Runtime.ListenerContexts[0].Routes.Count.ShouldBe(1);

            creatormock.Verify(x => x.Create(It.IsAny<Channel>()), Times.Once);

            creatormock.Verify(x => x.Open(It.IsAny<ListenerContext>()), Times.Once);
        }
    }
}
