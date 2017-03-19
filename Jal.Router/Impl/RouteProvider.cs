using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RouteProvider : IRouteProvider
    {
        private readonly Route[] _routes;
        public RouteProvider(IRouterConfigurationSource[] sources)
        {
            var routes = new List<Route>(); 

            foreach (var source in sources)
            {
                routes.AddRange(source.Source());
            }

            _routes = routes.ToArray();
        }


        public Route<TBody, TConsumer>[] Provide<TBody, TConsumer>(string name)
        {
            return Provide(typeof (TBody), name).Where(x=>x.ConsumerInterfaceType==typeof(TConsumer)).OfType<Route<TBody, TConsumer>>().ToArray();
        }

        public Route[] Provide(Type bodytype, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return _routes.Where(x => x.BodyType == bodytype).ToArray();
            }
            else
            {
                return _routes.Where(x => x.Name == name && x.BodyType == bodytype).ToArray();
            }
            
        }
    }
}