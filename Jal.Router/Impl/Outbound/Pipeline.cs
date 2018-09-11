using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class Pipeline : IPipeline
    {
        private readonly IComponentFactory _factory;

        public Pipeline(IComponentFactory factory)
        {
            _factory = factory;
        }

        public object Execute(Type[] middlewares, MessageContext message, Options options, string outboundtype, Type resulttype=null)
        {
            return GetNext().Invoke(message, new MiddlewareContext() {MiddlewareTypes= middlewares, Index=0,  Options = options, OutboundType = outboundtype, ResultType = resulttype });
        }

        private Func<MessageContext, MiddlewareContext, object> GetNext()
        {
            return (c,p) =>
            {
                if (p.Index < p.MiddlewareTypes.Length)
                {
                    var middleware = _factory.Create<IMiddleware>(p.MiddlewareTypes[p.Index]);
                    p.Index++;
                    return middleware.Execute(c, GetNext(), p);
                }
                return null;
            };
        }
    }
}