using Jal.Router.Impl;
using Jal.Router.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class RuntimeLoaderTests
    {
        [TestMethod]
        public async Task Run_WithType_ShouldRun()
        {
            var configurationmock = new Mock<IRouterConfigurationSource>();

            configurationmock.Setup(x => x.GetRoutes()).Returns(new Model.Route[] { Builder.CreateRoute() });

            configurationmock.Setup(x => x.GetEndPoints()).Returns(new Model.EndPoint[] { Builder.CreateEndpoint() }  );

            var saga = new Model.Saga("name", typeof(object));

            saga.InitialRoutes.Add(Builder.CreateRoute());

            saga.FinalRoutes.Add(Builder.CreateRoute());

            saga.Routes.Add(Builder.CreateRoute());

            configurationmock.Setup(x => x.GetSagas()).Returns(new Model.Saga[] { saga });

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var sut = new RuntimeLoader(factory, new IRouterConfigurationSource[] { configurationmock.Object }, new NullLogger());

            await sut.Run();

            configurationmock.Verify(x => x.GetRoutes(), Times.Once);

            configurationmock.Verify(x => x.GetEndPoints(), Times.Once);

            configurationmock.Verify(x => x.GetSagas(), Times.Once);

            factory.Configuration.Runtime.Routes.Count.ShouldBe(4);

            factory.Configuration.Runtime.EndPoints.Count.ShouldBe(1);

            factory.Configuration.Runtime.Sagas.Count.ShouldBe(1);
        }
    }
}
