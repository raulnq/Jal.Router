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
        public AzureServiceBusSession(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder)
            : base("request replay",factory, configuration, builder)
        {

        }

        public override MessageContext ReceiveOnQueue(MessageContext inputcontext, IMessageAdapter adapter)
        {
            var client = QueueClient.CreateFromConnectionString(inputcontext.ToReplyConnectionString, inputcontext.ToReplyPath);

            var messagesession = client.AcceptMessageSession(inputcontext.ReplyToRequestId);

            var message = inputcontext.ToReplyTimeOut != 0 ? messagesession.Receive(TimeSpan.FromSeconds(inputcontext.ToReplyTimeOut)) : messagesession.Receive();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.Read(message, inputcontext.ResultType);

                message.Complete();
            }

            messagesession.Close();

            client.Close();

            return outputcontext;
        }

        public override MessageContext ReceiveOnTopic(MessageContext inputcontext, IMessageAdapter adapter)
        {
            var client = SubscriptionClient.CreateFromConnectionString(inputcontext.ToReplyConnectionString, inputcontext.ToReplyPath, inputcontext.ToReplySubscription);

            var messagesession = client.AcceptMessageSession(inputcontext.ReplyToRequestId);

            var message = inputcontext.ToReplyTimeOut != 0 ? messagesession.Receive(TimeSpan.FromSeconds(inputcontext.ToReplyTimeOut)) : messagesession.Receive();

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = adapter.Read(message, inputcontext.ResultType);

                message.Complete();
            }

            messagesession.Close();

            client.Close();

            return outputcontext;
        }

        public override string Send(MessageContext context, object message)
        {
            var queueclient = QueueClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

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