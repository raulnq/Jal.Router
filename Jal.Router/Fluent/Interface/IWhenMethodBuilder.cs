using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenMethodBuilder<out TBody, out THandler> : IRetryBuilder
    {
        IRetryBuilder When(Func<TBody, THandler, bool> method);

        IRetryBuilder When<TContext>(Func<TBody, THandler, TContext, bool> method);
    }
}