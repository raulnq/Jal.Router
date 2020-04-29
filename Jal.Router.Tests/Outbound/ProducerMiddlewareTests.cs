using Jal.ChainOfResponsability;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class ProducerMiddlewareTests
    {
        [TestMethod]
        public async Task ExecuteAsync_WithStorageDisabled_ShouldBeExecuted()
        {
            var producermock = new Mock<IProducer>();

            var factorymock = Builder.CreateFactoryMock();

            var factory = factorymock.Object;

            factory.Configuration.DisableStorage();

            var messagecontext = Builder.CreateMessageContextFromListen();

            messagecontext.Route.Middlewares.Add(typeof(string));

            var sut = new ProducerMiddleware(producermock.Object, factorymock.Object);

            await sut.ExecuteAsync(new AsyncContext<MessageContext>() { Data = messagecontext }, x=>Task.CompletedTask);

            producermock.WasExecuted();
        }

        [TestMethod]
        public async Task ExecuteAsync_WithStorageEnabled_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMock();

            var producermock = new Mock<IProducer>();

            var entitystoragemock = Builder.CreateEntityStorageMock();

            factorymock.AddEntityStorage(entitystoragemock);

            var factory = factorymock.Object;

            var messagecontext = Builder.CreateMessageContextToSend(factory);

            factory.Configuration.EnableStorage();

            var sut = new ProducerMiddleware(producermock.Object, factorymock.Object);

            await sut.ExecuteAsync(new AsyncContext<MessageContext>() { Data = messagecontext }, x => Task.CompletedTask);

            producermock.WasExecuted();

            entitystoragemock.CreateMessageEntityWasExecuted();
        }
    }
}
