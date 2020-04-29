using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerRouteBuilder
    {
        IHandlerBuilder ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }

    public interface IListenerRouteBuilder<out TData>
    {
        IHandlerBuilder<TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}