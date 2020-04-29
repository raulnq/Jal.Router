using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;

namespace Jal.Router.Tests
{
    [TestClass]
    public class EndPointProviderTests
    {
        [TestMethod]
        public void Provide_WithExistingEndpointName_ShouldReturn()
        {
            var endpointname = "name";

            var mock = new Mock<IRouterConfigurationSource>();

            mock.Setup(x => x.GetEndPoints()).Returns(new EndPoint[] { Builder.CreateEndpoint(endpointname) });

            var sut = new EndPointProvider(new IRouterConfigurationSource[] { mock.Object });

            var endpoint = sut.Provide(Options.Create(endpointname), typeof(object));

            endpoint.ShouldNotBeNull();

            endpoint.Name.ShouldBe(endpointname);
        }

        [TestMethod]
        public void Provide_WithNonExistingEndpointName_ShouldThrowException()
        {
            var sut = new EndPointProvider(new IRouterConfigurationSource[] { });

            Should.Throw<ApplicationException>(() => sut.Provide(Options.Create(), typeof(object)));
        }
    }
}
