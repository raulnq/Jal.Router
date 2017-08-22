using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class TypedSagaRouter : ITypedSagaRouter
    {
        public IStorage Storage { get; set; }

        private readonly IRouterInvoker _invoker;

        public TypedSagaRouter(IRouterInvoker invoker)
        {
            Storage = AbstractStorage.Instance;

            _invoker = invoker;
        }

        public void Continue<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route) where TData : class, new()
        {
            var data = Storage.Find(saga, context, route);

            if (data != null)
            {
                _invoker.Invoke(context, new [] { route }, data);

                Storage.Update(saga, context, route, data);
            }
            else
            {
                throw new ApplicationException($"No data {nameof(TData)} for {nameof(TContent)}, saga {saga.Name} and route {route.Name}");
            }
        }

        public void Start<TContent, TData>(Saga<TData> saga, InboundMessageContext<TContent> context, Route route) where TData : class, new()
        {
            var data = new TData();

            _invoker.Invoke(context, new [] { route }, data);

            Storage.Create(saga, context, route, data);
        }
    }
}