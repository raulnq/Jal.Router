using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class LastNameRouteBuilder<TData> : ILastNameRouteBuilder<TData>, ILastListenerRouteBuilder<TData>, IListenerChannelBuilder
    {
        private readonly string _name;

        public List<Route> Routes { get; set; }

        private readonly Saga _saga;

        private readonly List<Channel> _channels;

        private Action<IListenerChannelBuilder> _channelbuilder;

        public LastNameRouteBuilder(Saga saga, string name)
        {
            _saga = saga;

            _name = name;

            _channels = new List<Channel>();
        }

        public IHandlerBuilder<TContent, TData> ForMessage<TContent>()
        {
            _channelbuilder?.Invoke(this);

            var builder = new HandlerBuilder<TContent, TData>(_saga.FinalRoutes, _name, _channels);

            return builder;
        }

        public void AddPointToPointChannel(string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            _channels.Add(new Channel(ChannelType.PointToPoint, connectionstring, path));
        }

        public void AddSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _channels.Add(new Channel(ChannelType.SubscriptionToPublishSubscribe, connectionstring, path, subscription));
        }

        public ILastNameRouteBuilder<TData> ToListen(Action<IListenerChannelBuilder> channelbuilder)
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