using System;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusSession : AbstractChannel, IRequestReplyChannel
    {
        public AzureServiceBusSession(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            : base(factory, configuration, logger)
        {

        }

        public Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnPointToPointChannelMethodFactory(SenderMetadata metadata)
        {
            return (context, adapter) =>
            {
                var client = QueueClient.CreateFromConnectionString(metadata.ToReplyConnectionString, metadata.ToReplyPath);

                var messagesession = client.AcceptMessageSession(context.Identity.ReplyToRequestId);

                var message = metadata.ToReplyTimeOut != 0 ? messagesession.Receive(TimeSpan.FromSeconds(metadata.ToReplyTimeOut)) : messagesession.Receive();

                MessageContext outputcontext = null;

                if (message != null)
                {
                    outputcontext = adapter.Read(message, context.ResultType, context.EndPoint.UseClaimCheck);

                    message.Complete();
                }

                messagesession.Close();

                client.Close();

                return outputcontext;
            };
        }

        public Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnPublishSubscriberChannelMethodFactory(SenderMetadata metadata)
        {
            return (context, adapter) =>
            {
                var client = SubscriptionClient.CreateFromConnectionString(metadata.ToReplyConnectionString, metadata.ToReplyPath, metadata.ToReplySubscription);

                var messagesession = client.AcceptMessageSession(context.Identity.ReplyToRequestId);

                var message = metadata.ToReplyTimeOut != 0 ? messagesession.Receive(TimeSpan.FromSeconds(metadata.ToReplyTimeOut)) : messagesession.Receive();

                MessageContext outputcontext = null;

                if (message != null)
                {
                    outputcontext = adapter.Read(message, context.ResultType, context.EndPoint.UseClaimCheck);

                    message.Complete();
                }

                messagesession.Close();

                client.Close();

                return outputcontext;
            };
        }

        public Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata)
        {
            return () => new object[] { QueueClient.CreateFromConnectionString(metadata.ToConnectionString, metadata.ToPath) };
        }

        public Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata)
        {
            return sender =>
            {
                var client = sender[0] as QueueClient;

                client.Close();
            };
        }

        public Func<object[], object, string> SendMethodFactory(SenderMetadata metadata)
        {
            return (sender, message) =>
            {
                var client = sender[0] as QueueClient;

                var bm = message as BrokeredMessage;

                if (bm != null)
                {
                    client.Send(bm);

                    return bm.MessageId;
                }

                return string.Empty;
            };
        }
    }
}