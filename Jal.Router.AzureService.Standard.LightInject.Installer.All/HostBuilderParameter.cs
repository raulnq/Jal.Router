using System;
using Common.Logging;
using Jal.Router.Interface;
using LightInject;

namespace Jal.Router.AzureServiceBus.Standard.LightInject.Installer.All
{
    public class HostBuilderParameter
    {
        public Type RouterInterceptorType { get; set; }
        public ServiceContainer Container { get; set; }
        public IRouterConfigurationSource[] Sources { get; set; }

        public ILog Log { get; set; }

        public int MaxConcurrentCalls { get; set; }

        public double AutoRenewTimeout { get; set; }

        public string ShutdownFile { get; set; }

        public string StorageConnectionString { get; set; }

        public string SagaStorageName { get; set; }

        public string MessageStorageName { get; set; }

        public string TableNameSufix { get; set; }

        public string ApplicationName { get; set; }

        public int HeartBeatFrequency { get; set; }

        public bool UseApplicationInsights { get; set; }

        public string ApplicationInsightsKey { get; set; }

        public HostBuilderParameter()
        {
            Sources = new IRouterConfigurationSource[]{};
            MaxConcurrentCalls = 4;
            AutoRenewTimeout = 60;
            SagaStorageName = "sagas";
            MessageStorageName = "messages";
            TableNameSufix = DateTime.UtcNow.ToString("yyyyMM");
            ApplicationName = "App";
            HeartBeatFrequency = 600000;
        }
    }
}