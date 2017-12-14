using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWithMethodBuilder<out TContent, out THandler>
    {
        IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler> method);

        IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler, MessageContext> method);
    }

    public interface IWithMethodBuilder<out TContent, out THandler, out TData>
    {
        IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler> method);

        IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler, TData> method);

        IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler, MessageContext> method);

        IWhenMethodBuilder<TContent, THandler> With(Action<TContent, THandler, MessageContext, TData> method);
    }
}