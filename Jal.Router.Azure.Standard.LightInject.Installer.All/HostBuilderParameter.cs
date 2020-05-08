using System;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Interface;
using LightInject;
using Jal.Router.AzureStorage;

namespace Jal.Router.Azure.Standard.LightInject.Installer.All
{
    public class HostBuilderParameter
    {
        public Action<IConfiguration> Setup { get; set; }
        public ServiceContainer Container { get; set; }
        public Action<IRouterBuilder> Action { get; set; }
        public bool UseSerilog { get; set; }
        public string ApplicationName { get; set; }
        public int HeartBeatFrequency { get; set; }
        public bool UseApplicationInsights { get; set; }
        public string ApplicationInsightsKey { get; set; }
        public AzureServiceBusChannelConnection AzureServiceBusParameter { get; set; }
        public AzureStorageParameter AzureStorageParameter { get; set; }
        public bool UseAzureStorage { get; set; }
        public HostBuilderParameter()
        {
            ApplicationName = "App";
            HeartBeatFrequency = 600;
            AzureServiceBusParameter = new AzureServiceBusChannelConnection();
            AzureStorageParameter = new AzureStorageParameter();
        }
    }
}