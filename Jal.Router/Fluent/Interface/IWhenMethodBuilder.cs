using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenMethodBuilder<out TBody, out THandler> : IOnRetryBuilder
    {
        IOnRetryBuilder When(Func<TBody, THandler, bool> method);

        IOnRetryBuilder When(Func<TBody, THandler, InboundMessageContext, bool> method);
    }
}