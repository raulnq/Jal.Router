using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class FirstNameRouteBuilder<THandler, TData> : IFirstNameRouteBuilder<THandler, TData>, IFirstListenerRouteBuilder<THandler, TData>, IListenerChannelBuilder
    {
        private readonly string _name;

        public List<Route> Routes { get; set; }

        private readonly Saga _saga;

        private readonly IList<Channel> _channels;

        private Action<IListenerChannelBuilder> _channelbuilder;

        public FirstNameRouteBuilder(Saga saga, string name)
        {
            _saga = saga;

            _name = name;

            _channels = new List<Channel>();
        }

        public IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>()
        {
            _channelbuilder?.Invoke(this);

            var value = new Route<TContent, THandler>(_saga, _name);

            value.Channels.AddRange(_channels);

            var builder = new HandlerBuilder<TContent, THandler, TData>(value);

            _saga.FirstRoute = value;

            return builder;
        }

        public void AddPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringextractor) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }

            _channels.Add(new Channel(ChannelType.PointToPoint)
            {
                ToPath = path,

                ToConnectionStringProvider = connectionstringextractor,

                ConnectionStringValueFinderType = typeof(TValueFinder)
            });
        }

        public void AddSubscriptionToPublishSubscribeChannel<TValueFinder>(string path, string subscription, Func<IValueFinder, string> connectionstringextractor) where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _channels.Add(new Channel(ChannelType.PublishSubscriber)
            {
                ToPath = path,

                ToConnectionStringProvider = connectionstringextractor,

                ConnectionStringValueFinderType = typeof(TValueFinder),

                ToSubscription = subscription
            });
        }

        public IFirstNameRouteBuilder<THandler, TData> ToListen(Action<IListenerChannelBuilder> channelbuilder)
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