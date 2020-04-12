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
    public class InitialConsumerMiddlewareTests
    {
        private InitialConsumerMiddleware Build(IConsumer consumer, IComponentFactoryGateway factory)
        {
            return new InitialConsumerMiddleware(factory, consumer);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageDisabled_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = new Mock<IConsumer>();

            var id = Guid.NewGuid().ToString();

            var entitystoragemock = Builder.CreateEntityStorage(id);

            factorymock.Setup(x => x.CreateEntityStorage()).Returns(entitystoragemock.Object);

            var messagecontext = Builder.CreateMessageContextWithSaga();

            var factory = factorymock.Object;

            factory.Configuration.DisableStorage();

            var sut = Build(consumermock.Object, factory);

            await sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            messagecontext.SagaContext.IsLoaded().ShouldBeTrue();

            messagecontext.SagaContext.Id.ShouldBe(id);

            messagecontext.SagaContext.Data.Id.ShouldBe(id);

            messagecontext.SagaContext.Data.Status.ShouldBe("STARTED");

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasNotExecuted();

            entitystoragemock.CreateSagaDataWasExecuted();

            entitystoragemock.UpdateSagaDataWasExecuted();
        }
    }
}
