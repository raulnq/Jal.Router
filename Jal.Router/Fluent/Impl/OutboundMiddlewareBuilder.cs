using Jal.ChainOfResponsability.Intefaces;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OutboundMiddlewareBuilder : IOutboundMiddlewareBuilder
    {
        private readonly EndPoint _endpoint;
        public OutboundMiddlewareBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public void Add<TMiddleware>() where TMiddleware : IMiddlewareAsync<MessageContext>
        {
            _endpoint.MiddlewareTypes.Add(typeof(TMiddleware));
        }
    }
}