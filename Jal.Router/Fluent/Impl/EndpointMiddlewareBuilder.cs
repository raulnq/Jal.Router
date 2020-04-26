using Jal.ChainOfResponsability;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndpointMiddlewareBuilder : IEndpointMiddlewareBuilder
    {
        private readonly EndPoint _endpoint;
        public EndpointMiddlewareBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public void Add<TMiddleware>() where TMiddleware : IAsyncMiddleware<MessageContext>
        {
            _endpoint.MiddlewareTypes.Add(typeof(TMiddleware));
        }
    }
}