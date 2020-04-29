using Jal.Router.Impl;
using Jal.Router.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{

    [TestClass]
    public class RouteEntryMessageHandlerTests
    {
        private RouteEntryMessageHandler Build()
        {
            return new RouteEntryMessageHandler(new NullLogger());
        }

        [TestMethod]
        public async Task Handle_WithNoInit_ShouldNotBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContextFromListen();

            var sut = Build();

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { }));
        }

        [TestMethod]
        public async Task Handle_WithNullInit_ShouldNotBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContextFromListen();

            var sut = Build();

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { {"init",null} }));
        }

        [TestMethod]
        public async Task Handle_WithNullInit_ShouldBeExecuted()
        {
            var messagecontext = Builder.CreateMessageContextFromListen();

            var sut = Build();

            var executed = false;

            Func<MessageContext, Model.Handler, Task> init = (mc, h) => { executed = true; return Task.CompletedTask; };

            await sut.Handle(messagecontext, new Model.Handler(typeof(object), new Dictionary<string, object>() { { "init", init } }));

            executed.ShouldBeTrue();
        }
    }
}
