using System.Configuration;
using Jal.Router.AzureServiceBus.Interface;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AppBrokeredMessageSettingsExtractor : IBrokeredMessageSettingsExtractor
    {
        public string Get(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}