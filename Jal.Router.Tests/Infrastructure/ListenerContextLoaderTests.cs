using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace Jal.Router.Tests
{
    [TestClass]
    public class ListenerContextLoaderTests
    {
        [TestMethod]
        public void Add_With_ShouldBeAdded()
        {
            var lifecyclemock = new Mock<IListenerContextLifecycle>();

            lifecyclemock.Setup(x => x.Add(It.IsAny<Route>(), It.IsAny<Channel>())).Returns(Builder.CreateListenerContext());

            var sut = new ListenerContextLoader(lifecyclemock.Object);

            var route = Builder.CreateRoute();

            var channel = Builder.CreateChannel();

            route.Channels.Add(channel);

            sut.Add(route);

            lifecyclemock.Verify(x => x.Add(It.IsAny<Route>(), It.IsAny<Channel>()), Times.Once);

            lifecyclemock.Verify(x => x.Get(It.IsAny<Channel>()), Times.Once);
        }
    }
}
