using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class Pipeline : IPipeline
    {
        private readonly IComponentFactory _factory;

        public Pipeline(IComponentFactory factory)
        {
            _factory = factory;
        }

        public void Execute(Type[] middlewares, MessageContext context)
        {
            GetNext().Invoke(context, new MiddlewareContext() { MiddlewareTypes = middlewares, Index  = 0 });
        }

        private Action<MessageContext, MiddlewareContext> GetNext()
        {
            return (c,p) =>
            {
                if (p.Index < p.MiddlewareTypes.Length)
                {
                    var middleware = _factory.Create<IMiddleware>(p.MiddlewareTypes[p.Index]);
                    p.Index++;
                    middleware.Execute(c, GetNext(), p);
                }
            };
        }
    }
}