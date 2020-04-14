using System;
using Jal.Router.ApplicationInsights.Extensions;
using Jal.Router.ApplicationInsights.LightInject.Installer;
using Jal.Router.AzureServiceBus.Standard.Extensions;
using Jal.Router.AzureStorage.LightInject.Installer;
using Jal.Router.LightInject.Installer;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.Interface;
using LightInject;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Jal.Router.AzureServiceBus.Standard.LightInject.Installer;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage.Model;
using Jal.Router.Newtonsoft.LightInject.Installer;
using Jal.Router.Newtonsoft.Extensions;
using Jal.Router.Impl;
using Jal.Router.Serilog.LightInject.Installer;
using Jal.Router.Serilog.Extensions;

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

        public IHostBuilder UseAzureServiceBus(IRouterConfigurationSource[] sources, Action<AzureServiceBusParameter> action=null)
        {
            _parameter.Sources = sources;
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

        public IHostBuilder UseRouterInterceptor<TRouterInterceptor>() where TRouterInterceptor : IRouterInterceptor
        {
            _parameter.RouterInterceptorType = typeof(TRouterInterceptor);
            return this;
        }

        public IHostBuilder UseBusInterceptor<TBusInterceptor>() where TBusInterceptor : IBusInterceptor
        {
            _parameter.BusInterceptorType = typeof(IBusInterceptor);
            return this;
        }

        public IHostBuilder Use(Action<IConfiguration> setup)
        {
            _parameter.Setup = setup;
            return this;
        }

        public IHost Build()
        {
            _parameter.Container.AddRouter(_parameter.Sources);

            _parameter.Container.AddAzureServiceBusForRouter();

            _parameter.Container.AddNewtonsoftForRouter();

            if (_parameter.UseSerilog)
            {
                _parameter.Container.AddSerilogForRouter();
            }

            if (_parameter.UseApplicationInsights)
            {
                _parameter.Container.Register<TelemetryClient>(x=> {

                    var conf = TelemetryConfiguration.CreateDefault();

                    if (!string.IsNullOrWhiteSpace(_parameter.ApplicationInsightsKey))
                    {
                        conf.InstrumentationKey = _parameter.ApplicationInsightsKey;
                    }

                    var client = new TelemetryClient(conf);

                    client.Context.Cloud.RoleName = _parameter.ApplicationName;

                    return client;
                } ,new PerContainerLifetime());

                _parameter.Container.AddApplicationInsightsForRouter();
            }

            if (_parameter.UseAzureStorage)
            {
                _parameter.Container.AddAzureStorageForRouter();
            }

            if (_parameter.RouterInterceptorType != null)
            {
                _parameter.Container.Register(typeof(IRouterInterceptor), _parameter.RouterInterceptorType, _parameter.RouterInterceptorType.FullName, new PerContainerLifetime());
            }

            if (_parameter.BusInterceptorType != null)
            {
                _parameter.Container.Register(typeof(IBusInterceptor), _parameter.BusInterceptorType, _parameter.BusInterceptorType.FullName, new PerContainerLifetime());
            }

            var host = _parameter.Container.GetInstance<IHost>();

            host.Configuration.UseAzureServiceBusAsTransport(_parameter.AzureServiceBusParameter);

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

            if (_parameter.RouterInterceptorType != null)
            {
                host.Configuration.RouterInterceptorType = _parameter.RouterInterceptorType;
            }


            if (_parameter.BusInterceptorType != null)
            {
                host.Configuration.BusInterceptorType = _parameter.BusInterceptorType;
            }

            host.Configuration.AddMonitoringTask<HeartBeatLogger>(_parameter.HeartBeatFrequency);

            _parameter.Setup?.Invoke(host.Configuration);

            return host;
        }
    }
}
