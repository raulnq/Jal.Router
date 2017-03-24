using System;
using System.Linq;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.AzureServiceBus.Model;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageEndPointSettingProvider : IBrokeredMessageEndPointSettingProvider
    {
        public IBrokeredMessageSettingsFactory Factory { get; set; }

        public BrokeredMessageEndPoint Provide<T>(EndPoint endpoint, T record)
        {
           
            var isvaluefinder = typeof(IBrokeredMessageEndPointSettingValueFinder).IsAssignableFrom(endpoint.ExtractorType);

            var isfinder = typeof(IBrokeredMessageEndPointSettingFinder<T>).IsAssignableFrom(endpoint.ExtractorType);

            if (isvaluefinder)
            {
                return ValueFinder(endpoint);
            }

            if (isfinder)
            {
                return Finder<T>(endpoint, record);
            }

            return null;

        }

        private BrokeredMessageEndPoint ValueFinder(EndPoint endpoint)
        {
            var output = new BrokeredMessageEndPoint();

            var extractor = Factory.Create(endpoint.ExtractorType);

            var toconnectionextractor =
                endpoint.ToConnectionStringExtractor as Func<IBrokeredMessageEndPointSettingValueFinder, string>;

            var topathextractor = endpoint.ToPathExtractor as Func<IBrokeredMessageEndPointSettingValueFinder, string>;

            if (toconnectionextractor != null && topathextractor != null)
            {
                output.ToConnectionString = toconnectionextractor(extractor);

                output.To = topathextractor(extractor);
            }

            var toreplyconnectionextractor =
                endpoint.ReplyToConnectionStringExtractor as Func<IBrokeredMessageEndPointSettingValueFinder, string>;

            var toreplypathextractor = endpoint.ReplyToPathExtractor as Func<IBrokeredMessageEndPointSettingValueFinder, string>;

            if (toreplyconnectionextractor != null && toreplypathextractor != null)
            {
                output.ReplyToConnectionString = toreplyconnectionextractor(extractor);

                output.ReplyTo = toreplypathextractor(extractor);
            }

            var fromextractor = endpoint.ReplyToPathExtractor as Func<IBrokeredMessageEndPointSettingValueFinder, string>;

            output.From = fromextractor?.Invoke(extractor);

            return output;
        }

        private BrokeredMessageEndPoint Finder<T>(EndPoint endpoint, T record)
        {
            var extractor = Factory.Create<T>(endpoint.ExtractorType);

            return extractor.Find(record);
        }
    }
}