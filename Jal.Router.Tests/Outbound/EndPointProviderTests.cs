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

            var ep = new EndPoint(endpointname);

            ep.SetContentType(typeof(object));

            ep.SetCondition((e, o, t) => true);

            mock.Setup(x => x.GetEndPoints()).Returns(new EndPoint[] { ep });

            var sut = new EndPointProvider(new Interface.IRouterConfigurationSource[] { mock.Object });

            var options = new Options(endpointname,
            new System.Collections.Generic.Dictionary<string, string>(),
            new SagaContext(null, string.Empty),
            new TrackingContext(null, new System.Collections.Generic.List<Tracking>()),
            new TracingContext(string.Empty),
            null,
            null,
            string.Empty,
            null);

            var endpoint = sut.Provide(options, typeof(object));

            endpoint.ShouldNotBeNull();

            endpoint.Name.ShouldBe(endpointname);
        }

        [TestMethod]
        public void Provide_WithNonExistingEndpointName_ShouldThrowException()
        {
            var endpointname = "name";

            var sut = new EndPointProvider(new Interface.IRouterConfigurationSource[] { });

            Should.Throw<ApplicationException>(() =>
            sut.Provide(new Options(endpointname,
            new System.Collections.Generic.Dictionary<string, string>(),
            new SagaContext(null, string.Empty),
            new TrackingContext(null, new System.Collections.Generic.List<Tracking>()),
            new TracingContext(string.Empty),
            null,
            null, 
            string.Empty,
            null), typeof(object)));
        }
    }
}
