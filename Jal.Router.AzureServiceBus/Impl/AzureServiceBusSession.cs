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
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public AzureServiceBusSession(IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder)
            : base("request replay",factory, configuration, builder)
        {
            _factory = factory;

            _configuration = configuration;
        }

        public object Reply(MessageContext context, Type resulttype)
        {
            var adapter = _factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            Send(context);

            if (string.IsNullOrWhiteSpace(context.ToReplySubscription))
            {
                return ReceiveOnQueue(context, resulttype, adapter);
            }
            else
            {
                return ReceiveOnTopic(context, resulttype, adapter);
            }
        }

        private object ReceiveOnTopic(MessageContext context, Type resulttype, IMessageAdapter adapter)
        {
            var subscriptionclient = SubscriptionClient.CreateFromConnectionString(context.ToReplyConnectionString, context.ToReplyPath, context.ToReplySubscription);

            var messagesession = subscriptionclient.AcceptMessageSession(context.ReplyToRequestId);

            BrokeredMessage message = null;

            if (context.ToReplyTimeOut != 0)
            {
                message = messagesession.Receive(TimeSpan.FromSeconds(context.ToReplyTimeOut));
            }
            else
            {
                message = messagesession.Receive();
            }

            object response = null;

            if (message != null)
            {
                var body = adapter.GetBody(message);

                var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

                response = serializer.Deserialize(body, resulttype);

                message.Complete();
            }


            messagesession.Close();

            subscriptionclient.Close();

            return response;
        }

        private object ReceiveOnQueue(MessageContext context, Type resulttype, IMessageAdapter adapter)
        {
            var queuereceiver = QueueClient.CreateFromConnectionString(context.ToReplyConnectionString, context.ToReplyPath);

            var messagesession = queuereceiver.AcceptMessageSession(context.ReplyToRequestId);

            BrokeredMessage message = null;

            if (context.ToReplyTimeOut != 0)
            {
                message = messagesession.Receive(TimeSpan.FromSeconds(context.ToReplyTimeOut));
            }
            else
            {
                message = messagesession.Receive();
            }

            object response = null;

            if (message != null)
            {
                var body = adapter.GetBody(message);

                var serializer = _factory.Create<IMessageBodySerializer>(_configuration.MessageBodySerializerType);

                response = serializer.Deserialize(body, resulttype);

                message.Complete();
            }


            messagesession.Close();

            queuereceiver.Close();

            return response;
        }

        public override string Send(MessageContext context, object message)
        {
            var queueclient = QueueClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var bm = message as BrokeredMessage;

            queueclient.Send(bm);

            queueclient.Close();

            return bm.MessageId;
        }
    }
}