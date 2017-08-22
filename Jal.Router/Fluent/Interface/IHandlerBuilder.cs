using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IHandlerBuilder<out TContent, THandler> 
    {
        IWhenHandlerBuilder<TContent> ToBeHandledBy<TConcreteHandler>(Action<IWithMethodBuilder<TContent, THandler>> action) where TConcreteHandler : THandler;
    }

    public interface IHandlerBuilder<out TContent, THandler, out TData>
    {
        IWhenHandlerBuilder<TContent> ToBeHandledBy<TConcreteHandler>(Action<IWithMethodBuilder<TContent, THandler, TData>> action) where TConcreteHandler : THandler;
    }
}