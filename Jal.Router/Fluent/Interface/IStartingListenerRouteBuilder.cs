using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IStartingListenerRouteBuilder<THandler, out TData>
    {
        IStartingNameRouteBuilder<THandler, TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}