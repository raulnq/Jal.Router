﻿using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;

namespace Jal.Router.Tests
{
    [TestClass]
    public class ListenerContextLifecycleTests
    {
        [TestMethod]
        public void Add_With_ShouldBeCreated()
        {
            var routermock = new Mock<IRouter>();

            var listenermock = new Mock<IChannelListener>();

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateListenerChannel(It.IsAny<Model.ChannelType>(), It.IsAny<Type>())).Returns((listenermock.Object, new NullPointToPointChannel(), new NullPointToPointChannel(), new NullPointToPointChannel()));

            var factory = factorymock.Object;

            var sut = new ListenerContextLifecycle(factory, routermock.Object, new NullLogger(), new Hasher());

            var listenercontext = sut.Add(Builder.CreateRoute(), Builder.CreateChannel());

            factorymock.Verify(x => x.CreateListenerChannel(It.IsAny<Model.ChannelType>(), It.IsAny<Type>()), Times.Once);

            listenercontext.Channel.ShouldNotBeNull();

            listenercontext.ListenerChannel.ShouldNotBeNull();

            listenercontext.ListenerChannel.ShouldBeAssignableTo<IChannelListener>();

            listenercontext.Channel.Path.ShouldBe("path");
        }
    }
}
