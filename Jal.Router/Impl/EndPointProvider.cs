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

        public EndPoint[] Provide<TContent>(string name = "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return _endpoints.Where(x => x.MessageType == typeof(TContent)).ToArray();
            }

            return _endpoints.Where(x => x.Name==name && x.MessageType == typeof(TContent)).ToArray();
        }

        public IEndPointSettingFinderFactory Factory { get; set; }

        public EndPointSetting Provide<T>(EndPoint endpoint, T content)
        {

            var isvaluefinder = typeof(IEndPointValueSettingFinder).IsAssignableFrom(endpoint.ExtractorType);

            var isfinder = typeof(IEndPointSettingFinder<T>).IsAssignableFrom(endpoint.ExtractorType);

            if (isvaluefinder)
            {
                return ValueFinder(endpoint);
            }

            if (isfinder)
            {
                return Finder<T>(endpoint, content);
            }

            return null;

        }

        private EndPointSetting ValueFinder(EndPoint endpoint)
        {
            var output = new EndPointSetting();

            var extractor = Factory.Create(endpoint.ExtractorType);

            var toconnectionextractor =
                endpoint.ToConnectionStringExtractor as Func<IEndPointValueSettingFinder, string>;

            var topathextractor = endpoint.ToPathExtractor as Func<IEndPointValueSettingFinder, string>;

            if (toconnectionextractor != null && topathextractor != null)
            {
                output.ToConnectionString = toconnectionextractor(extractor);

                output.ToPath = topathextractor(extractor);
            }

            var toreplyconnectionextractor =
                endpoint.ReplyToConnectionStringExtractor as Func<IEndPointValueSettingFinder, string>;

            var toreplypathextractor = endpoint.ReplyToPathExtractor as Func<IEndPointValueSettingFinder, string>;

            if (toreplyconnectionextractor != null && toreplypathextractor != null)
            {
                output.ReplyToConnectionString = toreplyconnectionextractor(extractor);

                output.ReplyToPath = toreplypathextractor(extractor);
            }

            var fromextractor = endpoint.ReplyToPathExtractor as Func<IEndPointValueSettingFinder, string>;

            output.From = fromextractor?.Invoke(extractor);

            return output;
        }
        private EndPointSetting Finder<T>(EndPoint endpoint, T record)
        {
            var extractor = Factory.Create<T>(endpoint.ExtractorType);

            return extractor.Find(record);
        }
    }
}