using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerChannelBuilder
    {
        void AddPointToPointChannel(string path, string connectionstring, Type adapter=null, Type type=null);
        void AddSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring, Type adapter = null, Type type = null);
    }
}