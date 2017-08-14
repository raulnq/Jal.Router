using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWithMethodBuilder<out TBody, out THandler>
    {
        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler> method);

        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, InboundMessageContext> method);
    }
}