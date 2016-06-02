using System;
using System.Collections.Specialized;
using Jal.Factory.Interface;
using Jal.Factory.Model;
using Jal.Router.Interface;
using Jal.Router.Model;
using Jal.Settings.Interface;

namespace Jal.Router.Impl
{


    public class EndPointSource : IEndPointSource
    {
        private readonly IObjectFactory _objectFactory;

        private readonly ISettingsExtractor _settingsExtractor;

        public EndPointSource(IObjectFactory objectFactory, ISettingsExtractor settingsExtractor)
        {
            _objectFactory = objectFactory;
            _settingsExtractor = settingsExtractor;
        }

        public EndPoint[] Get<TRequest, TResponse>(TRequest request, IRequestSender<TRequest, TResponse> sender, string route)
        {
            ObjectFactoryConfigurationItem[] objectFactoryConfigurationItems = null;

            var appSettings = _settingsExtractor.All();

            if (string.IsNullOrWhiteSpace(route))
            {
                objectFactoryConfigurationItems =  _objectFactory.ConfigurationFor<TRequest, IRequestSender<TRequest, TResponse>>(request);
            }
            else
            {
                objectFactoryConfigurationItems = _objectFactory.ConfigurationFor<TRequest, IRequestSender<TRequest, TResponse>>(request, route);
            }
            foreach (var configurationItem in objectFactoryConfigurationItems)
            {
                if (configurationItem.Bag != null && configurationItem.Bag.EndPointProvider != null && configurationItem.ResultType == sender.GetType())
                {

                    var provider =  configurationItem.Bag.EndPointProvider as Func<NameValueCollection,EndPoint[]>;
                    if (provider != null)
                    {
                        return provider(appSettings);
                    }
                }
            }
            return new EndPoint[]{};
        }
    }
}