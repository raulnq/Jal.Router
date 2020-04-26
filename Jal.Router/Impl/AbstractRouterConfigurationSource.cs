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

        private readonly List<Resource> _resources = new List<Resource>();

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
                endPoint.SetOrigin(_origin);
            }
            return _enpoints.ToArray();
        }

        public Resource[] GetResources()
        {
            return _resources.ToArray();
        }

        public IListenerRouteBuilder RegisterHandler(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var route = new Route(name, typeof(ConsumerMiddleware));

            _routes.Add(route);

            var builder = new ListenerRouteBuilder(route);

            return builder;
        }

        public void RegisterSubscriptionToPublishSubscribeChannel(string subscription, string path, string connectionstring, Dictionary<string, string> properties, Rule rule = null)
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

            var resource = new Resource(ChannelType.SubscriptionToPublishSubscribe, path, connectionstring, properties, subscription);

            if(rule!=null)
            {
                resource.Rules.Add(rule);
            }
            else
            {
                resource.Rules.Add(new Rule($"origin='{_origin.Key}'", "$Default", true));
            }

            _resources.Add(resource);
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

            var resource = new Resource(ChannelType.PointToPoint, path, connectionstring, properties);

            _resources.Add(resource);
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

            var resource = new Resource(ChannelType.PublishSubscribe, path, connectionstring, properties);

            _resources.Add(resource);
        }

        public ITimeoutBuilder RegisterSaga<TData>(string name, Action<IRouteBuilder<TData>> start, Action<IRouteBuilder<TData>> @continue=null, Action<IRouteBuilder<TData>> end = null) where TData : class, new()
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

            start(new RouteBuilder<TData>(saga, saga.InitialRoutes, typeof(InitialConsumerMiddleware)));

            @continue?.Invoke(new RouteBuilder<TData>(saga, saga.Routes, typeof(MiddleConsumerMiddleware)));

            end?.Invoke(new RouteBuilder<TData>(saga, saga.FinalRoutes, typeof(FinalConsumerMiddleware)));

            _sagas.Add(saga);

            var timeoutbuilder = new TimeoutBuilder(saga);

            return timeoutbuilder;
        }

        public ITimeoutBuilder RegisterSaga<TData>(Action<IRouteBuilder<TData>> start, Action<IRouteBuilder<TData>> @continue = null, Action<IRouteBuilder<TData>> end = null) where TData : class, new()
        {
            return RegisterSaga<TData>(typeof(TData).Name.ToLower(), start, @continue, end);
        }

        public void RegisterOrigin(string name, string key="")
        {
            _origin.From = name;

            _origin.Key = key;
        }

        public IForMessageEndPointBuilder RegisterEndPoint(string name)
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
