using System;
using Jal.Locator.Interface;
using Jal.Router.AzureServiceBus.Interface;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageSettingsExtractorFactory : IBrokeredMessageSettingsExtractorFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public BrokeredMessageSettingsExtractorFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IBrokeredMessageSettingsExtractor Create(Type consumertype)
        {
            return _serviceLocator.Resolve<IBrokeredMessageSettingsExtractor>(consumertype.FullName);
        }
    }
}