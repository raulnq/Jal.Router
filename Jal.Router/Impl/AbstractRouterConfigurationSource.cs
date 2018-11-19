using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Impl;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{
    public abstract class AbstractRouterConfigurationSource : IRouterConfigurationSource
    {
        private readonly List<Route> _routes = new List<Route>();

        private readonly List<EndPoint> _enpoints = new List<EndPoint>();

        private readonly List<SubscriptionToPublishSubscribeChannel> _subscriptions = new List<SubscriptionToPublishSubscribeChannel>();

        private readonly List<PublishSubscribeChannel> _publishsubscriberchannels = new List<PublishSubscribeChannel>();

        private readonly List<PointToPointChannel> _pointtopointchannels = new List<PointToPointChannel>();

        private readonly List<Saga> _sagas = new List<Saga>();

        private readonly Origin _origin = new Origin();

        public Route[] GetRoutes()
        {
            return _routes.ToArray();
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
            return _publishsubscriberchannels.ToArray();
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

        public void RegisterSubscriptionToPublishSubscriberChannel<TExtractorConectionString>(string subscription, string path, Func<IValueFinder, string> connectionstringextractor, SubscriptionToPublishSubscribeChannelRule rule = null)
            where TExtractorConectionString : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(subscription))
            {
                throw new ArgumentNullException(nameof(subscription));
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            var channel = new SubscriptionToPublishSubscribeChannel(subscription, path)
            {
                ConnectionStringValueFinderType = typeof(TExtractorConectionString),
                ConnectionStringProvider = connectionstringextractor
            };

            if(rule!=null)
            {
                channel.Rules.Add(rule);
            }

            _subscriptions.Add(channel);
        }

        public void RegisterSubscriptionToPublishSubscriberChannel(string subscription, string path, string connectionstring, SubscriptionToPublishSubscribeChannelRule rule = null)
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

            Func<IValueFinder, string> extractor = x => connectionstring;

            var channel = new SubscriptionToPublishSubscribeChannel(subscription, path)
            {
                ConnectionStringValueFinderType = typeof(NullValueFinder),
                ConnectionStringProvider = extractor,
            };


            if (rule != null)
            {
                channel.Rules.Add(rule);
            }

            _subscriptions.Add(channel);
        }


        public void RegisterPointToPointChannel<TExtractorConectionString>(string path, Func<IValueFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            var channel = new PointToPointChannel(path)
            {
                ConnectionStringValueFinderType = typeof (TExtractorConectionString),
                ConnectionStringProvider = connectionstringextractor
            };

            _pointtopointchannels.Add(channel);
        }

        public void RegisterPointToPointChannel(string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            var channel = new PointToPointChannel(path)
            {
                ConnectionStringValueFinderType = typeof(NullValueFinder),
                ConnectionStringProvider = extractor
            };

            _pointtopointchannels.Add(channel);
        }

        public void RegisterPublishSubscriberChannel<TExtractorConectionString>(string path, Func<IValueFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueFinder
        { 
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            var channel = new PublishSubscribeChannel(path)
            {
                ConnectionStringValueFinderType = typeof(TExtractorConectionString),
                ConnectionStringProvider = connectionstringextractor
            };

            _publishsubscriberchannels.Add(channel);
        }

        public void RegisterPublishSubscriberChannel(string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueFinder, string> extractor = x => connectionstring;

            var channel = new PublishSubscribeChannel(path)
            {
                ConnectionStringValueFinderType = typeof(NullValueFinder),
                ConnectionStringProvider = extractor
            };

            _publishsubscriberchannels.Add(channel);
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
