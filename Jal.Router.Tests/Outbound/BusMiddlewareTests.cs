using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;
using Jal.Router.Tests;
using System.Collections.Generic;

namespace Jal.Router.Tests
{
    [TestClass]
    public class BusMiddlewareTests
    {
        private BusMiddleware Build(IComponentFactoryGateway factory)
        {
            return new BusMiddleware(new NullLogger(), factory);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithNoHandlers_ShouldDoNothing()
        {
            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { Builder.CreateChannel() });

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateChannelShuffler()).Returns(shuffermock.Object);

            factorymock.Setup(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>())).Returns(default(IBusEntryMessageHandler));

            factorymock.Setup(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>())).Returns(default(IBusErrorMessageHandler));

            factorymock.Setup(x => x.CreateBusExitMessageHandler(It.IsAny<Type>())).Returns(default(IBusExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            var sut = Build(factorymock.Object);

            await sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            factorymock.Verify(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateBusExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithException_ShouldThrowException()
        {
            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { Builder.CreateChannel() });

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateChannelShuffler()).Returns(shuffermock.Object);

            factorymock.Setup(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>())).Returns(default(IBusEntryMessageHandler));

            factorymock.Setup(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>())).Returns(default(IBusErrorMessageHandler));

            factorymock.Setup(x => x.CreateBusExitMessageHandler(It.IsAny<Type>())).Returns(default(IBusExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            var sut = Build(factorymock.Object);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new Exception()));

            factorymock.Verify(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateBusExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithExceptionAndHandlers_ShouldExecuteTheHandlers()
        {
            var mockentryhandler = new Mock<IBusEntryMessageHandler>();

            var entryhandler = mockentryhandler.Object;

            var mockerrorhandler = new Mock<IBusErrorMessageHandler>();

            mockerrorhandler.Setup(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<ErrorHandler>())).ReturnsAsync(true);

            var errorhandler = mockerrorhandler.Object;

            var mockexithandler = new Mock<IBusExitMessageHandler>();

            var exithandler = mockexithandler.Object;

            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { Builder.CreateChannel() });

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateChannelShuffler()).Returns(shuffermock.Object);

            factorymock.Setup(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>())).Returns(entryhandler);

            factorymock.Setup(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>())).Returns(errorhandler);

            factorymock.Setup(x => x.CreateBusExitMessageHandler(It.IsAny<Type>())).Returns(exithandler);

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.EndPoint.EntryHandlers.Add(new Jal.Router.Model.Handler(entryhandler.GetType(), new Dictionary<string, object>()));

            messagecontext.EndPoint.ExitHandlers.Add(new Jal.Router.Model.Handler(exithandler.GetType(), new Dictionary<string, object>()));

            messagecontext.EndPoint.ErrorHandlers.Add(new ErrorHandler(errorhandler.GetType(), new Dictionary<string, object>(), false));

            var sut = Build(factorymock.Object);

            await sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new Exception());

            factorymock.Verify(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>()), Times.Once());

            mockentryhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Jal.Router.Model.Handler>()), Times.Once());

            factorymock.Verify(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>()), Times.Once());

            mockerrorhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<ErrorHandler>()), Times.Once());

            factorymock.Verify(x => x.CreateBusExitMessageHandler(It.IsAny<Type>()), Times.Once());

            mockexithandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Jal.Router.Model.Handler>()), Times.Once());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithExceptionAndHandler_ShouldExecuteTheHandlerAndThrowException()
        {
            var mockerrorhandler = new Mock<IBusErrorMessageHandler>();

            mockerrorhandler.Setup(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<ErrorHandler>())).ReturnsAsync(false);

            var errorhandler = mockerrorhandler.Object;

            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { Builder.CreateChannel() });

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateChannelShuffler()).Returns(shuffermock.Object);

            factorymock.Setup(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>())).Returns(default(IBusEntryMessageHandler));

            factorymock.Setup(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>())).Returns(errorhandler);

            factorymock.Setup(x => x.CreateBusExitMessageHandler(It.IsAny<Type>())).Returns(default(IBusExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.EndPoint.ErrorHandlers.Add(new ErrorHandler(errorhandler.GetType(), new Dictionary<string, object>(), false));

            var sut = Build(factorymock.Object);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new Exception()));

            factorymock.Verify(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>()), Times.Once());

            mockerrorhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<ErrorHandler>()), Times.Once());

            factorymock.Verify(x => x.CreateBusExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithExceptionAndHandlerForApplicationException_ShouldNotExecuteTheHandlerAndThrowException()
        {
            var mockerrorhandler = new Mock<IBusErrorMessageHandler>();

            mockerrorhandler.Setup(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<ErrorHandler>())).ReturnsAsync(false);

            var errorhandler = mockerrorhandler.Object;

            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { Builder.CreateChannel() });

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateChannelShuffler()).Returns(shuffermock.Object);

            factorymock.Setup(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>())).Returns(default(IBusEntryMessageHandler));

            factorymock.Setup(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>())).Returns(errorhandler);

            factorymock.Setup(x => x.CreateBusExitMessageHandler(It.IsAny<Type>())).Returns(default(IBusExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            var handler = new ErrorHandler(errorhandler.GetType(), new Dictionary<string, object>(), false);

            handler.ExceptionTypes.Add(typeof(ApplicationException));

            messagecontext.EndPoint.ErrorHandlers.Add(handler);

            var sut = Build(factorymock.Object);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new Exception()));

            factorymock.Verify(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>()), Times.Never());

            mockerrorhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<ErrorHandler>()), Times.Never());

            factorymock.Verify(x => x.CreateBusExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithApplicationExceptionAndHandlerForApplicationException_ShouldExecuteTheHandler()
        {
            var mockerrorhandler = new Mock<IBusErrorMessageHandler>();

            mockerrorhandler.Setup(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<ErrorHandler>())).ReturnsAsync(true);

            var errorhandler = mockerrorhandler.Object;

            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { Builder.CreateChannel() });

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateChannelShuffler()).Returns(shuffermock.Object);

            factorymock.Setup(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>())).Returns(default(IBusEntryMessageHandler));

            factorymock.Setup(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>())).Returns(errorhandler);

            factorymock.Setup(x => x.CreateBusExitMessageHandler(It.IsAny<Type>())).Returns(default(IBusExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            var handler = new ErrorHandler(errorhandler.GetType(), new Dictionary<string, object>(), false);

            handler.ExceptionTypes.Add(typeof(ApplicationException));

            messagecontext.EndPoint.ErrorHandlers.Add(handler);

            var sut = Build(factorymock.Object);

            await sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new ApplicationException());

            factorymock.Verify(x => x.CreateBusEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateBusErrorMessageHandler(It.IsAny<Type>()), Times.Once());

            mockerrorhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<ErrorHandler>()), Times.Once());

            factorymock.Verify(x => x.CreateBusExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }
    }
}
