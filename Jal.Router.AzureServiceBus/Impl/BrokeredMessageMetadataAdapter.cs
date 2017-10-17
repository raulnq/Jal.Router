using System;
using System.Linq;
using Jal.Router.Impl;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageMetadataAdapter : AbstractMessageMetadataAdapter
    {
        public override MessageContext Create<TMessage>(TMessage message)
        {
            var brokeredmessage = message as BrokeredMessage;

            if (brokeredmessage != null)
            {
                var context = new MessageContext
                {
                    Id = brokeredmessage.MessageId,
                    DateTimeUtc = DateTime.UtcNow
                };

                if (brokeredmessage.Properties.ContainsKey(From))
                {
                    context.Origin.Name = brokeredmessage.Properties[From].ToString();
                }

                if (brokeredmessage.Properties.ContainsKey(SagaId))
                {
                    context.Saga.Id = brokeredmessage.Properties[SagaId].ToString();
                }

                if (brokeredmessage.Properties.ContainsKey(Version))
                {
                    context.Version = brokeredmessage.Properties[Version].ToString();
                }

                if (brokeredmessage.Properties.ContainsKey(Origin))
                {
                    context.Origin.Key = brokeredmessage.Properties[Origin].ToString();
                }

                if (brokeredmessage.Properties.ContainsKey(RetryCount))
                {
                    context.RetryCount = Convert.ToInt32(brokeredmessage.Properties[RetryCount].ToString());
                }

                if (brokeredmessage.Properties != null)
                {
                    foreach (var property in brokeredmessage.Properties.Where(x => x.Key != From && x.Key != Origin && x.Key != Version && x.Key != RetryCount && x.Key != SagaId))
                    {
                        context.Headers.Add(property.Key, property.Value?.ToString());
                    }
                }

                return context;
            }
            throw new ApplicationException($"Invalid message type {typeof(TMessage).FullName}");
        }

        public override TMessage Create<TMessage>(MessageContext messagecontext, TMessage message)
        {
            var brokeredmessage = message as BrokeredMessage;

            if (brokeredmessage != null)
            {
                foreach (var header in messagecontext.Headers)
                {
                    brokeredmessage.Properties.Add(header.Key, header.Value);
                }

                if (!string.IsNullOrWhiteSpace(messagecontext.Origin.Name))
                {
                    brokeredmessage.Properties.Add(From, messagecontext.Origin.Name);
                }

                if (!string.IsNullOrWhiteSpace(messagecontext.Version))
                {
                    brokeredmessage.Properties.Add(Version, messagecontext.Version);
                }

                if (!string.IsNullOrWhiteSpace(messagecontext.Saga.Id))
                {
                    brokeredmessage.Properties.Add(SagaId, messagecontext.Saga.Id);
                }

                if (messagecontext.ScheduledEnqueueDateTimeUtc != null)
                {
                    brokeredmessage.ScheduledEnqueueTimeUtc = messagecontext.ScheduledEnqueueDateTimeUtc.Value;
                }

                if (!string.IsNullOrWhiteSpace(messagecontext.Origin.Key))
                {
                    brokeredmessage.Properties.Add(Origin, messagecontext.Origin.Key);
                }

                if (!string.IsNullOrWhiteSpace(messagecontext.Id))
                {
                    brokeredmessage.MessageId = messagecontext.Id;
                }

                brokeredmessage.Properties.Add(RetryCount, messagecontext.RetryCount.ToString());

                return message;
            }
            throw new ApplicationException($"Invalid message type {typeof(TMessage).FullName}");
        }
    }
}