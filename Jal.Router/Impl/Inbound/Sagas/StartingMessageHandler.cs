using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public class StartingMessageHandler : IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IMessageRouter _router;

        private readonly IConfiguration _configuration;

        public StartingMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration)
        {
            _factory = factory;
            _router = router;
            _configuration = configuration;
        }

        public void Execute(MessageContext context, Action next, MiddlewareParameter parameter)
        {
            var data = Activator.CreateInstance(parameter.Saga.DataType);

            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            storage.StartSaga(context, data);

            _router.Route(context, parameter.Route, data, parameter.Saga.DataType);

            storage.ContinueSaga(context, data);
        }
    }
}