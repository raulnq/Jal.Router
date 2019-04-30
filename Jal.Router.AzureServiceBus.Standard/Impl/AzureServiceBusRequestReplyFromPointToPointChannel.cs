using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusRequestReplyFromPointToPointChannel : AbstractAzureServiceBusRequestReply, IRequestReplyChannelFromPointToPointChannel
    {
        public AzureServiceBusRequestReplyFromPointToPointChannel(IComponentFactoryGateway factory, ILogger logger)
            : base(factory, logger)
        {

        }

        public async Task<MessageContext> Read(MessageContext context, IMessageAdapter adapter)
        {
            var client = new SessionClient(_metadata.Channel.ToReplyConnectionString, _metadata.Channel.ToReplyPath);

            var messagesession = await client.AcceptMessageSessionAsync(context.IdentityContext.ReplyToRequestId).ConfigureAwait(false);

            var message = _metadata.Channel.ToReplyTimeOut != 0 ? 
                await messagesession.ReceiveAsync(TimeSpan.FromSeconds(_metadata.Channel.ToReplyTimeOut)).ConfigureAwait(false) : 
                await messagesession.ReceiveAsync().ConfigureAwait(false);

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = await adapter.ReadMetadataAndContentFromEndpoint(message, context.EndPoint).ConfigureAwait(false);

                await messagesession.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
            }

            await messagesession.CloseAsync().ConfigureAwait(false);

            await client.CloseAsync().ConfigureAwait(false);

            return outputcontext;
        }
    }
}