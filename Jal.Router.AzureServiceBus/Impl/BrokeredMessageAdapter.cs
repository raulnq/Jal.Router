using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Jal.Router.Impl.Inbound;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
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
        public BrokeredMessageAdapter(IComponentFactory factory, IConfiguration configuration, IBus bus, IStorageFacade facade) : base(factory, configuration, bus, facade)
        {
        }

        public override MessageContext Read(object message)
        {
            var brokeredmessage = message as BrokeredMessage;

            if (brokeredmessage != null)
            {
                var context = new MessageContext(Bus, Facade)
                {
                    Id = brokeredmessage.MessageId,
                    DateTimeUtc = DateTime.UtcNow,
                    ReplyToRequestId = brokeredmessage.ReplyToSessionId,
                    RequestId = brokeredmessage.SessionId
                };

                if (brokeredmessage.Properties.ContainsKey(From))
                {
                    context.Origin.Name = brokeredmessage.Properties[From].ToString();
                }

                if (brokeredmessage.Properties.ContainsKey(SagaId))
                {
                    context.SagaInfo.Id = brokeredmessage.Properties[SagaId].ToString();
                }

                if (brokeredmessage.Properties.ContainsKey(ParentSagaId))
                {
                    context.SagaInfo.ParentId = brokeredmessage.Properties[ParentSagaId].ToString();
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
                    foreach (var property in brokeredmessage.Properties.Where(x => x.Key != From && x.Key != Origin && x.Key != Version && x.Key != RetryCount && x.Key != SagaId && x.Key != ParentSagaId))
                    {
                        context.Headers.Add(property.Key, property.Value?.ToString());
                    }
                }

                return context;
            }
            throw new ApplicationException($"Invalid message type {message.GetType().FullName}");
        }

        public override string GetBody(object message)
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
            throw new ApplicationException($"Invalid message type {message.GetType().FullName}");
        }

        public override object Write(MessageContext context)
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

            if (!string.IsNullOrWhiteSpace(context.SagaInfo.ParentId))
            {
                brokeredmessage.Properties.Add(ParentSagaId, context.SagaInfo.ParentId);
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

            if (!string.IsNullOrWhiteSpace(context.ReplyToRequestId))
            {
                brokeredmessage.ReplyToSessionId = context.ReplyToRequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.RequestId))
            {
                brokeredmessage.SessionId = context.RequestId;
            }

            brokeredmessage.Properties.Add(RetryCount, context.RetryCount.ToString());

            return brokeredmessage;
        }
    }
}