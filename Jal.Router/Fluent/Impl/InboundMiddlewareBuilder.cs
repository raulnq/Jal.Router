using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface.Inbound;
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

        public void Add<TMiddleware>() where TMiddleware : IMiddleware
        {
            _route.MiddlewareTypes.Add(typeof(TMiddleware));
        }
    }
}