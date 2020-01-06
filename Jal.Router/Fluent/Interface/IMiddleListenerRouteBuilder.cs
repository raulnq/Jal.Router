using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IMiddleListenerRouteBuilder<out TData>
    {
        IMiddleNameRouteBuilder<TData> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}