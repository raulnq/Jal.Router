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
    public class NextMessageHandler : IMiddleware
    {
        private readonly IComponentFactory _factory;

        private readonly IMessageRouter _router;

        private readonly IConfiguration _configuration;

        public NextMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration)
        {
            _factory = factory;
            _router = router;
            _configuration = configuration;
        }

        public void Execute<TContent>(InboundMessageContext<TContent> context, Action next, MiddlewareParameter parameter)
        {
            var routemethod = typeof(NextMessageHandler).GetMethods().First(x => x.Name == nameof(NextMessageHandler.Continue));

            var genericroutemethod = routemethod?.MakeGenericMethod(parameter.Route.BodyType, parameter.Saga.DataType);

            genericroutemethod?.Invoke(this, new object[] { parameter.Saga, context, parameter.Route });

            next();
        }

        public void Continue<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route) where TData : class, new()
        {
            var storage = _factory.Create<IStorage>(_configuration.StorageType);

            var data = storage.Find<TData>(context);

            if (data != null)
            {
                _router.Route(context, route, data);

                if (!_configuration.Storage.ManualSagaSave)
                {
                    storage.Update(context, data);
                }
            }
            else
            {
                throw new ApplicationException($"No data {typeof(TData).FullName} for {typeof(TContent).FullName}, saga {saga.Name} and route {route.Name}");
            }
        }
    }
}