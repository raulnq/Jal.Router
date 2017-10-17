using System;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class MessageHandler : IMiddleware
    {
        private readonly IMessageRouter _router;

        public MessageHandler(IMessageRouter router)
        {
            _router = router;
        }


        public void Execute<TContent>(IndboundMessageContext<TContent> context, Action next, MiddlewareParameter parameter)
        {
            _router.Route(context, parameter.Route);

            next();
        }
    }
}