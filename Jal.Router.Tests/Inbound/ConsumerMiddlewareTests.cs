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
    public class ConsumerMiddlewareTests
    {
        private ConsumerMiddleware Build(IConsumer consumer, IComponentFactoryGateway factory)
        {
            return new ConsumerMiddleware(factory, consumer);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageDisabled_ShouldBeExecutedAndNotStored()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = new Mock<IConsumer>();

            var messagecontext = Builder.CreateMessageContext();

            var factory = factorymock.Object;

            factory.Configuration.DisableStorage();

            var sut = Build(consumermock.Object, factory);

            await sut.ExecuteAsync(new ChainOfResponsability.Model.Context<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasNotExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithException_ShouldBeExecutedAndStored()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = new Mock<IConsumer>();

            consumermock.Setup(x => x.Consume(It.IsAny<MessageContext>())).Throws(new Exception());

            var messagecontext = Builder.CreateMessageContext();

            var factory = factorymock.Object;

            var entitystoragemock = Builder.CreateEntityStorage();

            factorymock.Setup(x => x.CreateEntityStorage()).Returns(entitystoragemock.Object);

            factory.Configuration.EnableStorage();

            var sut = Build(consumermock.Object, factory);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new ChainOfResponsability.Model.Context<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask));

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageEnabled_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = new Mock<IConsumer>();

            var messagecontext = Builder.CreateMessageContext();

            var entitystoragemock = Builder.CreateEntityStorage();

            factorymock.Setup(x => x.CreateEntityStorage()).Returns(entitystoragemock.Object);

            var factory = factorymock.Object;

            factory.Configuration.EnableStorage();

            var sut = Build(consumermock.Object, factory);

            await sut.ExecuteAsync(new ChainOfResponsability.Model.Context<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageEnabledAndIgnoringExceptions_ShouldIgnoreTheException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = new Mock<IConsumer>();

            var messagecontext = Builder.CreateMessageContext();

            var entitystoragemock = Builder.CreateEntityStorage();

            entitystoragemock.Setup(x => x.Create(It.IsAny<MessageEntity>())).Throws(new Exception());

            factorymock.Setup(x => x.CreateEntityStorage()).Returns(entitystoragemock.Object);

            var factory = factorymock.Object;

            factory.Configuration.EnableStorage(true);

            var sut = Build(consumermock.Object, factory);

            await sut.ExecuteAsync(new ChainOfResponsability.Model.Context<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageEnabledAndIgnoringExceptions_ShouldThrowTheException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = new Mock<IConsumer>();

            var messagecontext = Builder.CreateMessageContext();

            var entitystoragemock = Builder.CreateEntityStorage();

            entitystoragemock.Setup(x => x.Create(It.IsAny<MessageEntity>())).Throws(new Exception());

            factorymock.Setup(x => x.CreateEntityStorage()).Returns(entitystoragemock.Object);

            var factory = factorymock.Object;

            factory.Configuration.EnableStorage(false);

            var sut = Build(consumermock.Object, factory);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new ChainOfResponsability.Model.Context<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask));

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasExecuted();
        }
    }
}
