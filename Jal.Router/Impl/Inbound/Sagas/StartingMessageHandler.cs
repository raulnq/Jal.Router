using System;
using System.Linq;
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

        public void Execute<TContent>(MessageContext<TContent> context, Action next, MiddlewareParameter parameter)
        {
            var routemethod = typeof(StartingMessageHandler).GetMethods().First(x => x.Name == nameof(StartingMessageHandler.Start));

            var genericroutemethod = routemethod?.MakeGenericMethod(parameter.Route.ContentType, parameter.Saga.DataType);

            genericroutemethod?.Invoke(this, new object[] { parameter.Saga, context, parameter.Route });

            next();
        }

        public void Start<TContent, TData>(Saga<TData> saga, MessageContext<TContent> context, Route route) where TData : class, new()
        {
            var data = new TData();

            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            storage.Create(context, data);

            _router.Route(context, route, data);

            if (!_configuration.Storage.ManualSagaSave)
            {
                storage.Update(context, data);
            }            
        }
    }
}