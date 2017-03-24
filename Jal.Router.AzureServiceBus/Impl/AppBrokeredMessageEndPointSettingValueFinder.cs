using System.Configuration;
using Jal.Router.AzureServiceBus.Interface;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AppBrokeredMessageEndPointSettingValueFinder : IBrokeredMessageEndPointSettingValueFinder
    {
        public string Find(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}