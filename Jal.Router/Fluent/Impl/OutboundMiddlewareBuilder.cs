using Jal.ChainOfResponsability;
using Jal.Router.Fluent.Interface;
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

        public void Add<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>
        {
            _endpoint.MiddlewareTypes.Add(typeof(TMiddleware));
        }
    }
}