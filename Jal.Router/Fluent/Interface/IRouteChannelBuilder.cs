using System;
using System.Collections.Generic;

namespace Jal.Router.Fluent.Interface
{
    public interface IRouteChannelBuilder
    {
        IChannelIWhenBuilder AddPointToPointChannel(string path, string connectionstring, Type adapter=null, Type type=null, IDictionary<string, object> properties = null);
        IChannelIWhenBuilder AddSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring, Type adapter = null, Type type = null, IDictionary<string, object> properties = null);
    }
}