using System;
using Jal.Locator.Interface;
using Jal.Router.AzureServiceBus.Interface;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageSettingsFactory : IBrokeredMessageSettingsFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public BrokeredMessageSettingsFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IBrokeredMessageEndPointSettingValueFinder Create(Type consumertype)
        {
            return _serviceLocator.Resolve<IBrokeredMessageEndPointSettingValueFinder>(consumertype.FullName);
        }

        public IBrokeredMessageEndPointSettingFinder<T> Create<T>(Type consumertype)
        {
            return _serviceLocator.Resolve<IBrokeredMessageEndPointSettingFinder<T>>(consumertype.FullName);
        }
    }
}