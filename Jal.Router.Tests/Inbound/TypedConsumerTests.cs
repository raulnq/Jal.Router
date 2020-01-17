using Jal.Router.Impl;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    [TestClass]
    public class TypedConsumerTests
    {
        private TypedConsumer Build()
        {
            return new TypedConsumer();
        }

        public void Select_With_ShouldBeTrue()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var routedmethod = new RouteMethod<object, Handler>((ob, hd) => Task.CompletedTask);

            var selected = sut.Select<object, Handler>(messagecontext, new object(), routedmethod, new Handler());

            selected.ShouldBeTrue();
        }

        [TestMethod]
        public void Select_WithContentAndHandler_ShouldBeTrue()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var executed = false;

            var routedmethod = new RouteMethod<object, Handler>((ob, hd) => Task.CompletedTask);

            routedmethod.UpdateEvaluator((ob, hd) => { executed = true; return true; });

            var selected = sut.Select<object, Handler>(messagecontext, new object(), routedmethod, new Handler());

            executed.ShouldBeTrue();

            selected.ShouldBeTrue();
        }

        [TestMethod]
        public void Select_WithContentAndHandlerAndMessageContext_ShouldBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var executed = false;

            var routedmethod = new RouteMethod<object, Handler>((ob, hd) => Task.CompletedTask);

            routedmethod.UpdateEvaluator((ob, hd, co) => { executed = true; return true; });

            var selected = sut.Select<object, Handler>(messagecontext, new object(), routedmethod, new Handler());

            executed.ShouldBeTrue();

            selected.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Consume_WithContentAndHandler_ShouldBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var executed = false;

            await sut.Consume<object, Handler>(messagecontext, new object(), new RouteMethod<object, Handler>((ob, hd)=> { executed = true; return Task.CompletedTask; }), new Handler());

            executed.ShouldBeTrue();
        }

        [TestMethod]
        public async Task Consume_WithContentAndHandlerAndMessageContext_ShouldBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var executed = false;

            await sut.Consume<object, Handler>(messagecontext, new object(), new RouteMethod<object, Handler>((object ob, Handler hd, MessageContext mc) => { executed = true; return Task.CompletedTask; }), new Handler());

            executed.ShouldBeTrue();
        }

        [TestMethod]
        public async Task ConsumeForData_WithContentAndHandler_ShouldBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var executed = false;

            await sut.Consume<object, Handler, object>(messagecontext, new object(), new RouteMethod<object, Handler>((ob, hd) => { executed = true; return Task.CompletedTask; }), new Handler(), new object());

            executed.ShouldBeTrue();
        }

        [TestMethod]
        public async Task ConsumeForData_WithContentAndHandlerAndMessageContext_ShouldBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var executed = false;

            await sut.Consume<object, Handler, object>(messagecontext, new object(), new RouteMethod<object, Handler>((object ob, Handler hd, MessageContext mc) => { executed = true; return Task.CompletedTask; }), new Handler(), new object());

            executed.ShouldBeTrue();
        }

        [TestMethod]
        public async Task ConsumeForSaga_WithContentAndHandlerAndData_ShouldBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var executed = false;

            await sut.Consume<object, Handler, object>(messagecontext, new object(), new RouteMethod<object, Handler>((ob, hd, da) => { executed = true; return Task.CompletedTask; }), new Handler(), new object());

            executed.ShouldBeTrue();
        }

        [TestMethod]
        public async Task ConsumeForSaga_WithContentAndHandlerAndMessageContextAndData_ShouldBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContext();

            var sut = Build();

            var executed = false;

            await sut.Consume<object, Handler, object>(messagecontext, new object(), new RouteMethod<object, Handler>((ob, hd, mc, da) => { executed = true; return Task.CompletedTask; }), new Handler(), new object());

            executed.ShouldBeTrue();
        }
    }
}
