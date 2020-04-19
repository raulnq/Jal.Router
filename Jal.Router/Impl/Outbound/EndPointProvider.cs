using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class EndPointProvider : IEndPointProvider
    {
        private readonly EndPoint[] _endpoints;
        public EndPointProvider(IRouterConfigurationSource[] sources)
        {
            var routes = new List<EndPoint>();

            foreach (var source in sources)
            {
                routes.AddRange(source.GetEndPoints());
            }

            _endpoints = routes.ToArray();
        }

        public EndPoint Provide(Options options, Type contenttype)
        {
            try
            {
                return _endpoints.Single(x => x.Condition(x, options, contenttype));
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Missing or duplicate endpoint {options.EndPointName} for type {contenttype.FullName}, {ex.Message}");
            }
            
        }
    }
}