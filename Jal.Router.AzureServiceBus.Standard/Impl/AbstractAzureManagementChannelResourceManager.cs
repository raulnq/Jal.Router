using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public abstract class AbstractAzureManagementChannelResourceManager<T,S> : AbstractChannelResourceManager<T, S>
    {
        public const string DefaultMessageTtlInDays = "defaultmessagettlindays";

        public const string MessageLockDurationInSeconds = "messagelockdurationinseconds";

        public const string DuplicateMessageDetectionInMinutes = "duplicatemessagedetectioninminutes";

        public const string SessionEnabled = "sessionenabled";

        public const string PartitioningEnabled = "partitioningenabled";

        public const string ExpressMessageEnabled = "expressmessageenabled";

        protected readonly IComponentFactoryGateway _factory;

        public AbstractAzureManagementChannelResourceManager(IComponentFactoryGateway factory)
        {
            _factory = factory;

            LoggerCallbackHandler.UseDefaultLogging = false;
        }

        protected async Task<IServiceBusNamespace> GetServiceBusNamespace(AzureServiceBusConfiguration configuration)
        {
            try
            {
                var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(configuration.ClientId, configuration.ClientSecret, configuration.TenantId, AzureEnvironment.AzureGlobalCloud);

                var serviceBusManager = ServiceBusManager.Authenticate(credentials, configuration.SubscriptionId);

                return await serviceBusManager.Namespaces.GetByResourceGroupAsync(configuration.ResourceGroupName,
                    configuration.ResourceName).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}