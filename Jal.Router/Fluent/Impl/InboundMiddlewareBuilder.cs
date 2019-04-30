using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class InboundMiddlewareBuilder : IInboundMiddlewareBuilder
    {
        private readonly Route _route;
        public InboundMiddlewareBuilder(Route route)
        {
            _route = route;
        }

        public void Add<TMiddleware>() where TMiddleware : IMiddlewareAsync<MessageContext>
        {
            _route.MiddlewareTypes.Add(typeof(TMiddleware));
        }
    }
}