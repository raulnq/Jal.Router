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
    public class EndpointValidatorTests
    {
        [TestMethod]
        public async Task Run_WithError_ShouldThrowException()
        {
            var validatormock = new Mock<IChannelValidator>();

            validatormock.Setup(x => x.Validate(It.IsAny<Channel>(), It.IsAny<string>(), It.IsAny<string>())).Returns("error");

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var endpoint = new EndPoint("name");

            endpoint.Channels.Add(Builder.CreateChannel());

            factory.Configuration.Runtime.EndPoints.Add(endpoint);

            var sut = new EndpointValidator(factory, new NullLogger(), validatormock.Object);

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
        public async Task Run_With_ShouldBeInitialized()
        {
            var validatormock = new Mock<IChannelValidator>();

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var endpoint = new EndPoint("name");

            endpoint.Channels.Add(Builder.CreateChannel());

            factory.Configuration.Runtime.EndPoints.Add(endpoint);

            var sut = new EndpointValidator(factory, new NullLogger(), validatormock.Object);

            await sut.Run();
        }
    }
}
