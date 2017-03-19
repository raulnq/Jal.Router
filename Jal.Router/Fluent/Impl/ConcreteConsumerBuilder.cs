using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class ConcreteConsumerBuilder<TBody, TConsumer> : IConcreteConsumerBuilder<TBody, TConsumer>
    {
        private readonly Route<TBody, TConsumer> _route;

        public ConcreteConsumerBuilder(Route<TBody, TConsumer> route)
        {
            _route = route;
        }

        public void ToBeConsumedBy<TConcreteConsumer>(Action<IWhithMethodBuilder<TBody, TConsumer>> action) where TConcreteConsumer : TConsumer
        {
            _route.ConsumerType = typeof (TConcreteConsumer);

            var whitRouteBuilder = new WhitRouteBuilder<TBody, TConsumer>(_route);

            action(whitRouteBuilder);
        }
    }
}