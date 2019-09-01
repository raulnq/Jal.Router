using System;

namespace Jal.Router.Fluent.Interface
{
    public interface ILastListenerRouteBuilder<out TData>
    {
        ILastNameRouteBuilder<TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}