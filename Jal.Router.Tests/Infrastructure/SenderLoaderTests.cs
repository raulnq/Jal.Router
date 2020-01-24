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
    public class SenderLoaderTests
    {
        [TestMethod]
        public async Task Run_WithEndpoint_ShouldBeLoaded()
        {
            var creatormock = new Mock<ISenderContextCreator>();

            creatormock.Setup(x => x.Create(It.IsAny<Channel>())).Returns(Builder.CreateSenderContext());

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var endpoint = new EndPoint("name");

            endpoint.Channels.Add(Builder.CreateChannel());

            factory.Configuration.Runtime.EndPoints.Add(endpoint);

            var sut = new SenderLoader(factory, creatormock.Object, new NullLogger());

            await sut.Run();

            factory.Configuration.Runtime.SenderContexts.Count.ShouldBe(1);

            factory.Configuration.Runtime.SenderContexts[0].Endpoints.Count.ShouldBe(1);

            creatormock.Verify(x => x.Create(It.IsAny<Channel>()), Times.Once);

            creatormock.Verify(x => x.Open(It.IsAny<SenderContext>()), Times.Once);
        }
    }
}
