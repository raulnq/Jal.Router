using System.Threading.Tasks;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Outbound;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public abstract class AbstractAzureServiceBusRequestReply : AbstractChannel
    {
        protected AbstractAzureServiceBusRequestReply(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            : base(factory, configuration, logger)
        {

        }

        protected SenderMetadata _metadata;

        private QueueClient _client;

        public void Open(SenderMetadata metadata)
        {
            _client = new QueueClient(metadata.Channel.ToConnectionString, metadata.Channel.ToPath);

            _metadata = metadata;
        }

        public string Send(object message)
        {
            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                _client.SendAsync(sbmessage).GetAwaiter().GetResult();

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public Task Close()
        {
            return _client.CloseAsync();
        }

    }
}