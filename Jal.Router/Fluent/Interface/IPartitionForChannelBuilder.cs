using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IPartitionForChannelBuilder
    {
        IPartitionUntilBuilder ForPointToPointChannel(string path, string connectionstringprovider, Type adapter=null, Type type = null);
        IPartitionUntilBuilder ForSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring, Type adapter = null, Type type = null);
    }
}