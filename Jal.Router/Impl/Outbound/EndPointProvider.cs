using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound
{
    public class EndPointProvider : IEndPointProvider
    {
        private readonly IComponentFactory _factory;

        private readonly EndPoint[] _endpoints;
        public EndPointProvider(IRouterConfigurationSource[] sources, IComponentFactory factory)
        {
            _factory = factory;

            var routes = new List<EndPoint>();

            foreach (var source in sources)
            {
                routes.AddRange(source.GetEndPoints());
            }

            _endpoints = routes.ToArray();
        }

        public EndPoint Provide(string name, Type contenttype)
        {
            return _endpoints.Single(x => x.Name==name && x.MessageType == contenttype);
        }
    }
}