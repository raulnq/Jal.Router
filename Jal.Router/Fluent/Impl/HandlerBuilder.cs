using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class HandlerBuilder<TBody, THandler> : IHandlerBuilder<TBody, THandler>
    {
        private readonly Route<TBody, THandler> _route;

        public HandlerBuilder(Route<TBody, THandler> route)
        {
            _route = route;
        }

        public void ToBeHandledBy<TConcreteConsumer>(Action<IWhithMethodBuilder<TBody, THandler>> action) where TConcreteConsumer : THandler
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            _route.ConsumerType = typeof (TConcreteConsumer);

            var whitRouteBuilder = new WhitRouteBuilder<TBody, THandler>(_route);

            action(whitRouteBuilder);
        }
    }
}