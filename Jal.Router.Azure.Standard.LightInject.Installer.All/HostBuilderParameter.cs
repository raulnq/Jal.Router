﻿using System;
using Common.Logging;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using LightInject;

namespace Jal.Router.Azure.Standard.LightInject.Installer.All
{
    public class HostBuilderParameter
    {
        public Type RouterInterceptorType { get; set; }
        public Type BusInterceptorType { get; set; }
        public Action<IConfiguration> Setup { get; set; }
        public ServiceContainer Container { get; set; }
        public IRouterConfigurationSource[] Sources { get; set; }
        public ILog Log { get; set; }
        public string ApplicationName { get; set; }
        public int HeartBeatFrequency { get; set; }
        public bool UseApplicationInsights { get; set; }
        public string ApplicationInsightsKey { get; set; }
        public AzureServiceBusParameter AzureServiceBusParameter { get; set; }
        public AzureStorageParameter AzureStorageParameter { get; set; }
        public bool UseAzureStorage { get; set; }
        public HostBuilderParameter()
        {
            Sources = new IRouterConfigurationSource[]{};
            ApplicationName = "App";
            HeartBeatFrequency = 600;
            AzureServiceBusParameter = new AzureServiceBusParameter();
            AzureStorageParameter = new AzureStorageParameter();
        }
    }
}