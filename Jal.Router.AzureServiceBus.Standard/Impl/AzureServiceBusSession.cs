using System;
using System.Collections.Generic;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Jal.Router.AzureServiceBus.Standard.Impl
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
                var client = new SessionClient(metadata.ToReplyConnectionString, metadata.ToReplyPath);

                var messagesession = client.AcceptMessageSessionAsync(context.Identity.ReplyToRequestId).GetAwaiter().GetResult();

                var message = metadata.ToReplyTimeOut != 0 ? messagesession.ReceiveAsync(TimeSpan.FromSeconds(metadata.ToReplyTimeOut)).GetAwaiter().GetResult() : messagesession.ReceiveAsync().GetAwaiter().GetResult();

                MessageContext outputcontext = null;

                if (message != null)
                {
                    outputcontext = adapter.Read(message, context.ResultType, context.EndPoint.UseClaimCheck);

                    messagesession.CompleteAsync(message.SystemProperties.LockToken);
                }

                messagesession.CloseAsync().GetAwaiter().GetResult();

                client.CloseAsync().GetAwaiter().GetResult();

                return outputcontext;
            };
        }

        public Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnPublishSubscriberChannelMethodFactory(SenderMetadata metadata)
        {
            return (context, adapter) =>
            {
                var entity = EntityNameHelper.FormatSubscriptionPath(metadata.ToReplyPath, metadata.ToReplySubscription);

                var client = new SessionClient(metadata.ToReplyConnectionString, entity);

                var messagesession = client.AcceptMessageSessionAsync(context.Identity.ReplyToRequestId).GetAwaiter().GetResult();

                var message = metadata.ToReplyTimeOut != 0 ? messagesession.ReceiveAsync(TimeSpan.FromSeconds(metadata.ToReplyTimeOut)).GetAwaiter().GetResult() : messagesession.ReceiveAsync().GetAwaiter().GetResult();

                MessageContext outputcontext = null;

                if (message != null)
                {
                    outputcontext = adapter.Read(message, context.ResultType, context.EndPoint.UseClaimCheck);

                    messagesession.CompleteAsync(message.SystemProperties.LockToken);
                }

                messagesession.CloseAsync().GetAwaiter().GetResult();

                client.CloseAsync().GetAwaiter().GetResult();

                return outputcontext;
            };
        }

        public Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata)
        {
            return () => new object[] { new QueueClient(metadata.ToConnectionString, metadata.ToPath) };
        }

        public Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata)
        {
            return sender =>
            {
                var client = sender[0] as QueueClient;

                client.CloseAsync().GetAwaiter().GetResult();
            };
        }

        public Func<object[], object, string> SendMethodFactory(SenderMetadata metadata)
        {
            return (sender, message) =>
            {
                var client = sender[0] as QueueClient;

                var sbmessage = message as Message;

                if (sbmessage != null)
                {
                    client.SendAsync(sbmessage).GetAwaiter().GetResult();

                    return sbmessage.MessageId;
                }

                return string.Empty;
            };
        }
    }
}