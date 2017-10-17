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

        public EndPoint[] Provide<TContent>(string name = "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return _endpoints.Where(x => x.MessageType == typeof(TContent)).ToArray();
            }

            return _endpoints.Where(x => x.Name==name && x.MessageType == typeof(TContent)).ToArray();
        }

        public EndPointSetting Provide<TContent>(EndPoint endpoint, TContent content)
        {

            if (endpoint.ExtractorType == null)
            {
                if (endpoint.ConnectionStringExtractorType!=null && (endpoint.PathExtractorType!=null || !string.IsNullOrWhiteSpace(endpoint.ToPath)))
                {
                    return ValueFinder(endpoint);
                }

                return null;
            }
            else
            {
                return Finder(endpoint, content);
            }
        }

        private EndPointSetting ValueFinder(EndPoint endpoint)
        {
            var output = new EndPointSetting();

            if (endpoint.PathExtractorType != null)
            {
                var extractorpath = _factory.Create<IValueSettingFinder>(endpoint.PathExtractorType);

                var topathextractor = endpoint.ToPathExtractor as Func<IValueSettingFinder, string>;

                if (topathextractor != null)
                {
                    output.ToPath = topathextractor(extractorpath);
                }
            }
            else
            {
                output.ToPath = endpoint.ToPath;
            }

            var extractorconnectionstring = _factory.Create<IValueSettingFinder>(endpoint.ConnectionStringExtractorType);

            var toconnectionextractor = endpoint.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;


            if (toconnectionextractor != null)
            {
                output.ToConnectionString = toconnectionextractor(extractorconnectionstring);
            }

            return output;
        }
        private EndPointSetting Finder<T>(EndPoint endpoint, T record)
        {
            var extractor = _factory.Create<IEndPointSettingFinder<T>>(endpoint.ExtractorType);

            var output = extractor.Find(record);

            return output;
        }
    }
}