using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenMethodBuilder<out TBody, out THandler>
    {
        void When(Func<TBody, THandler, bool> method);

        void When<TContext>(Func<TBody, THandler, TContext, bool> method);
    }
}