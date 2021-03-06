﻿using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{

    public class ListenerRouteBuilder : IListenerRouteBuilder, IRouteChannelBuilder
    {
        private readonly Route _route;

        public ListenerRouteBuilder(Route route)
        {
            _route = route;
        }

        public IChannelIWhenBuilder AddPointToPointChannel(string path, string connectionstring, Type adapter=null, Type type = null, IDictionary<string, object> properties = null, string trasnportname = null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (adapter != null && !typeof(IMessageAdapter).IsAssignableFrom(adapter))
            {
                throw new InvalidOperationException("The adapter type is not valid");
            }

            if (type != null && !typeof(IPointToPointChannel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The channel type is not valid");
            }

            var channel = new Channel(ChannelType.PointToPoint, connectionstring, path, adapter, type, trasnportname);

            if(properties!=null)
            {
                foreach (var item in properties)
                {
                    channel.Properties.Add(item.Key, item.Value);
                }
            }

            _route.Channels.Add(channel);

            return new ChannelWhenBuilder(channel);
        }

        public IChannelIWhenBuilder AddSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring, Type adapter=null, Type type = null, IDictionary<string, object> properties = null, string trasnportname = null)
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
            if(adapter !=null && !typeof(IMessageAdapter).IsAssignableFrom(adapter))
            {
                throw new InvalidOperationException("The adapter type is not valid");
            }
            if (type != null && !typeof(ISubscriptionToPublishSubscribeChannel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The channel type is not valid");
            }

            var channel = new Channel(ChannelType.SubscriptionToPublishSubscribe, connectionstring, path, subscription, adapter, type, trasnportname);


            if (properties != null)
            {
                foreach (var item in properties)
                {
                    channel.Properties.Add(item.Key, item.Value);
                }
            }


            _route.Channels.Add(channel);

            return new ChannelWhenBuilder(channel);
        }

        public IHandlerBuilder ToListen(Action<IRouteChannelBuilder> channelbuilder)
        {
            if (channelbuilder == null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            channelbuilder.Invoke(this);

            var builder = new HandlerBuilder(_route);

            return builder;
        }
    }

    public class ListenerRouteBuilder<TData> : IListenerRouteBuilder<TData>, IRouteChannelBuilder
    {
        private readonly Route _route;

        public ListenerRouteBuilder(Route route)
        {
            _route = route;
        }

        public IChannelIWhenBuilder AddPointToPointChannel(string path, string connectionstring, Type adapter = null, Type type = null, IDictionary<string, object> properties = null, string trasnportname= null)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (adapter != null && !typeof(IMessageAdapter).IsAssignableFrom(adapter))
            {
                throw new InvalidOperationException("The adapter type is not valid");
            }

            if (type != null && !typeof(IPointToPointChannel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The channel type is not valid");
            }

            var channel = new Channel(ChannelType.PointToPoint, connectionstring, path, adapter, type, trasnportname);

            if (properties != null)
            {
                foreach (var item in properties)
                {
                    channel.Properties.Add(item.Key, item.Value);
                }
            }

            _route.Channels.Add(channel);

            return new ChannelWhenBuilder(channel);
        }

        public IChannelIWhenBuilder AddSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring, Type adapter = null, Type type = null, IDictionary<string, object> properties = null, string trasnportname = null)
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
            if (adapter != null && !typeof(IMessageAdapter).IsAssignableFrom(adapter))
            {
                throw new InvalidOperationException("The adapter type is not valid");
            }
            if (type != null && !typeof(ISubscriptionToPublishSubscribeChannel).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The channel type is not valid");
            }

            var channel = new Channel(ChannelType.SubscriptionToPublishSubscribe, connectionstring, path, subscription, adapter, type, trasnportname);


            if (properties != null)
            {
                foreach (var item in properties)
                {
                    channel.Properties.Add(item.Key, item.Value);
                }
            }


            _route.Channels.Add(channel);

            return new ChannelWhenBuilder(channel);
        }

        public IHandlerBuilder<TData> ToListen(Action<IRouteChannelBuilder> channelbuilder)
        {
            if (channelbuilder == null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            channelbuilder.Invoke(this);

            var builder = new HandlerBuilder<TData>(_route);

            return builder;
        }
    }
}