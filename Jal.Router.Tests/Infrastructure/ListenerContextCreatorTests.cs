using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace Jal.Router.Tests
{
    [TestClass]
    public class ListenerContextCreatorTests
    {
        [TestMethod]
        public void Open_With_ShouldBeOpened()
        {
            var factorymock = Builder.CreateFactoryMock();

            var listenermock = new Mock<IListenerChannel>();

            var factory = factorymock.Object;

            var sut = new ListenerContextCreator(factory, new NullLogger());

            sut.Open(new Model.ListenerContext(Builder.CreateChannel(), listenermock.Object, null));

            listenermock.Verify(x => x.Listen(It.IsAny<Model.ListenerContext>()), Times.Once);

            listenermock.Verify(x => x.Open(It.IsAny<Model.ListenerContext>()), Times.Once);
        }

        [TestMethod]
        public void Create_With_ShouldBeCreated()
        {
            var listenermock = new Mock<IListenerChannel>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateListenerChannel(It.IsAny<Model.ChannelType>())).Returns(listenermock.Object);

            var factory = factorymock.Object;

            var sut = new ListenerContextCreator(factory, new NullLogger());

            var listenercontext = sut.Create(Builder.CreateChannel());

            factorymock.Verify(x => x.CreateListenerChannel(It.IsAny<Model.ChannelType>()), Times.Once);

            listenercontext.Channel.ShouldNotBeNull();

            listenercontext.ListenerChannel.ShouldNotBeNull();

            listenercontext.ListenerChannel.ShouldBeAssignableTo<IListenerChannel>();

            listenercontext.Channel.Path.ShouldBe("path");

            listenercontext.Partition.ShouldBeNull();
        }
    }
}
