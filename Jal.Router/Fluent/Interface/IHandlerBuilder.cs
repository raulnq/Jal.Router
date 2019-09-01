using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IHandlerBuilder<out TContent> 
    {
        IWhenHandlerBuilder Use<THandler, TConcreteHandler>(Action<IWithMethodBuilder<TContent, THandler>> action) where TConcreteHandler : THandler;
    }

    public interface IHandlerBuilder<out TContent, out TData>
    {
        IWhenHandlerBuilder Use<THandler, TConcreteHandler>(Action<IWithMethodBuilder<TContent, THandler, TData>> action) where TConcreteHandler : THandler;
    }
}