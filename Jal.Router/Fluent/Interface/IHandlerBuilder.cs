using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IHandlerBuilder<out TContent> 
    {
        IWhenHandlerBuilder Use(Action<IWithMethodBuilder<TContent>> action);
    }

    public interface IHandlerBuilder<out TContent, out TData>
    {
        IWhenHandlerBuilder Use(Action<IWithMethodBuilder<TContent, TData>> action);
    }
}