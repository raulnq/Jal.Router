using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IMiddleListenerRouteBuilder<THandler, out TData>
    {
        IMiddleNameRouteBuilder<THandler, TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}