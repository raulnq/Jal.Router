using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IFirstListenerRouteBuilder<out TData>
    {
        IFirstNameRouteBuilder<TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}