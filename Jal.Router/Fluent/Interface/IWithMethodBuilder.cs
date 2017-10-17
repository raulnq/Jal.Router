using System;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IWithMethodBuilder<out TBody, out THandler>
    {
        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler> method);

        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, MessageContext> method);
    }

    public interface IWithMethodBuilder<out TBody, out THandler, out TData>
    {
        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler> method);

        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, TData> method);

        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, MessageContext> method);

        IWhenMethodBuilder<TBody, THandler> With(Action<TBody, THandler, MessageContext, TData> method);
    }
}