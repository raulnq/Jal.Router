using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IStartNameRouteBuilder<THandler, out TData>
    {
        IHandlerBuilder<TContent, THandler> ForMessage<TContent>(Func<TData, string> key);
    }
}