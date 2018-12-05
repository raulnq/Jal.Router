using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Jal.Router.Impl.Inbound;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.InteropExtensions;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    internal static class StrictEncodings
    {
        public static UTF8Encoding Utf8 { get; } = new UTF8Encoding(false, true);
    }
    public class MessageAdapter : AbstractMessageAdapter
    {
        public MessageAdapter(IComponentFactory factory, IConfiguration configuration, IBus bus) : base(factory, configuration, bus)
        {
        }

        protected override MessageContext Read(object message)
        {
            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

                var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

                var context = new MessageContext(Bus, serializer, storage)
                {
                    DateTimeUtc = DateTime.UtcNow,
                };

                context.Identity.Id = sbmessage.MessageId;

                context.Identity.ReplyToRequestId = sbmessage.ReplyToSessionId;

                context.Identity.RequestId = sbmessage.SessionId;

                if (sbmessage.UserProperties.ContainsKey(OperationId))
                {
                    context.Identity.OperationId = sbmessage.UserProperties[OperationId].ToString();
                }

                if (sbmessage.UserProperties.ContainsKey(ParentId))
                {
                    context.Identity.ParentId = sbmessage.UserProperties[ParentId].ToString();
                }

                if (sbmessage.UserProperties.ContainsKey(From))
                {
                    context.Origin.From = sbmessage.UserProperties[From].ToString();
                }

                if (sbmessage.UserProperties.ContainsKey(ContentId))
                {
                    context.ContentId = sbmessage.UserProperties[ContentId].ToString();
                }

                if (sbmessage.UserProperties.ContainsKey(Tracks))
                {
                    context.Tracks = Deserialize(sbmessage.UserProperties[Tracks].ToString(), typeof(List<Track>)) as List<Track>;
                }

                if (sbmessage.UserProperties.ContainsKey(SagaId))
                {
                    context.SagaContext.Id = sbmessage.UserProperties[SagaId].ToString();
                }

                if (sbmessage.UserProperties.ContainsKey(Version))
                {
                    context.Version = sbmessage.UserProperties[Version].ToString();
                }

                if (sbmessage.UserProperties.ContainsKey(Origin))
                {
                    context.Origin.Key = sbmessage.UserProperties[Origin].ToString();
                }

                if (sbmessage.UserProperties.ContainsKey(RetryCount))
                {
                    context.RetryCount = Convert.ToInt32(sbmessage.UserProperties[RetryCount].ToString());
                }

                if (sbmessage.UserProperties != null)
                {
                    foreach (var property in sbmessage.UserProperties.Where(x => x.Key != From && x.Key != Origin && x.Key != Version 
                    && x.Key != RetryCount && x.Key != SagaId && x.Key!=Tracks && x.Key!= ContentId && x.Key != ParentId && x.Key != OperationId))
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
            var sbmessage = message as Message;

            if (sbmessage != null)
            {
                var bytes = sbmessage.Body;
                using (var stream = new MemoryStream(bytes))
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

                        var clonedMessage = sbmessage.Clone();
                        try
                        {
                            return clonedMessage.GetBody<string>();
                        }
                        catch (Exception exception)
                        {
                            var contentType = sbmessage.ContentType ?? "null";

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
            var brokeredmessage = new Message(Encoding.UTF8.GetBytes(context.Content)) { ContentType = "application/json" };

            foreach (var header in context.Headers)
            {
                brokeredmessage.UserProperties.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.From))
            {
                brokeredmessage.UserProperties.Add(From, context.Origin.From);
            }

            if (!string.IsNullOrWhiteSpace(context.Version))
            {
                brokeredmessage.UserProperties.Add(Version, context.Version);
            }

            if (!string.IsNullOrWhiteSpace(context.SagaContext.Id))
            {
                brokeredmessage.UserProperties.Add(SagaId, context.SagaContext.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.ContentId))
            {
                brokeredmessage.UserProperties.Add(ContentId, context.ContentId);
            }

            if (context.Tracks != null)
            {
                var root = Serialize(context.Tracks);

                if (!string.IsNullOrWhiteSpace(root))
                {
                    brokeredmessage.UserProperties.Add(Tracks, root);
                }
            }

            if (context.ScheduledEnqueueDateTimeUtc != null)
            {
                brokeredmessage.ScheduledEnqueueTimeUtc = context.ScheduledEnqueueDateTimeUtc.Value;
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.Key))
            {
                brokeredmessage.UserProperties.Add(Origin, context.Origin.Key);
            }

            if (!string.IsNullOrWhiteSpace(context.Identity.Id))
            {
                brokeredmessage.MessageId = context.Identity.Id;
            }

            if (!string.IsNullOrWhiteSpace(context.Identity.ReplyToRequestId))
            {
                brokeredmessage.ReplyToSessionId = context.Identity.ReplyToRequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.Identity.RequestId))
            {
                brokeredmessage.SessionId = context.Identity.RequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.Identity.OperationId))
            {
                brokeredmessage.UserProperties.Add(OperationId, context.Identity.OperationId);
            }

            if (!string.IsNullOrWhiteSpace(context.Identity.ParentId))
            {
                brokeredmessage.UserProperties.Add(ParentId, context.Identity.ParentId);
            }

            brokeredmessage.UserProperties.Add(RetryCount, context.RetryCount.ToString());

            return brokeredmessage;
        }
    }
}