using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenMethodBuilder<out TBody, out TConsumer>
    {
        void When(Func<TBody, TConsumer, bool> method);

        void When<TContext>(Func<TBody, TConsumer, TContext, bool> method);
    }
}