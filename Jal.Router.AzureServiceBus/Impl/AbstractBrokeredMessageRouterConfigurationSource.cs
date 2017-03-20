using System.Collections.Generic;
using Jal.Router.AzureServiceBus.Fluent.Impl;
using Jal.Router.AzureServiceBus.Fluent.Interface;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.AzureServiceBus.Model;

namespace Jal.Router.AzureServiceBus.Impl
{
    public abstract class AbstractBrokeredMessageRouterConfigurationSource : IBrokeredMessageRouterConfigurationSource
    {
        private readonly List<EndPoint> _enpoints = new List<EndPoint>();

        public EndPoint[] Source()
        {
            return _enpoints.ToArray();
        }

        public INameEndPointBuilder RegisterEndPoint<TExtractor>(string name = "") where TExtractor : IBrokeredMessageSettingsExtractor
        {
            var endpoint = new EndPoint(name);

            _enpoints.Add(endpoint);

            var builder = new EndPointBuilder<TExtractor>(endpoint);

            return builder;
        }
    }
}