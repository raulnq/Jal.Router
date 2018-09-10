using System;

namespace Jal.Router.Fluent.Interface
{
    public interface ILastListenerRouteBuilder<THandler, out TData>
    {
        ILastNameRouteBuilder<THandler, TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}