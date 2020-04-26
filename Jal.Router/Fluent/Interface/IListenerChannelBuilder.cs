using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerChannelBuilder
    {
        IChannelIWhenBuilder AddPointToPointChannel(string path, string connectionstring, Type adapter=null, Type type=null);
        IChannelIWhenBuilder AddSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring, Type adapter = null, Type type = null);
    }
}