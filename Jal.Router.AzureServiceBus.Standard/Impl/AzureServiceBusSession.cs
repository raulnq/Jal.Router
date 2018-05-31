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
        public AzureServiceBusSession(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder)
            : base("request replay",factory, configuration, builder)
        {

        }

        public override MessageContext ReceiveOnQueue(MessageContext inputcontext, IMessageAdapter adapter)
        {
            var client = new SessionClient(inputcontext.ToReplyConnectionString, inputcontext.ToReplyPath);

            var messagesession = client.AcceptMessageSessionAsync(inputcontext.ReplyToRequestId).GetAwaiter().GetResult();

            var message = inputcontext.ToReplyTimeOut != 0 ? messagesession.ReceiveAsync(TimeSpan.FromSeconds(inputcontext.ToReplyTimeOut)).GetAwaiter().GetResult() : messagesession.ReceiveAsync().GetAwaiter().GetResult();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.Read(message, inputcontext.ResultType);

                messagesession.CompleteAsync(message.SystemProperties.LockToken);
            }

            messagesession.CloseAsync().GetAwaiter().GetResult();
            
            client.CloseAsync().GetAwaiter().GetResult();

            return outputcontext;
        }

        public override MessageContext ReceiveOnTopic(MessageContext inputcontext, IMessageAdapter adapter)
        {
            var entity = EntityNameHelper.FormatSubscriptionPath(inputcontext.ToReplyPath, inputcontext.ToReplySubscription);

            var client = new SessionClient(inputcontext.ToReplyConnectionString, entity);

            var messagesession = client.AcceptMessageSessionAsync(inputcontext.ReplyToRequestId).GetAwaiter().GetResult();

            var message = inputcontext.ToReplyTimeOut != 0 ? messagesession.ReceiveAsync(TimeSpan.FromSeconds(inputcontext.ToReplyTimeOut)).GetAwaiter().GetResult() : messagesession.ReceiveAsync().GetAwaiter().GetResult();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.Read(message, inputcontext.ResultType);

                messagesession.CompleteAsync(message.SystemProperties.LockToken);
            }

            messagesession.CloseAsync().GetAwaiter().GetResult();

            client.CloseAsync().GetAwaiter().GetResult();

            return outputcontext;
        }

        public override string Send(MessageContext context, object message)
        {
            var sender = new MessageSender(context.ToConnectionString, context.ToPath);

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