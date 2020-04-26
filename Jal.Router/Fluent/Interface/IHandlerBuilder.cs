using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IHandlerBuilder
    {
        IWhenRouteBuilder With(Action<IForMessageRouteBuilder> action);
    }

    public interface IHandlerBuilder<out TData>
    {
        IWhenRouteBuilder With(Action<IForMessageRouteBuilder<TData>> action);
    }
}