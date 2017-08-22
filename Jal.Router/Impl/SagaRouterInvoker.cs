using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class SagaRouterInvoker : ISagaRouterInvoker
    {
        private readonly ITypedSagaRouter _router;

        public SagaRouterInvoker(ITypedSagaRouter router)
        {
            _router = router;
        }

        public void Continue<TContent>(Saga saga, InboundMessageContext<TContent> context, Route route)
        {
            var routemethod = typeof(ITypedSagaRouter).GetMethods().First(x => x.Name == nameof(ITypedSagaRouter.Continue));

            var genericroutemethod = routemethod?.MakeGenericMethod(route.BodyType, saga.DataType);

            genericroutemethod?.Invoke(_router, new object[] { saga, context, route });
        }

        public void Start<TContent>(Saga saga, InboundMessageContext<TContent> context, Route route)
        {
            var routemethod = typeof(ITypedSagaRouter).GetMethods().First(x => x.Name == nameof(ITypedSagaRouter.Start));

            var genericroutemethod = routemethod?.MakeGenericMethod(route.BodyType, saga.DataType);

            genericroutemethod?.Invoke(_router, new object[] { saga, context, route });
        }
    }
}