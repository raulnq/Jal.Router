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

        private readonly List<EndPoint> _endpoints = new List<EndPoint>();

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
            foreach (var endPoint in _endpoints)
            {
                endPoint.SetOrigin(_origin);

                foreach (var channel in endPoint.Channels)
                {
                    if(channel.Rules.Count==0)
                    {
                        channel.Rules.Add(new Rule($"origin='{_origin.Key}'", "$Default", true));
                    }
                }
            }
            return _endpoints.ToArray();
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

        public void RegisterOrigin(string name, string key="")
        {
            _origin.From = name;

            _origin.Key = key;
        }

        public IEndPointBuilder RegisterEndPoint(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var endpoint = new EndPoint(name);

            _endpoints.Add(endpoint);

            var builder = new EndPointBuilder(endpoint);

            return builder;
        }
    }
}
