using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IListenerChannelBuilder
    {
        void AddPointToPointChannel(string path, string connectionstring);
        void AddSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring);
    }
}