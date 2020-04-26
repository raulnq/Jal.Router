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
    public class ConsumerTests
    {
        public Mock<ITypedConsumer> CreateTypedConsumerMock(bool select = true)
        {
            var typeconsumer = new Mock<ITypedConsumer>();

            typeconsumer.Setup(m => m.Select(It.IsAny<MessageContext>(), It.IsAny<object>(), It.IsAny<RouteMethod<object, Handler>>(), It.IsAny<Handler>())).Returns(select);

            typeconsumer.Setup(m => m.Select(It.IsAny<MessageContext>(), It.IsAny<object>(), It.IsAny<RouteMethodWithData<object, Handler, object>>(), It.IsAny<Handler>(), It.IsAny<object>())).Returns(select);

            return typeconsumer;
        }

        private Consumer Build(IComponentFactoryFacade factory, ITypedConsumer typedconsumer)
        {
            return new Consumer(factory, new NullLogger(), typedconsumer);
        }

        [TestMethod]
        public async Task Consume_WithNoRoutes_ShouldDoNothing()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var messagecontext = Builder.CreateMessageContext();

            var typedconsumer = CreateTypedConsumerMock();

            var sut = Build(factorymock.Object, typedconsumer.Object);

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Never());

            typedconsumer.Verify(x => x.Consume(It.IsAny<MessageContext>(), It.IsAny<object>(), It.IsAny<RouteMethod<object, Handler>>(), It.IsAny<Handler>()), Times.Never());
        }

        [TestMethod]
        public async Task Consume_WithoutRoutesForSaga_ShouldDoNothing()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var messagecontext = Builder.CreateMessageContextWithSaga();

            var typedconsumer = CreateTypedConsumerMock();

            var sut = Build(factorymock.Object, typedconsumer.Object);

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Never());

            typedconsumer.Verify(x => x.Consume(It.IsAny<MessageContext>(), It.IsAny<object>(), It.IsAny<RouteMethodWithData<object, Handler, object>>(), It.IsAny<Handler>(), It.IsAny<object>()), Times.Never());
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task Consume_WithRoute_ShouldBe(bool evaluator)
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRouteWithConsumer();

            var messagecontext = Builder.CreateMessageContext(route);

            var typedconsumer = CreateTypedConsumerMock(evaluator);

            var sut = Build(factorymock.Object, typedconsumer.Object);

            await sut.Consume(messagecontext);

            if(evaluator)
            {
                factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

                typedconsumer.Verify(x => x.Consume(It.IsAny<MessageContext>(), It.IsAny<object>(), It.IsAny<RouteMethod<object, Handler>>(), It.IsAny<Handler>()), Times.Once());
            }
            else
            {
                factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

                typedconsumer.Verify(x => x.Consume(It.IsAny<MessageContext>(), It.IsAny<object>(), It.IsAny<RouteMethod<object, Handler>>(), It.IsAny<Handler>()), Times.Never());
            }
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task Consume_WithRouteForSaga_ShouldBe(bool evaluator)
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRouteWithConsumer("status");

            var messagecontext = Builder.CreateMessageContextWithSaga(status: "status");

            var typedconsumer = CreateTypedConsumerMock(evaluator);

            var sut = Build(factorymock.Object, typedconsumer.Object);

            await sut.Consume(messagecontext);

            if (evaluator)
            {
                factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

                typedconsumer.Verify(x => x.Consume(It.IsAny<MessageContext>(), It.IsAny<object>(), It.IsAny<RouteMethodWithData<object, Handler, object>>(), It.IsAny<Handler>(), It.IsAny<object>()), Times.Once());

                messagecontext.SagaContext.Data.Status.ShouldBe("status");
            }
            else
            {
                factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

                typedconsumer.Verify(x => x.Consume(It.IsAny<MessageContext>(), It.IsAny<object>(), It.IsAny<RouteMethodWithData<object, Handler, object>>(), It.IsAny<Handler>(), It.IsAny<object>()), Times.Never());

                messagecontext.SagaContext.Data.Status.ShouldNotBe("status");
            }

        }
    }
}
