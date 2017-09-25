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

        private readonly List<EndPoint> _enpoints = new List<EndPoint>();

        private readonly List<Subscription> _subscriptions = new List<Subscription>();

        private readonly List<Topic> _topics = new List<Topic>();

        private readonly List<Queue> _queues = new List<Queue>();

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

        public Subscription[] GetSubscriptions()
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Origin = _origin;
            }
            return _subscriptions.ToArray();
        }

        public Queue[] GetQueues()
        {
            return _queues.ToArray();
        }

        public Topic[] GetTopics()
        {
            return _topics.ToArray();
        }


        public INameRouteBuilder<THandler> RegisterRoute<THandler>(string name="")
        {
            var builder = new NameRouteBuilder<THandler>(name, _routes);

            return builder;
        }

        public void RegisterSubscription<TExtractorConectionString>(string name, string topicpath, Func<IValueSettingFinder, string> connectionstringextractor)
            where TExtractorConectionString : IValueSettingFinder
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(topicpath))
            {
                throw new ArgumentNullException(nameof(topicpath));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            var subscription = new Subscription(name)
            {
                ConnectionStringExtractorType = typeof(TExtractorConectionString),
                ToConnectionStringExtractor = connectionstringextractor,
                TopicPath = topicpath
            };

            _subscriptions.Add(subscription);
        }

        public void RegisterQueue<TExtractorConectionString>(string name, Func<IValueSettingFinder, string> connectionstringextractor)
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
            var queue = new Queue(name)
            {
                ConnectionStringExtractorType = typeof (TExtractorConectionString),
                ToConnectionStringExtractor = connectionstringextractor
            };

            _queues.Add(queue);
        }

        public void RegisterTopic<TExtractorConectionString>(string name, Func<IValueSettingFinder, string> connectionstringextractor)
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
            var topic = new Topic(name)
            {
                ConnectionStringExtractorType = typeof(TExtractorConectionString),
                ToConnectionStringExtractor = connectionstringextractor
            };

            _topics.Add(topic);
        }

        public void RegisterSaga<TData>(string name, Action<IStartRegisterRouteBuilder<TData>> start, Action<IContinueRegisterRouteBuilder<TData>> @continue=null) where TData : class, new()
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

            start(new StartRegisterRouteBuilder<TData>(saga));

            @continue(new ContinueRegisterRouteBuilder<TData>(saga));

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
