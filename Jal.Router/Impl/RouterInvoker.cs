using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RouterInvoker : IRouterInvoker
    {
        private readonly ITypedRouter _router;

        public RouterInvoker(ITypedRouter router)
        {
            _router = router;
        }

        public void Invoke<TContent>(InboundMessageContext<TContent> context, Route[] routes)
        {
            var routemethod = typeof(ITypedRouter).GetMethods().First(x => x.Name == nameof(ITypedRouter.Route) && x.GetParameters().Count() == 2);

            foreach (var route in routes)
            {
                var genericroutemethod = routemethod?.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType);

                genericroutemethod?.Invoke(_router, new object[] { context, route });
            }
        }

        public void Invoke<TContent, TData>(InboundMessageContext<TContent> context, Route[] routes, TData data) where TData : class, new()
        {
            var routemethod = typeof(ITypedRouter).GetMethods().First(x => x.Name == nameof(ITypedRouter.Route) && x.GetParameters().Count() == 3);

            foreach (var route in routes)
            {
                var genericroutemethod = routemethod?.MakeGenericMethod(route.BodyType, route.ConsumerInterfaceType, typeof(TData));

                genericroutemethod?.Invoke(_router, new object[] { context, route, data });
            }
        }
    }
}