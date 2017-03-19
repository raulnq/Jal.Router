using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhithMethodBuilder<out TBody, out TConsumer>
    {
        IWhenMethodBuilder<TBody, TConsumer> With(Action<TBody, TConsumer> method);

        IWhenMethodBuilder<TBody, TConsumer> With<TContext>(Action<TBody, TConsumer, TContext> method);
    }
}