using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class HandlerBuilder<TBody, THandler> : IHandlerBuilder<TBody, THandler>, IWhenHandlerBuilder<TBody>
    {
        private readonly Route<TBody, THandler> _route;

        public HandlerBuilder(Route<TBody, THandler> route)
        {
            _route = route;
        }

        public IWhenHandlerBuilder<TBody> ToBeHandledBy<TConcreteConsumer>(Action<IWithMethodBuilder<TBody, THandler>> action) where TConcreteConsumer : THandler
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _route.ConsumerType = typeof (TConcreteConsumer);

            var whitRouteBuilder = new WhitRouteBuilder<TBody, THandler>(_route);

            action(whitRouteBuilder);

            return this;
        }

        public void When(Func<TBody, InboundMessageContext, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            _route.When = condition;
        }
    }
}