using Jal.Router.Interface;

namespace Jal.Router.Impl
{

    public abstract class AbstractChannel
    {
        protected readonly IComponentFactoryFacade Factory;

        protected readonly ILogger Logger;

        public const string DefaultMessageTtlInDays = "defaultmessagettlindays";

        public const string MessageLockDurationInSeconds = "messagelockdurationinseconds";

        public const string DuplicateMessageDetectionInMinutes = "duplicatemessagedetectioninminutes";

        public const string SessionEnabled = "sessionenabled";

        public const string PartitioningEnabled = "partitioningenabled";

        public const string ExpressMessageEnabled = "expressmessageenabled";

        protected AbstractChannel(IComponentFactoryFacade factory, ILogger logger)
        {
            Factory = factory;
            Logger = logger;
        }
    }
}