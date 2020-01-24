using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Impl;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractRouterConfigurationSource : IRouterConfigurationSource
    {
        private readonly List<Route> _routes = new List<Route>();

        private readonly List<Partition> _partition = new List<Partition>();

        private readonly List<EndPoint> _enpoints = new List<EndPoint>();

        private readonly List<SubscriptionToPublishSubscribeChannelResource> _subscriptions = new List<SubscriptionToPublishSubscribeChannelResource>();

        private readonly List<PublishSubscribeChannelResource> _publishsubscribechannels = new List<PublishSubscribeChannelResource>();

        private readonly List<PointToPointChannelResource> _pointtopointchannels = new List<PointToPointChannelResource>();

        private readonly List<Saga> _sagas = new List<Saga>();

        private readonly Origin _origin = new Origin();

        public Route[] GetRoutes()
        {
            return _routes.ToArray();
        }

        public Partition[] GetPartitions()
        {
            return _partition.ToArray();
        }

        public Saga[] GetSagas()
        {
            return _sagas.ToArray();
        }

        public EndPoint[] GetEndPoints()
        {
            foreach (var endPoint in _enpoints)
            {
                endPoint.SetOrigin(_origin);
            }
            return _enpoints.ToArray();
        }

        public SubscriptionToPublishSubscribeChannelResource[] GetSubscriptionsToPublishSubscribeChannelResource()
        {
            foreach (var subscription in _subscriptions)
            {
                if(subscription.Rules.Count==0)
                {
                    subscription.Rules.Add(new SubscriptionToPublishSubscribeChannelResourceRule($"origin='{_origin.Key}'", "$Default", true));
                }
            }
            return _subscriptions.ToArray();
        }

        public PointToPointChannelResource[] GetPointToPointChannelResources()
        {
            return _pointtopointchannels.ToArray();
        }

        public PublishSubscribeChannelResource[] GetPublishSubscribeChannelResources()
        {
            return _publishsubscribechannels.ToArray();
        }


        public IListenerRouteBuilder RegisterHandler(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new NameRouteBuilder(_routes, name);

            return builder;
        }

        public void RegisterSubscriptionToPublishSubscribeChannel(string subscription, string path, string connectionstring, Dictionary<string, string> properties, SubscriptionToPublishSubscribeChannelResourceRule rule = null)
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

            var channel = new SubscriptionToPublishSubscribeChannelResource(subscription, path, connectionstring, properties);

            if(rule!=null)
            {
                channel.Rules.Add(rule);
            }

            _subscriptions.Add(channel);
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

            var channel = new PointToPointChannelResource(path, connectionstring, properties);

            _pointtopointchannels.Add(channel);
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

            var channel = new PublishSubscribeChannelResource(path, connectionstring, properties);

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

        public ITimeoutBuilder RegisterSaga<TData>(Action<IFirstRouteBuilder<TData>> start, Action<IMiddleRouteBuilder<TData>> @continue = null, Action<ILastRouteBuilder<TData>> end = null) where TData : class, new()
        {
            return RegisterSaga<TData>(typeof(TData).Name.ToLower(), start, @continue, end);
        }

        public void RegisterOrigin(string name, string key="")
        {
            _origin.From = name;

            _origin.Key = key;
        }

        public IPartitionForChannelBuilder RegisterPartition(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var partition = new Partition(name);

            _partition.Add(partition);

            return new PartitionForChannelBuilder(partition);
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
