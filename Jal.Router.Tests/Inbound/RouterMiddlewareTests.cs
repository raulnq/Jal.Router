using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{

    [TestClass]
    public class RouterMiddlewareTests
    {
        private RouterMiddleware Build(IComponentFactoryFacade factory)
        {
            return new RouterMiddleware(factory, new NullLogger());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithNoHandlers_ShouldDoNothing()
        {
            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>())).Returns(default(IRouteEntryMessageHandler));

            factorymock.Setup(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>())).Returns(default(IRouteErrorMessageHandler));

            factorymock.Setup(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>())).Returns(default(IRouteExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            var sut = Build(factorymock.Object);

            await sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => Task.CompletedTask);

            factorymock.Verify(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithException_ShouldThrowException()
        {
            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>())).Returns(default(IRouteEntryMessageHandler));

            factorymock.Setup(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>())).Returns(default(IRouteErrorMessageHandler));

            factorymock.Setup(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>())).Returns(default(IRouteExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            var sut = Build(factorymock.Object);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new Exception()));

            factorymock.Verify(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithExceptionAndHandlers_ShouldExecuteTheHandlers()
        {
            var mockentryhandler = new Mock<IRouteEntryMessageHandler>();

            var entryhandler = mockentryhandler.Object;

            var mockerrorhandler = new Mock<IRouteErrorMessageHandler>();

            mockerrorhandler.Setup(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<Model.ErrorHandler>())).ReturnsAsync(true);

            var errorhandler = mockerrorhandler.Object;

            var mockexithandler = new Mock<IRouteExitMessageHandler>();

            var exithandler = mockexithandler.Object;

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>())).Returns(entryhandler);

            factorymock.Setup(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>())).Returns(errorhandler);

            factorymock.Setup(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>())).Returns(exithandler);

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.Route.EntryHandlers.Add(new Model.Handler(entryhandler.GetType(), new Dictionary<string, object>()));

            messagecontext.Route.ExitHandlers.Add(new Model.Handler(exithandler.GetType(), new Dictionary<string, object>()));

            messagecontext.Route.ErrorHandlers.Add(new Model.ErrorHandler(errorhandler.GetType(), new Dictionary<string, object>(), false));

            var sut = Build(factorymock.Object);

            await sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new Exception());

            factorymock.Verify(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>()), Times.Once());

            mockentryhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Model.Handler>()), Times.Once());

            factorymock.Verify(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>()), Times.Once());

            mockerrorhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<Model.ErrorHandler>()), Times.Once());

            factorymock.Verify(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>()), Times.Once());

            mockexithandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Model.Handler>()), Times.Once());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithExceptionAndHandler_ShouldExecuteTheHandlerAndThrowException()
        {
            var mockerrorhandler = new Mock<IRouteErrorMessageHandler>();

            mockerrorhandler.Setup(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<Model.ErrorHandler>())).ReturnsAsync(false);

            var errorhandler = mockerrorhandler.Object;

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>())).Returns(default(IRouteEntryMessageHandler));

            factorymock.Setup(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>())).Returns(errorhandler);

            factorymock.Setup(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>())).Returns(default(IRouteExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            messagecontext.Route.ErrorHandlers.Add(new Model.ErrorHandler(errorhandler.GetType(), new Dictionary<string, object>(), false));

            var sut = Build(factorymock.Object);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new Exception()));

            factorymock.Verify(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>()), Times.Once());

            mockerrorhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<Model.ErrorHandler>()), Times.Once());

            factorymock.Verify(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithExceptionAndHandlerForApplicationException_ShouldNotExecuteTheHandlerAndThrowException()
        {
            var mockerrorhandler = new Mock<IRouteErrorMessageHandler>();

            mockerrorhandler.Setup(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<Model.ErrorHandler>())).ReturnsAsync(false);

            var errorhandler = mockerrorhandler.Object;

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>())).Returns(default(IRouteEntryMessageHandler));

            factorymock.Setup(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>())).Returns(errorhandler);

            factorymock.Setup(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>())).Returns(default(IRouteExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            var handler = new Model.ErrorHandler(errorhandler.GetType(), new Dictionary<string, object>(), false);

            handler.ExceptionTypes.Add(typeof(ApplicationException));

            messagecontext.Route.ErrorHandlers.Add(handler);

            var sut = Build(factorymock.Object);

            await Should.ThrowAsync<Exception>(sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new Exception()));

            factorymock.Verify(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>()), Times.Never());

            mockerrorhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<Model.ErrorHandler>()), Times.Never());

            factorymock.Verify(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }

        [TestMethod]
        public async Task ExecuteAsync_WithApplicationExceptionAndHandlerForApplicationException_ShouldExecuteTheHandler()
        {
            var mockerrorhandler = new Mock<IRouteErrorMessageHandler>();

            mockerrorhandler.Setup(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<Model.ErrorHandler>())).ReturnsAsync(true);

            var errorhandler = mockerrorhandler.Object;

            var factorymock = Builder.CreateFactoryMock();

            factorymock.Setup(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>())).Returns(default(IRouteEntryMessageHandler));

            factorymock.Setup(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>())).Returns(errorhandler);

            factorymock.Setup(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>())).Returns(default(IRouteExitMessageHandler));

            var messagecontext = Builder.CreateMessageContext();

            var handler = new Model.ErrorHandler(errorhandler.GetType(), new Dictionary<string, object>(), false);

            handler.ExceptionTypes.Add(typeof(ApplicationException));

            messagecontext.Route.ErrorHandlers.Add(handler);

            var sut = Build(factorymock.Object);

            await sut.ExecuteAsync(new ChainOfResponsability.AsyncContext<MessageContext>() { Data = messagecontext }, c => throw new ApplicationException());

            factorymock.Verify(x => x.CreateRouteEntryMessageHandler(It.IsAny<Type>()), Times.Never());

            factorymock.Verify(x => x.CreateRouteErrorMessageHandler(It.IsAny<Type>()), Times.Once());

            mockerrorhandler.Verify(x => x.Handle(It.IsAny<MessageContext>(), It.IsAny<Exception>(), It.IsAny<Model.ErrorHandler>()), Times.Once());

            factorymock.Verify(x => x.CreateRouteExitMessageHandler(It.IsAny<Type>()), Times.Never());
        }
    }
}
