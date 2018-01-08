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

        public EndPointSetting Provide<TContent>(EndPoint endpoint, TContent content)
        {
            if (endpoint.ExtractorType == null)
            {
                if (endpoint.ConnectionStringExtractorType!=null && !string.IsNullOrWhiteSpace(endpoint.ToPath))
                {
                    return ValueFinder(endpoint);
                }

                return null;
            }
            else
            {
                return EndPointFinder(endpoint, content);
            }
        }

        private EndPointSetting ValueFinder(EndPoint endpoint)
        {
            var output = new EndPointSetting
            {
                EndPointName = endpoint.Name,

                ToPath = endpoint.ToPath,

                ToReplyPath = endpoint.ToReplyPath,

                ToReplySubscription = endpoint.ToReplySubscription,

                ToReplyTimeOut = endpoint.ToReplyTimeOut
            };

            var extractorconnectionstring = _factory.Create<IValueSettingFinder>(endpoint.ConnectionStringExtractorType);

            var toconnectionextractor = endpoint.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

            if (toconnectionextractor != null)
            {
                output.ToConnectionString = toconnectionextractor(extractorconnectionstring);
            }

            if (endpoint.ToReplyConnectionStringExtractor != null)
            {
                var finder = _factory.Create<IValueSettingFinder>(endpoint.ReplyConnectionStringExtractorType);

                var extractor = endpoint.ToReplyConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (extractor != null)
                {
                    output.ToReplyConnectionString = extractor(finder);
                }
            }

            return output;
        }
        private EndPointSetting EndPointFinder<TContent>(EndPoint endpoint, TContent record)
        {
            var endpointfinder = _factory.Create<IEndPointSettingFinder<TContent>>(endpoint.ExtractorType);

            var output = endpointfinder.Find(record);

            return output;
        }
    }
}