using System;
using Jal.Router.ApplicationInsights;
using Jal.Router.AzureServiceBus.Standard;
using Jal.Router.LightInject.Installer;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.Interface;
using LightInject;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage;
using Jal.Router.Newtonsoft.Extensions;
using Jal.Router.Impl;
using Jal.Router.Serilog;
using Jal.Router.Newtonsoft;

namespace Jal.Router.Azure.Standard.LightInject.Installer.All
{
    public class HostBuilder : IHostBuilder
    {
        private readonly HostBuilderParameter _parameter;

        public HostBuilder(ServiceContainer container, string applicationname)
        {
            _parameter = new HostBuilderParameter()
            {
                Container = container,
                ApplicationName = applicationname
            };
        }

        public static IHostBuilder Create(ServiceContainer container, string applicationname)
        {
            return new HostBuilder(container, applicationname);
        }

        public IHostBuilder UseAzureServiceBus(Action<IRouterBuilder> builderaction, Action<AzureServiceBusChannelConnection> action=null)
        {
            _parameter.Action = builderaction;
            action?.Invoke(_parameter.AzureServiceBusParameter);
            return this;
        }

        public IHostBuilder UseSerilog()
        {
            _parameter.UseSerilog = true;
            return this;
        }

        public IHostBuilder UseApplicationInsights(string applicationinsightskey="")
        {
            _parameter.ApplicationInsightsKey = applicationinsightskey;
            _parameter.UseApplicationInsights = true;
            return this;
        }

        public IHostBuilder UseAzureStorage(Action<AzureStorageParameter> action)
        {
            action?.Invoke(_parameter.AzureStorageParameter);
            _parameter.UseAzureStorage = true;
            return this;
        }

        public IHostBuilder UseHeartBeatMonitor(int frequency)
        {
            _parameter.HeartBeatFrequency = frequency;
            return this;
        }

        public IHostBuilder Use(Action<IConfiguration> setup)
        {
            _parameter.Setup = setup;
            return this;
        }

        public IHost Build()
        {
            _parameter.Container.AddRouter(c=>
            {
                if(_parameter.Action!=null)
                {
                    _parameter.Action(c);
                }

                c.AddAzureServiceBus();

                c.AddNewtonsoft();

                if (_parameter.UseSerilog)
                {
                    c.AddSerilog();
                }

                if (_parameter.UseApplicationInsights)
                {
                    _parameter.Container.Register<TelemetryClient>(x => {

                        var conf = TelemetryConfiguration.CreateDefault();

                        if (!string.IsNullOrWhiteSpace(_parameter.ApplicationInsightsKey))
                        {
                            conf.InstrumentationKey = _parameter.ApplicationInsightsKey;
                        }

                        var client = new TelemetryClient(conf);

                        client.Context.Cloud.RoleName = _parameter.ApplicationName;

                        return client;
                    }, new PerContainerLifetime());

                    c.AddApplicationInsights();
                }

                if (_parameter.UseAzureStorage)
                {
                    c.AddAzureStorage();
                }
            });

            var host = _parameter.Container.GetInstance<IHost>();

            host.Configuration.AddAzureServiceBusAsTransport(_parameter.AzureServiceBusParameter);

            if (_parameter.UseSerilog)
            {
                host.Configuration.UseSerilog();
            }

            host.Configuration.SetApplicationName(_parameter.ApplicationName);

            if (_parameter.UseAzureStorage)
            {
                host.Configuration.UseAzureStorageAsStorage(_parameter.AzureStorageParameter);
            }

            host.Configuration.Storage.IgnoreExceptions = true;

            if (!string.IsNullOrWhiteSpace(_parameter.ApplicationInsightsKey))
            {
                host.Configuration.UseApplicationInsights();
            }

            host.Configuration.UseNewtonsoftAsSerializer();

            host.Configuration.AddMonitoringTask<MonitoringTask>(_parameter.HeartBeatFrequency);

            _parameter.Setup?.Invoke(host.Configuration);

            return host;
        }
    }
}
