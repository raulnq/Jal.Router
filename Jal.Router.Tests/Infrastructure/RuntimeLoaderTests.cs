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
            var routermock = new Mock<IRouter>();

            var configurationmock = new Mock<IRouterConfigurationSource>();

            configurationmock.Setup(x => x.GetRoutes()).Returns(new Model.Route[] { Builder.CreateRoute() });

            configurationmock.Setup(x => x.GetEndPoints()).Returns(new Model.EndPoint[] { new Model.EndPoint("name") }  );

            configurationmock.Setup(x => x.GetPartitions()).Returns(new Model.Partition[] { new Model.Partition("name") });

            configurationmock.Setup(x => x.GetResources()).Returns(new Model.Resource[] { new Model.Resource(Model.ChannelType.PointToPoint, "path", "connectionstring", new Dictionary<string, string>()) });

            var saga = new Model.Saga("name", typeof(object));

            saga.InitialRoutes.Add(Builder.CreateRoute());

            saga.FinalRoutes.Add(Builder.CreateRoute());

            saga.Routes.Add(Builder.CreateRoute());

            configurationmock.Setup(x => x.GetSagas()).Returns(new Model.Saga[] { saga });

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            var sut = new RuntimeLoader(factory, new IRouterConfigurationSource[] { configurationmock.Object }, routermock.Object, new NullLogger());

            await sut.Run();

            configurationmock.Verify(x => x.GetRoutes(), Times.Once);

            configurationmock.Verify(x => x.GetEndPoints(), Times.Once);

            configurationmock.Verify(x => x.GetPartitions(), Times.Once);

            configurationmock.Verify(x => x.GetResources(), Times.Once);

            configurationmock.Verify(x => x.GetSagas(), Times.Once);

            factory.Configuration.Runtime.Routes.Count.ShouldBe(4);

            factory.Configuration.Runtime.EndPoints.Count.ShouldBe(1);

            factory.Configuration.Runtime.Partitions.Count.ShouldBe(1);

            factory.Configuration.Runtime.Resources.Count.ShouldBe(1);

            factory.Configuration.Runtime.Sagas.Count.ShouldBe(1);
        }
    }
}
