﻿using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel : AbstractAzureServiceBusRequestReply, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
    {
        public AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel(IComponentFactoryFacade factory, ILogger logger)
            : base(factory, logger)
        {

        }

        public async Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            var entity = EntityNameHelper.FormatSubscriptionPath(sendercontext.Channel.ReplyPath, sendercontext.Channel.ReplySubscription);

            var client = new SessionClient(sendercontext.Channel.ReplyConnectionString, entity);

            var messagesession = await client.AcceptMessageSessionAsync(context.TracingContext.ReplyToRequestId).ConfigureAwait(false);

            var message = sendercontext.Channel.ReplyTimeOut != 0 ? 
                await messagesession.ReceiveAsync(TimeSpan.FromSeconds(sendercontext.Channel.ReplyTimeOut)).ConfigureAwait(false) : 
                await messagesession.ReceiveAsync().ConfigureAwait(false);

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = await adapter.ReadFromPhysicalMessage(message, sendercontext).ConfigureAwait(false);

                await messagesession.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
            }

            await messagesession.CloseAsync().ConfigureAwait(false);

            await client.CloseAsync().ConfigureAwait(false);

            return outputcontext;
        }
    }
}