using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IWithMethodBuilder<out TContent>
    {
        IWhenMethodBuilder<TContent> With(Func<TContent, MessageContext, Task> method);

        IWhenMethodBuilder<TContent, THandler> With<THandler, TConcreteHandler>(Func<TContent, THandler, MessageContext, Task> method) where TConcreteHandler : THandler;
    }

    public interface IWithMethodBuilder<out TContent, out TData>
    {
        IWhenMethodBuilderWithData<TContent, TData> With(Func<TContent, MessageContext, TData, Task> method, string status = null);

        IWhenMethodBuilderWithData<TContent, THandler, TData> With<THandler, TConcreteHandler>(Func<TContent, THandler, MessageContext, TData, Task> method, string status = null) where TConcreteHandler : THandler;
    }
}