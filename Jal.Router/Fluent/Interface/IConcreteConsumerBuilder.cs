using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IConcreteConsumerBuilder<out TBody, TConsumer> 
    {
        void ToBeConsumedBy<TConcreteConsumer>(Action<IWhithMethodBuilder<TBody, TConsumer>> action) where TConcreteConsumer : TConsumer;
    }
}