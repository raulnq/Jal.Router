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
    public class ConsumerTests
    {
        private Consumer Build(IComponentFactoryGateway factory)
        {
            return new Consumer(factory, new NullLogger());
        }

        [TestMethod]
        public async Task Consume_WithoutRoutes_ShouldDoNothing()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var messagecontext = Builder.CreateMessageContext();

            var sut = Build(factorymock.Object);

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Never());
        }

        [TestMethod]
        public async Task Consume_WithoutRoutesFromSaga_ShouldDoNothing()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var messagecontext = Builder.CreateMessageContextWithSaga();

            var sut = Build(factorymock.Object);

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Never());
        }

        [TestMethod]
        public async Task Consume_WithRouteAndConsumerButNoEvaluator_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRoute();

            var wasconsumed = false;

            route.RouteMethods.Add(new RouteMethod<object, Handler>((c,h) => { wasconsumed = true; return Task.CompletedTask; } ));

            var messagecontext = Builder.CreateMessageContext(route);

            var sut = Build(factorymock.Object);

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

            wasconsumed.ShouldBeTrue();
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task Consume_WithRouteAndConsumerAndEvaluator_ShouldBe(bool evaluator)
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRoute();

            var wasconsumed = false;

            var method = new RouteMethod<object, Handler>((c, h) => { wasconsumed = true; return Task.CompletedTask; });

            method.UpdateEvaluator((o, h) => evaluator);

            route.RouteMethods.Add(method);

            var messagecontext = Builder.CreateMessageContext(route);

            var sut = Build(factorymock.Object);

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

            wasconsumed.ShouldBe(evaluator);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task Consume_WithRouteAndConsumerAndEvaluatorWithContext_ShouldBe(bool evaluator)
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRoute();

            var wasconsumed = false;

            var method = new RouteMethod<object, Handler>((c, h) => { wasconsumed = true; return Task.CompletedTask; });

            method.UpdateEvaluatorWithContext((o, h, c) => evaluator);

            route.RouteMethods.Add(method);

            var messagecontext = Builder.CreateMessageContext(route);

            var sut = new Consumer(factorymock.Object, new NullLogger());

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

            wasconsumed.ShouldBe(evaluator);
        }

        [TestMethod]
        public async Task Consume_WithRouteAndConsumerWithContext_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRoute();

            var wasconsumed = false;

            route.RouteMethods.Add(new RouteMethod<object, Handler>((object o, Handler h, MessageContext c) => { wasconsumed = true; return Task.CompletedTask; }));

            var messagecontext = Builder.CreateMessageContext(route);

            var sut = new Consumer(factorymock.Object, new NullLogger());

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

            wasconsumed.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Consume_WithRouteAndConsumerFromSaga_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRoute();

            var wasconsumed = false;

            route.RouteMethods.Add(new RouteMethod<object, Handler>((o, h) => { wasconsumed = true; return Task.CompletedTask; }));

            var messagecontext = Builder.CreateMessageContextWithSaga(route);

            var sut = new Consumer(factorymock.Object, new NullLogger());

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

            wasconsumed.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Consume_WithRouteAndConsumerWithDataFromSaga_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRoute();

            var wasconsumed = false;

            route.RouteMethods.Add(new RouteMethod<object, Handler>((object o, Handler h, object d) => { wasconsumed = true; return Task.CompletedTask; }));

            var messagecontext = Builder.CreateMessageContextWithSaga(route);

            var sut = new Consumer(factorymock.Object, new NullLogger());

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

            wasconsumed.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Consume_WithRouteAndConsumerWithContextFromSaga_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRoute();

            var wasconsumed = false;

            route.RouteMethods.Add(new RouteMethod<object, Handler>((object o, Handler h, MessageContext c) => { wasconsumed = true; return Task.CompletedTask; }));

            var messagecontext = Builder.CreateMessageContextWithSaga(route);

            var sut = new Consumer(factorymock.Object, new NullLogger());

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

            wasconsumed.ShouldBeTrue();
        }


        [TestMethod]
        public async Task Consume_WithRouteAndConsumerWithDataAndContextFromSaga_ShouldBeExecuted()
        {
            var factorymock = Builder.CreateFactoryMockWithHandler<Handler>();

            var route = Builder.CreateRoute();

            var wasconsumed = false;

            route.RouteMethods.Add(new RouteMethod<object, Handler>((object o, Handler h, MessageContext c, object data) => { wasconsumed = true; return Task.CompletedTask; }));

            var messagecontext = Builder.CreateMessageContextWithSaga(route);

            var sut = new Consumer(factorymock.Object, new NullLogger());

            await sut.Consume(messagecontext);

            factorymock.Verify(x => x.CreateComponent<Handler>(typeof(Handler)), Times.Once());

            wasconsumed.ShouldBeTrue();
        }
    }
}
