using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWithMethodBuilder<out TContent, out THandler>
    {
        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, Task> method);

        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, Task> method);
    }

    public interface IWithMethodBuilder<out TContent, out THandler, out TData>
    {
        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, Task> method);

        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, TData, Task> method);

        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, Task> method);

        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, TData, Task> method);

        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, Task> method, string status);

        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, TData, Task> method, string status);

        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, Task> method, string status);

        IWhenMethodBuilder<TContent, THandler> With(Func<TContent, THandler, MessageContext, TData, Task> method, string status);
    }
}