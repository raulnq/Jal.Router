﻿using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel : AbstractAzureServiceBusRequestReply, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
    {
        public AzureServiceBusRequestReplyFromSubscriptionToPublishSubscribeChannel(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            : base(factory, configuration, logger)
        {

        }

        public MessageContext Read(MessageContext context, IMessageAdapter adapter)
        {
            var entity = EntityNameHelper.FormatSubscriptionPath(_metadata.Channel.ToReplyPath, _metadata.Channel.ToReplySubscription);

            var client = new SessionClient(_metadata.Channel.ToReplyConnectionString, entity);

            var messagesession = client.AcceptMessageSessionAsync(context.IdentityContext.ReplyToRequestId).GetAwaiter().GetResult();

            var message = _metadata.Channel.ToReplyTimeOut != 0 ? messagesession.ReceiveAsync(TimeSpan.FromSeconds(_metadata.Channel.ToReplyTimeOut)).GetAwaiter().GetResult() : messagesession.ReceiveAsync().GetAwaiter().GetResult();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.ReadMetadataAndContentFromEndpoint(message, context.EndPoint).GetAwaiter().GetResult();

                messagesession.CompleteAsync(message.SystemProperties.LockToken).GetAwaiter().GetResult();
            }

            messagesession.CloseAsync().GetAwaiter().GetResult();

            client.CloseAsync().GetAwaiter().GetResult();

            return outputcontext;
        }
    }
}