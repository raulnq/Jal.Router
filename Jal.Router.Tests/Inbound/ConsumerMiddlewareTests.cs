using Jal.ChainOfResponsability;
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
        private ConsumerMiddleware Build(IConsumer consumer, IComponentFactoryFacade factory)
        {
            return new ConsumerMiddleware(factory, consumer);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageDisabled_ShouldBeExecutedAndNotStored()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = Builder.CreateConsumerMock();

            var messagecontext = Builder.CreateMessageContextFromListen();

            var factory = factorymock.Object;

            factory.Configuration.DisableStorage();

            var sut = Build(consumermock.Object, factory);

            await sut.ExecuteAsync(new AsyncContext<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasNotExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithException_ShouldBeExecutedAndStored()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = Builder.CreateConsumerMock();

            consumermock.Setup(x => x.Consume(It.IsAny<MessageContext>())).Throws(new Exception());

            var factory = factorymock.Object;

            var entitystoragemock = Builder.CreateEntityStorageMock();

            factorymock.AddEntityStorage(entitystoragemock);

            factory.Configuration.EnableStorage();

            var messagecontext = Builder.CreateMessageContextFromListen(factory);

            var sut = Build(consumermock.Object, factory);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new AsyncContext<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask));

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageEnabled_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = Builder.CreateConsumerMock();

            var entitystoragemock = Builder.CreateEntityStorageMock();

            factorymock.AddEntityStorage(entitystoragemock);

            var factory = factorymock.Object;

            var messagecontext = Builder.CreateMessageContextFromListen(factory);

            factory.Configuration.EnableStorage();

            var sut = Build(consumermock.Object, factory);

            await sut.ExecuteAsync(new AsyncContext<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageEnabledAndIgnoringExceptions_ShouldIgnoreTheException()
        {
            var factorymock = Builder.CreateFactoryMock();

            var consumermock = Builder.CreateConsumerMock();

            var entitystoragemock = Builder.CreateEntityStorageMock();

            entitystoragemock.Setup(x => x.Insert(It.IsAny<MessageEntity>(), It.IsAny<IMessageSerializer>())).Throws(new Exception());

            factorymock.AddEntityStorage(entitystoragemock);

            var factory = factorymock.Object;

            var messagecontext = Builder.CreateMessageContextFromListen(factory);

            factory.Configuration.EnableStorage(true);

            var sut = Build(consumermock.Object, factory);

            await sut.ExecuteAsync(new AsyncContext<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

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

            var entitystoragemock = Builder.CreateEntityStorageMock();

            entitystoragemock.Setup(x => x.Insert(It.IsAny<MessageEntity>(), It.IsAny<IMessageSerializer>())).Throws(new Exception());

            factorymock.AddEntityStorage(entitystoragemock);

            var factory = factorymock.Object;

            var messagecontext = Builder.CreateMessageContextFromListen(factory);

            factory.Configuration.EnableStorage(false);

            var sut = Build(consumermock.Object, factory);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new AsyncContext<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask));

            consumermock.WasExecuted();

            messagecontext.TrackingContext.Trackings.ShouldNotBeEmpty();

            factorymock.CreateEntityStorageWasExecuted();

            entitystoragemock.CreateMessageEntityWasExecuted();
        }
    }
}
