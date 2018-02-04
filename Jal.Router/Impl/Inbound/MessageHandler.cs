using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class MessageHandler : IMiddleware
    {
        private readonly IMessageRouter _router;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public MessageHandler(IMessageRouter router, IComponentFactory factory, IConfiguration configuration)
        {
            _router = router;
            _factory = factory;
            _configuration = configuration;
        }


        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            if (_configuration.Storage.SaveMessage)
            {
                storage.Create(context);
            }
            
            _router.Route(context, parameter.Route);

            next();
        }
    }
}