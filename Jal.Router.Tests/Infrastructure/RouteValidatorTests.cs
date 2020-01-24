using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class RouteValidatorTests
    {
        [TestMethod]
        public async Task Run_WithError_ShouldThrowException()
        {
            var validatormock = new Mock<IChannelValidator>();

            validatormock.Setup(x => x.Validate(It.IsAny<Channel>(), It.IsAny<string>(), It.IsAny<string>())).Returns("error");

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var route = Builder.CreateRoute();

            route.Channels.Add(Builder.CreateChannel());

            factory.Configuration.Runtime.Routes.Add(route);

            var partition = new Partition("name");

            partition.UpdateChannel(Builder.CreateChannel());

            factory.Configuration.Runtime.Partitions.Add(partition);

            var sut = new RouteValidator(factory, new NullLogger(), validatormock.Object);

            var exception = false;

            try
            {
                await sut.Run();
            }
            catch (ApplicationException)
            {
                exception = true;
            }

            exception.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Run_WithRouteAndPartition_ShouldBeInitialized()
        {
            var validatormock = new Mock<IChannelValidator>();

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var route = Builder.CreateRoute();

            route.Channels.Add(Builder.CreateChannel());

            factory.Configuration.Runtime.Routes.Add(route);

            var partition = new Partition("name");

            partition.UpdateChannel(Builder.CreateChannel());

            factory.Configuration.Runtime.Partitions.Add(partition);

            var sut = new RouteValidator(factory, new NullLogger(), validatormock.Object);

            await sut.Run();
        }
    }
}
