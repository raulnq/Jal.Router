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

        public EndPoint Provide(string name, Type contenttype)
        {
            try
            {
                return _endpoints.Single(x => x.Name == name && x.ContentType == contenttype);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Missing or duplicate endpoint {name} for type {contenttype.FullName}, {ex.Message}");
            }
            
        }
    }
}