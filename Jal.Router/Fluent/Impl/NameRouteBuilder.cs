using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NameRouteBuilder<THandler> : INameRouteBuilder<THandler>, IListenerRouteBuilder<THandler>, IListenerChannelBuilder
    {
        private readonly List<Route> _routes;

        private readonly string _name;

        private readonly IList<Channel> _channels;

        private Action<IListenerChannelBuilder> _channelbuilder;

        public NameRouteBuilder(List<Route> routes, string name)
        {
            _routes = routes;

            _name = name;

            _channels = new List<Channel>();
        }

        public IHandlerBuilder<TContent, THandler> ForMessage<TContent>()
        {
            _channelbuilder?.Invoke(this);

            var value = new Route<TContent, THandler>(_name);

            value.Channels.AddRange(_channels);

            var builder = new HandlerBuilder<TContent, THandler>(value);

            _routes.Add(value);

            return builder;
        }

        public void AddPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }

            _channels.Add(new Channel(ChannelType.PointToPoint)
            {
                ToPath = path,

                ToConnectionStringProvider = connectionstringprovider,

                ConnectionStringValueFinderType = typeof(TValueFinder)
            });
        }

        public void AddSubscriptionToPublishSubscribeChannel<TValueFinder>(string path, string subscription, Func<IValueFinder, string> connectionstringprovider) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _channels.Add(new Channel(ChannelType.SubscriptionToPublishSubscribe)
            {
                ToPath = path,

                ToConnectionStringProvider = connectionstringprovider,

                ConnectionStringValueFinderType = typeof(TValueFinder),

                ToSubscription = subscription
            });
        }

        public INameRouteBuilder<THandler> ToListen(Action<IListenerChannelBuilder> channelbuilder)
        {
            if (channelbuilder == null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            _channelbuilder = channelbuilder;

            return this;
        }
    }
}