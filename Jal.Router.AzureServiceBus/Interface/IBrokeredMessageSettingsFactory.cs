using System;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageSettingsFactory
    {
        IBrokeredMessageEndPointSettingValueFinder Create(Type extractortype);

        IBrokeredMessageEndPointSettingFinder<T> Create<T>(Type consumertype);
    }
}