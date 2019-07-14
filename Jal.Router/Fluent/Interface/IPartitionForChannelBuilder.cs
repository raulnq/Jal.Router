using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IPartitionForChannelBuilder
    {
        IPartitionUntilBuilder ForPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder;
        IPartitionUntilBuilder ForSubscriptionToPublishSubscribeChannel<TValueFinder>(string path, string subscription, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder;
    }
}