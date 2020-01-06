using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerRouteBuilder
    {
        INameRouteBuilder ToListen(Action<IListenerChannelBuilder> channelbuilder);
    }
}