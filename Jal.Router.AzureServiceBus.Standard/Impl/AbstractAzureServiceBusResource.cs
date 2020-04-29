using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public abstract class AbstractAzureServiceBusResource : AbstractResource
    {
        public const string DefaultMessageTtlInDays = "defaultmessagettlindays";

        public const string MessageLockDurationInSeconds = "messagelockdurationinseconds";

        public const string DuplicateMessageDetectionInMinutes = "duplicatemessagedetectioninminutes";

        public const string SessionEnabled = "sessionenabled";

        public const string PartitioningEnabled = "partitioningenabled";

        public const string ExpressMessageEnabled = "expressmessageenabled";

        protected readonly IComponentFactoryFacade _factory;

        public AbstractAzureServiceBusResource(IComponentFactoryFacade factory)
        {
            _factory = factory;
        }
    }
}