using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IStartNameRouteBuilder<THandler, out TData>
    {
        IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>(Func<TData, InboundMessageContext, string> key);
    }
}