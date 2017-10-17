using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Impl;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Interface.Outbound;
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
                subscription.Origin = _origin;
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


        public INameRouteBuilder<THandler> RegisterRoute<THandler>(string name="")
        {
            var builder = new NameRouteBuilder<THandler>(name, _routes);

            return builder;
        }

        public void RegisterSubscriptionToPublishSubscriberChannel<TExtractorConectionString>(string name, string path, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            var subscription = new SubscriptionToPublishSubscribeChannel(name)
            {
                ConnectionStringExtractorType = typeof(TExtractorConectionString),
                ToConnectionStringExtractor = connectionstringextractor,
                TopicPath = path
            };

            _subscriptions.Add(subscription);
        }

        public void RegisterPointToPointChannel<TExtractorConectionString>(string name, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            var queue = new PointToPointChannel(name)
            {
                ConnectionStringExtractorType = typeof (TExtractorConectionString),
                ToConnectionStringExtractor = connectionstringextractor
            };

            _pointtopointchannels.Add(queue);
        }

        public void RegisterPublishSubscriberChannel<TExtractorConectionString>(string name, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        { 
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            var topic = new PublishSubscribeChannel(name)
            {
                ConnectionStringExtractorType = typeof(TExtractorConectionString),
                ToConnectionStringExtractor = connectionstringextractor
            };

            _publishsubscriberchannels.Add(topic);
        }

        public void RegisterSaga<TData>(string name, Action<IStartingRouteBuilder<TData>> start, Action<INextRouteBuilder<TData>> @continue=null) where TData : class, new()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (start==null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (@continue == null)
            {
                throw new ArgumentNullException(nameof(@continue));
            }

            var saga = new Saga<TData>(name);

            start(new StartingRouteBuilder<TData>(saga));

            @continue(new NextRouteBuilder<TData>(saga));

            _sagas.Add(saga);
        }

        public void RegisterOrigin(string name, string key="")
        {
            _origin.Name = name;

            _origin.Key = key;
        }

        public INameEndPointBuilder RegisterEndPoint(string name = "")
        {
            var endpoint = new EndPoint(name);

            _enpoints.Add(endpoint);

            var builder = new EndPointBuilder(endpoint);

            return builder;
        }

        public void RegisterEndPoint<TExtractor, T>(string name = "") where TExtractor : IEndPointSettingFinder<T>
        {
            var endpoint = new EndPoint(name);

            _enpoints.Add(endpoint);

            endpoint.ExtractorType = typeof(TExtractor);

            endpoint.MessageType = typeof(T);
        }
    }
}
