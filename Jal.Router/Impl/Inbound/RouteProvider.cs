using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class RouteProvider : IRouteProvider
    {
        private readonly Route[] _routes;

        private readonly Saga[] _sagas;

        public RouteProvider(IRouterConfigurationSource[] sources)
        {
            var routes = new List<Route>(); 

            foreach (var source in sources)
            {
                routes.AddRange(source.GetRoutes());
            }

            _routes = routes.ToArray();

            var sagas = new List<Saga>();

            foreach (var source in sources)
            {
                sagas.AddRange(source.GetSagas());
            }

            _sagas = sagas.ToArray();
        }

        public Route[] Provide(Type type, string routename)
        {
            if (string.IsNullOrEmpty(routename))
            {
                return _routes.Where(x => x.BodyType == type).ToArray();
            }
            else
            {
                return _routes.Where(x => x.Name == routename && x.BodyType == type).ToArray();
            }
        }

        public Route[] Provide(Route[] routes, Type type, string routename)
        {
            if (string.IsNullOrEmpty(routename))
            {
                return routes.Where(x => x.BodyType == type).ToArray();
            }
            else
            {
                return routes.Where(x => x.Name == routename && x.BodyType == type).ToArray();
            }
        }

        public Saga Provide(string saganame)
        {
            return _sagas.FirstOrDefault(x => x.Name == saganame);
        }
    }
}