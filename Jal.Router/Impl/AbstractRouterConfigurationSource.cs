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

        public INameRouteBuilder<THandler> RegisterRoute<THandler>(string name="")
        {
            var builder = new NameRouteBuilder<THandler>(name, _routes);

            return builder;
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
