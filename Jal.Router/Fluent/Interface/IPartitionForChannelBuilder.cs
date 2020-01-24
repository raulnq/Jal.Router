using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IPartitionForChannelBuilder
    {
        IPartitionUntilBuilder ForPointToPointChannel(string path, string connectionstringprovider);
        IPartitionUntilBuilder ForSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring);
    }
}