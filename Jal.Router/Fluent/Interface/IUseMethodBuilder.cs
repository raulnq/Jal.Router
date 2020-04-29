using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IUseMethodBuilder<out TContent>
    {
        IWhenMethodBuilder<TContent> Use(Func<TContent, MessageContext, Task> method);

        IWhenMethodBuilder<TContent, THandler> Use<THandler, TConcreteHandler>(Func<TContent, THandler, MessageContext, Task> method) where TConcreteHandler : THandler;
    }

    public interface IUseMethodBuilder<out TContent, out TData>
    {
        IWhenMethodBuilderWithData<TContent, TData> Use(Func<TContent, MessageContext, TData, Task> method, string status = null);

        IWhenMethodBuilderWithData<TContent, THandler, TData> Use<THandler, TConcreteHandler>(Func<TContent, THandler, MessageContext, TData, Task> method, string status = null) where TConcreteHandler : THandler;
    }
}