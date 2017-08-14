using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenMethodBuilder<out TBody, out THandler> : IRetryBuilder
    {
        IRetryBuilder When(Func<TBody, THandler, bool> method);

        IRetryBuilder When(Func<TBody, THandler, InboundMessageContext, bool> method);
    }
}