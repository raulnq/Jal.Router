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

        public EndPointSetting Provide<TContent>(EndPoint endpoint, TContent content)
        {

            var isvaluefinder = typeof(IEndPointValueSettingFinder).IsAssignableFrom(endpoint.ExtractorType);

            var isfinder = typeof(IEndPointSettingFinder<TContent>).IsAssignableFrom(endpoint.ExtractorType);

            if (isvaluefinder)
            {
                return ValueFinder(endpoint);
            }

            if (isfinder)
            {
                return Finder(endpoint, content);
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
                endpoint.ReplyToConnectionStringExtractor as Func<IEndPointValueSettingFinder, string>;//TODO delete

            var toreplypathextractor = endpoint.ReplyToPathExtractor as Func<IEndPointValueSettingFinder, string>;//TODO delete

            if (toreplyconnectionextractor != null && toreplypathextractor != null)//TODO delete
            {
                output.ReplyToConnectionString = toreplyconnectionextractor(extractor);//TODO delete

                output.ReplyToPath = toreplypathextractor(extractor);//TODO delete
            }

            output.From = endpoint.OriginName;

            output.Origin = endpoint.OriginKey;

            return output;
        }
        private EndPointSetting Finder<T>(EndPoint endpoint, T record)
        {
            var extractor = Factory.Create<T>(endpoint.ExtractorType);

            var output = extractor.Find(record);

            if (string.IsNullOrWhiteSpace(output.From))
            {
                output.From = endpoint.OriginName;
            }

            if (string.IsNullOrWhiteSpace(output.Origin))
            {
                output.Origin = endpoint.OriginKey;
            }

            return output;
        }
    }
}