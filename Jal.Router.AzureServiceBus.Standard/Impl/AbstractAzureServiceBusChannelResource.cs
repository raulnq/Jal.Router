using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public abstract class AbstractAzureServiceBusChannelResource<T, S> : AbstractChannelResource<T, S>
    {
        public const string DefaultMessageTtlInDays = "defaultmessagettlindays";

        public const string MessageLockDurationInSeconds = "messagelockdurationinseconds";

        public const string DuplicateMessageDetectionInMinutes = "duplicatemessagedetectioninminutes";

        public const string SessionEnabled = "sessionenabled";

        public const string PartitioningEnabled = "partitioningenabled";

        public const string ExpressMessageEnabled = "expressmessageenabled";

        protected readonly IComponentFactoryGateway _factory;

        public AbstractAzureServiceBusChannelResource(IComponentFactoryGateway factory)
        {
            _factory = factory;
        }
    }
}