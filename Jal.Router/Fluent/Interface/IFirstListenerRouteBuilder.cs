using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IFirstListenerRouteBuilder<THandler, out TData>
    {
        IFirstNameRouteBuilder<THandler, TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}