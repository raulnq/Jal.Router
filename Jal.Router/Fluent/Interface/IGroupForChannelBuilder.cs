using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IGroupForChannelBuilder
    {
        IGroupUntilBuilder ForPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder;
        IGroupUntilBuilder ForSubscriptionToPublishSubscribeChannel<TValueFinder>(string path, string subscription, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder;
    }
}