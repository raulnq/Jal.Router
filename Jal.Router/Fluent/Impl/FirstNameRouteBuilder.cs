using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class FirstNameRouteBuilder<TData> : IFirstNameRouteBuilder<TData>, IFirstListenerRouteBuilder<TData>, IListenerChannelBuilder
    {
        private readonly string _name;

        public List<Route> Routes { get; set; }

        private readonly Saga _saga;

        private readonly List<Channel> _channels;

        private Action<IListenerChannelBuilder> _channelbuilder;

        public FirstNameRouteBuilder(Saga saga, string name)
        {
            _saga = saga;

            _name = name;

            _channels = new List<Channel>();
        }

        public IHandlerBuilder<TContent, TData> ForMessage<TContent>()
        {
            _channelbuilder?.Invoke(this);

            var builder = new HandlerBuilder<TContent, TData>(_saga.InitialRoutes, _name, _channels);

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

            _channels.Add(new Channel(ChannelType.PointToPoint, typeof(TValueFinder), connectionstringprovider, path));
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

            _channels.Add(new Channel(ChannelType.SubscriptionToPublishSubscribe, typeof(TValueFinder), 
                connectionstringprovider, path, subscription));
        }

        public IFirstNameRouteBuilder<TData> ToListen(Action<IListenerChannelBuilder> channelbuilder)
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