using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Impl;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl.ValueFinder;
using Jal.Router.Interface;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public abstract class AbstractRouterConfigurationSource : IRouterConfigurationSource
    {
        private readonly List<Route> _routes = new List<Route>();

        private readonly List<Group> _group = new List<Group>();

        private readonly List<EndPoint> _enpoints = new List<EndPoint>();

        private readonly List<SubscriptionToPublishSubscribeChannel> _subscriptions = new List<SubscriptionToPublishSubscribeChannel>();

        private readonly List<PublishSubscribeChannel> _publishsubscribechannels = new List<PublishSubscribeChannel>();

        private readonly List<PointToPointChannel> _pointtopointchannels = new List<PointToPointChannel>();

        private readonly List<Saga> _sagas = new List<Saga>();

        private readonly Origin _origin = new Origin();

        public Route[] GetRoutes()
        {
            return _routes.ToArray();
        }

        public Group[] GetGroups()
        {
            return _group.ToArray();
        }

        public Saga[] GetSagas()
        {
            return _sagas.ToArray();
        }

        public EndPoint[] GetEndPoints()
        {
            foreach (var endPoint in _enpoints)
            {
                endPoint.Origin = _origin;
            }
            return _enpoints.ToArray();
        }

        public SubscriptionToPublishSubscribeChannel[] GetSubscriptionsToPublishSubscribeChannel()
        {
            foreach (var subscription in _subscriptions)
            {
                if(subscription.Rules.Count==0)
                {
                    subscription.Rules.Add(new SubscriptionToPublishSubscribeChannelRule() { Name = "$Default", Filter = $"origin='{_origin.Key}'", IsDefault = true });
                }
            }
            return _subscriptions.ToArray();
        }

        public PointToPointChannel[] GetPointToPointChannels()
        {
            return _pointtopointchannels.ToArray();
        }

        public PublishSubscribeChannel[] GetPublishSubscribeChannels()
        {
            return _publishsubscribechannels.ToArray();
        }


        public IListenerRouteBuilder<THandler> RegisterHandler<THandler>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new NameRouteBuilder<THandler>(_routes, name);

            return builder;
        }

        public void RegisterSubscriptionToPublishSubscribeChannel<TValueFinder>(string subscription, string path, Func<IValueFinder, string> connectionstringprovider, Dictionary<string, string> properties, SubscriptionToPublishSubscribeChannelRule rule = null)
            where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(subscription));
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }
            var channel = new SubscriptionToPublishSubscribeChannel(subscription, path)
            {
                ConnectionStringValueFinderType = typeof(TValueFinder),
                ConnectionStringProvider = connectionstringprovider,
                Properties = properties
            };

            if(rule!=null)
            {
                channel.Rules.Add(rule);
            }

            _subscriptions.Add(channel);
        }

        public void RegisterSubscriptionToPublishSubscribeChannel(string subscription, string path, string connectionstring, Dictionary<string, string> properties, SubscriptionToPublishSubscribeChannelRule rule = null)
        {
            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(subscription));
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            var channel = new SubscriptionToPublishSubscribeChannel(subscription, path)
            {
                ConnectionStringValueFinderType = typeof(NullValueFinder),
                ConnectionStringProvider = provider,
                Properties = properties
            };


            if (rule != null)
            {
                channel.Rules.Add(rule);
            }

            _subscriptions.Add(channel);
        }


        public void RegisterPointToPointChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider, Dictionary<string, string> properties)
            where TValueFinder : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }
            var channel = new PointToPointChannel(path)
            {
                ConnectionStringValueFinderType = typeof (TValueFinder),
                ConnectionStringProvider = connectionstringprovider,
                Properties = properties
            };

            _pointtopointchannels.Add(channel);
        }

        public void RegisterPointToPointChannel(string path, string connectionstring, Dictionary<string, string> properties)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            var channel = new PointToPointChannel(path)
            {
                ConnectionStringValueFinderType = typeof(NullValueFinder),
                ConnectionStringProvider = provider,
                Properties = properties
            };

            _pointtopointchannels.Add(channel);
        }

        public void RegisterPublishSubscribeChannel<TValueFinder>(string path, Func<IValueFinder, string> connectionstringprovider, Dictionary<string, string> properties)
            where TValueFinder : IValueFinder
        { 
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringprovider == null)
            {
                throw new ArgumentNullException(nameof(connectionstringprovider));
            }
            var channel = new PublishSubscribeChannel(path)
            {
                ConnectionStringValueFinderType = typeof(TValueFinder),
                ConnectionStringProvider = connectionstringprovider,
                Properties = properties
            };

            _publishsubscribechannels.Add(channel);
        }

        public void RegisterPublishSubscribeChannel(string path, string connectionstring, Dictionary<string, string> properties)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> provider = x => connectionstring;

            var channel = new PublishSubscribeChannel(path)
            {
                ConnectionStringValueFinderType = typeof(NullValueFinder),
                ConnectionStringProvider = provider,
                Properties = properties
            };

            _publishsubscribechannels.Add(channel);
        }

        public ITimeoutBuilder RegisterSaga<TData>(string name, Action<IFirstRouteBuilder<TData>> start, Action<IMiddleRouteBuilder<TData>> @continue=null, Action<ILastRouteBuilder<TData>> end = null) where TData : class, new()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (start==null)
            {
                throw new ArgumentNullException(nameof(start));
            }


            var saga = new Saga(name, typeof(TData));

            start(new FirstRouteBuilder<TData>(saga));

            @continue?.Invoke(new MiddleRouteBuilder<TData>(saga));

            end?.Invoke(new LastRouteBuilder<TData>(saga));

            _sagas.Add(saga);

            var timeoutbuilder = new TimeoutBuilder(saga);

            return timeoutbuilder;
        }

        public void RegisterOrigin(string name, string key="")
        {
            _origin.From = name;

            _origin.Key = key;
        }

        public IGroupForChannelBuilder RegisterGroup(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var group = new Group(name);

            _group.Add(group);

            return new GroupForChannelBuilder(group);
        }

        public INameEndPointBuilder RegisterEndPoint(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var endpoint = new EndPoint(name);

            _enpoints.Add(endpoint);

            var builder = new EndPointBuilder(endpoint);

            return builder;
        }
    }
}
