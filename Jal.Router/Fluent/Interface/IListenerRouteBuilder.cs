using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerRouteBuilder<THandler>
    {
        INameRouteBuilder<THandler> ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}