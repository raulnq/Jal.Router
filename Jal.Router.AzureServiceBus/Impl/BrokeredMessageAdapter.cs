using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Jal.Router.Impl.Inbound;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    internal static class StrictEncodings
    {
        public static UTF8Encoding Utf8 { get; } = new UTF8Encoding(false, true);
    }
    public class BrokeredMessageAdapter : AbstractMessageAdapter
    {
        public BrokeredMessageAdapter(IComponentFactory factory, IConfiguration configuration) : base(factory, configuration)
        {
        }

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
                    context.SagaInfo.Id = brokeredmessage.Properties[SagaId].ToString();
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

        public override string ReadBody<TMessage>(TMessage message)
        {
            var brokeredmesage = message as BrokeredMessage;

            if (brokeredmesage != null)
            {
                using (var stream = brokeredmesage.GetBody<Stream>())
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    using (TextReader reader = new StreamReader(stream, StrictEncodings.Utf8))
                    {

                        try
                        {
                            return reader.ReadToEnd();
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        var clonedMessage = brokeredmesage.Clone();
                        try
                        {
                            return clonedMessage.GetBody<string>();
                        }
                        catch (Exception exception)
                        {
                            var contentType = brokeredmesage.ContentType ?? "null";

                            var msg = string.Format(CultureInfo.InvariantCulture, "The BrokeredMessage with ContentType '{0}' failed to deserialize to a string with the message: '{1}'", contentType, exception.Message);

                            throw new InvalidOperationException(msg, exception);
                        }

                    }
                }
            }
            throw new ApplicationException($"Invalid message type {typeof(TMessage).FullName}");
        }

        public override TMessage Write<TContent, TMessage>(MessageContext<TContent> context)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(context.ContentAsString));

            var brokeredmessage = new BrokeredMessage(stream, true) { ContentType = "application/json" };

            foreach (var header in context.Headers)
            {
                brokeredmessage.Properties.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.Name))
            {
                brokeredmessage.Properties.Add(From, context.Origin.Name);
            }

            if (!string.IsNullOrWhiteSpace(context.Version))
            {
                brokeredmessage.Properties.Add(Version, context.Version);
            }

            if (!string.IsNullOrWhiteSpace(context.SagaInfo.Id))
            {
                brokeredmessage.Properties.Add(SagaId, context.SagaInfo.Id);
            }

            if (context.ScheduledEnqueueDateTimeUtc != null)
            {
                brokeredmessage.ScheduledEnqueueTimeUtc = context.ScheduledEnqueueDateTimeUtc.Value;
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.Key))
            {
                brokeredmessage.Properties.Add(Origin, context.Origin.Key);
            }

            if (!string.IsNullOrWhiteSpace(context.Id))
            {
                brokeredmessage.MessageId = context.Id;
            }

            brokeredmessage.Properties.Add(RetryCount, context.RetryCount.ToString());

            return (TMessage)Convert.ChangeType(brokeredmessage, typeof(TMessage));
        }
    }
}