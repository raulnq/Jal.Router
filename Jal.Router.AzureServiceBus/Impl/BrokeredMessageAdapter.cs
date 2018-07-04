using System;
using System.Collections.Generic;
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
        public BrokeredMessageAdapter(IComponentFactory factory, IConfiguration configuration, IBus bus) : base(factory, configuration, bus)
        {
        }

        protected override MessageContext Read(object message)
        {
            var brokeredmessage = message as BrokeredMessage;

            if (brokeredmessage != null)
            {
                var storage = Factory.Create<ISagaStorage>(Configuration.SagaStorageType);

                var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

                var context = new MessageContext(Bus, serializer, storage)
                {
                    Id = brokeredmessage.MessageId,
                    DateTimeUtc = DateTime.UtcNow,
                    ReplyToRequestId = brokeredmessage.ReplyToSessionId,
                    RequestId = brokeredmessage.SessionId
                };

                if (brokeredmessage.Properties.ContainsKey(From))
                {
                    context.Origin.From = brokeredmessage.Properties[From].ToString();
                }

                if (brokeredmessage.Properties.ContainsKey(DataId))
                {
                    context.DataId = brokeredmessage.Properties[DataId].ToString();
                }

                if (brokeredmessage.Properties.ContainsKey(Tracks))
                {
                    context.Tracks = Deserialize(brokeredmessage.Properties[Tracks].ToString(), typeof(List<Track>)) as List<Track>;
                }

                if (brokeredmessage.Properties.ContainsKey(SagaId))
                {
                    context.SagaContext.Id = brokeredmessage.Properties[SagaId].ToString();
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
                    foreach (var property in brokeredmessage.Properties.Where(x => x.Key != From && x.Key != Origin && x.Key != Version 
                    && x.Key != RetryCount && x.Key != SagaId && x.Key!=Tracks && x.Key!=DataId))
                    {
                        context.Headers.Add(property.Key, property.Value?.ToString());
                    }
                }

                return context;
            }
            throw new ApplicationException($"Invalid message type {message.GetType().FullName}");
        }

        protected override string GetContent(object message)
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

        protected override object Write(MessageContext context)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(context.Content));

            var brokeredmessage = new BrokeredMessage(stream, true) { ContentType = "application/json" };

            foreach (var header in context.Headers)
            {
                brokeredmessage.Properties.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.From))
            {
                brokeredmessage.Properties.Add(From, context.Origin.From);
            }

            if (!string.IsNullOrWhiteSpace(context.Version))
            {
                brokeredmessage.Properties.Add(Version, context.Version);
            }

            if (!string.IsNullOrWhiteSpace(context.SagaContext.Id))
            {
                brokeredmessage.Properties.Add(SagaId, context.SagaContext.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.DataId))
            {
                brokeredmessage.Properties.Add(DataId, context.DataId);
            }

            if (context.Tracks != null)
            {
                var root = Serialize(context.Tracks);

                if (!string.IsNullOrWhiteSpace(root))
                {
                    brokeredmessage.Properties.Add(Tracks, root);
                }
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