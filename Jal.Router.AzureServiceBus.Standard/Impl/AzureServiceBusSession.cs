using System;
using System.Collections.Generic;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBusSession : AbstractChannel, IRequestReplyChannel
    {
        public AzureServiceBusSession(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            : base("request replay",factory, configuration, logger)
        {

        }

        public override MessageContext ReceiveOnQueue(Channel channel, MessageContext context, IMessageAdapter adapter)
        {
            var client = new SessionClient(channel.ToReplyConnectionString, channel.ToReplyPath);

            var messagesession = client.AcceptMessageSessionAsync(context.Identity.ReplyToRequestId).GetAwaiter().GetResult();

            var message = channel.ToReplyTimeOut != 0 ? messagesession.ReceiveAsync(TimeSpan.FromSeconds(channel.ToReplyTimeOut)).GetAwaiter().GetResult() : messagesession.ReceiveAsync().GetAwaiter().GetResult();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.Read(message, context.ResultType, context.EndPoint.UseClaimCheck);

                messagesession.CompleteAsync(message.SystemProperties.LockToken);
            }

            messagesession.CloseAsync().GetAwaiter().GetResult();
            
            client.CloseAsync().GetAwaiter().GetResult();

            return outputcontext;
        }

        public override MessageContext ReceiveOnTopic(Channel channel, MessageContext context, IMessageAdapter adapter)
        {
            var entity = EntityNameHelper.FormatSubscriptionPath(channel.ToReplyPath, channel.ToReplySubscription);

            var client = new SessionClient(channel.ToReplyConnectionString, entity);

            var messagesession = client.AcceptMessageSessionAsync(context.Identity.ReplyToRequestId).GetAwaiter().GetResult();

            var message = channel.ToReplyTimeOut != 0 ? messagesession.ReceiveAsync(TimeSpan.FromSeconds(channel.ToReplyTimeOut)).GetAwaiter().GetResult() : messagesession.ReceiveAsync().GetAwaiter().GetResult();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.Read(message, context.ResultType, context.EndPoint.UseClaimCheck);

                messagesession.CompleteAsync(message.SystemProperties.LockToken);
            }

            messagesession.CloseAsync().GetAwaiter().GetResult();

            client.CloseAsync().GetAwaiter().GetResult();

            return outputcontext;
        }

        public override string Send(Channel channel, object message)
        {
            var sender = new MessageSender(channel.ToConnectionString, channel.ToPath);

            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                sender.SendAsync(new List<Message>() {sbmessage}).GetAwaiter().GetResult();

                sender.CloseAsync().GetAwaiter().GetResult();

                return sbmessage.MessageId;
            }

            return string.Empty;
        }
    }
}