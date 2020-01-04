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
    public class MiddleConsumerMiddlewareTests
    {
        private MiddleConsumerMiddleware Build(IConsumer consumer, IComponentFactoryGateway factory)
        {
            return new MiddleConsumerMiddleware(factory, consumer);
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

            await sut.ExecuteAsync(new ChainOfResponsability.Model.Context<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            messagecontext.SagaContext.IsLoaded().ShouldBeTrue();

            messagecontext.SagaContext.Data.Updated.ShouldBe(messagecontext.DateTimeUtc);

            messagecontext.SagaContext.Data.Status.ShouldBe("IN PROCESS");

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasNotExecuted();

            entitystoragemock.UpdateSagaDataWasExecuted();

            entitystoragemock.GetSagaDataWasExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageDisabledAndNoSagaData_ShouldThrowAnException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = new Mock<IConsumer>();

            var id = Guid.NewGuid().ToString();

            var entitystoragemock = Builder.CreateEntityStorage(id);

            entitystoragemock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(default(SagaData));

            factorymock.Setup(x => x.CreateEntityStorage()).Returns(entitystoragemock.Object);

            var messagecontext = Builder.CreateMessageContextWithSaga();

            var factory = factorymock.Object;

            factory.Configuration.DisableStorage();

            var sut = Build(consumermock.Object, factory);

            var exception = await Should.ThrowAsync<ApplicationException>(sut.ExecuteAsync(new ChainOfResponsability.Model.Context<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask));

            exception.Message.ShouldContain("No saga record type");

            consumermock.WasNotExecuted();

            messagecontext.TrackingContext.Trackings.ShouldBeEmpty();

            messagecontext.SagaContext.IsLoaded().ShouldBeFalse();

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasNotExecuted();

            entitystoragemock.UpdateSagaDataWasNotExecuted();

            entitystoragemock.GetSagaDataWasExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageDisabledAndSagaDataWithNoData_ShouldThrowAnException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = new Mock<IConsumer>();

            var id = Guid.NewGuid().ToString();

            var entitystoragemock = Builder.CreateEntityStorage(id);

            entitystoragemock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new SagaData(null, typeof(object), "name", DateTime.UtcNow, 0, string.Empty ));

            factorymock.Setup(x => x.CreateEntityStorage()).Returns(entitystoragemock.Object);

            var messagecontext = Builder.CreateMessageContextWithSaga();

            var factory = factorymock.Object;

            factory.Configuration.DisableStorage();

            var sut = Build(consumermock.Object, factory);

            var exception = await Should.ThrowAsync<ApplicationException>(sut.ExecuteAsync(new ChainOfResponsability.Model.Context<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask));

            exception.Message.ShouldContain("Empty/Invalid saga record data");

            consumermock.WasNotExecuted();

            messagecontext.TrackingContext.Trackings.ShouldBeEmpty();

            messagecontext.SagaContext.IsLoaded().ShouldBeTrue();

            messagecontext.SagaContext.Data.IsValid().ShouldBeFalse();

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasNotExecuted();

            entitystoragemock.UpdateSagaDataWasNotExecuted();

            entitystoragemock.GetSagaDataWasExecuted();
        }
    }
}
