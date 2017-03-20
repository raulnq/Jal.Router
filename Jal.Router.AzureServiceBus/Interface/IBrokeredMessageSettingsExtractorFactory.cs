using System;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageSettingsExtractorFactory
    {
        IBrokeredMessageSettingsExtractor Create(Type extractortype);
    }
}