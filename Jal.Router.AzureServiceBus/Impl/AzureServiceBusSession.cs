using System;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusSession : AbstractChannel, IRequestReplyChannel
    {
        public AzureServiceBusSession(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            : base("request replay",factory, configuration, logger)
        {

        }

        public override MessageContext ReceiveOnQueue(Channel channel, MessageContext context, IMessageAdapter adapter)
        {
            var client = QueueClient.CreateFromConnectionString(channel.ToReplyConnectionString, channel.ToReplyPath);

            var messagesession = client.AcceptMessageSession(context.Identity.ReplyToRequestId);

            var message = channel.ToReplyTimeOut != 0 ? messagesession.Receive(TimeSpan.FromSeconds(channel.ToReplyTimeOut)) : messagesession.Receive();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.Read(message, context.ResultType, context.EndPoint.UseClaimCheck);

                message.Complete();
            }

            messagesession.Close();

            client.Close();

            return outputcontext;
        }

        public override MessageContext ReceiveOnTopic(Channel channel, MessageContext inputcontext, IMessageAdapter adapter)
        {
            var client = SubscriptionClient.CreateFromConnectionString(channel.ToReplyConnectionString, channel.ToReplyPath, channel.ToReplySubscription);

            var messagesession = client.AcceptMessageSession(inputcontext.Identity.ReplyToRequestId);

            var message = channel.ToReplyTimeOut != 0 ? messagesession.Receive(TimeSpan.FromSeconds(channel.ToReplyTimeOut)) : messagesession.Receive();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.Read(message, inputcontext.ResultType, inputcontext.EndPoint.UseClaimCheck);

                message.Complete();
            }

            messagesession.Close();

            client.Close();

            return outputcontext;
        }

        public override string Send(Channel channel, object message)
        {
            var queueclient = QueueClient.CreateFromConnectionString(channel.ToConnectionString, channel.ToPath);

            var bm = message as BrokeredMessage;

            if (bm != null)
            {
                queueclient.Send(bm);

                queueclient.Close();

                return bm.MessageId;
            }

            return string.Empty;
        }
    }
}