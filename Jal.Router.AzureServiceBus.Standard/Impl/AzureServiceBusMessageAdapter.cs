using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.InteropExtensions;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    internal static class StrictEncodings
    {
        public static UTF8Encoding Utf8 { get; } = new UTF8Encoding(false, true);
    }
    public class AzureServiceBusMessageAdapter : AbstractMessageAdapter
    {
        public AzureServiceBusMessageAdapter(IComponentFactoryFacade factory, IBus bus) : base(factory, bus)
        {
        }

        protected override MessageContext ReadMetadata(object message, IMessageSerializer serializer)
        {
            var sbmessage = message as Microsoft.Azure.ServiceBus.Message;

            if (sbmessage != null)
            {
                var operationid = string.Empty;

                if (sbmessage.UserProperties.ContainsKey(OperationId))
                {
                    operationid = sbmessage.UserProperties[OperationId].ToString();
                }

                var parentid = string.Empty;

                if (sbmessage.UserProperties.ContainsKey(ParentId))
                {
                    parentid = sbmessage.UserProperties[ParentId].ToString();
                }

                var tracingcontext = new TracingContext(id: sbmessage.MessageId, operationid: operationid, parentid: parentid, partitionid: sbmessage.SessionId, replytorequestid: sbmessage.ReplyToSessionId, requestid: sbmessage.SessionId);

                var trackings = new List<Tracking>();

                if (sbmessage.UserProperties.ContainsKey(Trackings))
                {
                    trackings = serializer.Deserialize(sbmessage.UserProperties[Trackings].ToString(), typeof(List<Tracking>)) as List<Tracking>;
                }

                var from = string.Empty;

                if (sbmessage.UserProperties.ContainsKey(From))
                {
                    from = sbmessage.UserProperties[From].ToString();
                }

                var key = string.Empty;

                if (sbmessage.UserProperties.ContainsKey(Origin))
                {
                    key = sbmessage.UserProperties[Origin].ToString();
                }

                var version = string.Empty;

                if (sbmessage.UserProperties.ContainsKey(Version))
                {
                    version = sbmessage.UserProperties[Version].ToString();
                }

                var sagaid = string.Empty;

                if (sbmessage.UserProperties.ContainsKey(SagaId))
                {
                    sagaid = sbmessage.UserProperties[SagaId].ToString();
                }

                var claimcheckid = string.Empty;

                if (sbmessage.UserProperties.ContainsKey(ClaimCheckId))
                {
                    claimcheckid = sbmessage.UserProperties[ClaimCheckId].ToString();
                }

                var context = new MessageContext(Bus, tracingcontext, DateTime.UtcNow, trackings, new Origin(from, key), sagaid, version, claimcheckid);

                if (sbmessage.UserProperties != null)
                {
                    foreach (var property in sbmessage.UserProperties.Where(x => x.Key != From && x.Key != Origin && x.Key != Version 
                    &&  x.Key != SagaId && x.Key!=Trackings && x.Key!= ClaimCheckId && x.Key != ParentId && x.Key != OperationId))
                    {
                        context.Headers.Add(property.Key, property.Value?.ToString());
                    }
                }

                return context;
            }
            throw new ApplicationException($"Invalid message type {message.GetType().FullName}");
        }

        protected override string ReadContent(object message)
        {
            var sbmessage = message as Microsoft.Azure.ServiceBus.Message;

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

        protected override object Write(MessageContext context, IMessageSerializer serializer)
        {
            var data = context.ContentContext.Data;

            if(context.ContentContext.IsClaimCheck)
            {
                data = string.Empty;
            }

            var brokeredmessage = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(data)) { ContentType = "application/json" };

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

            if (!string.IsNullOrWhiteSpace(context.ContentContext.ClaimCheckId))
            {
                brokeredmessage.UserProperties.Add(ClaimCheckId, context.ContentContext.ClaimCheckId);
            }

            if (context.TrackingContext != null)
            {
                var root = serializer.Serialize(context.TrackingContext.Trackings);

                if (!string.IsNullOrWhiteSpace(root))
                {
                    brokeredmessage.UserProperties.Add(Trackings, root);
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

            if (!string.IsNullOrWhiteSpace(context.TracingContext.Id))
            {
                brokeredmessage.MessageId = context.TracingContext.Id;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.ReplyToRequestId))
            {
                brokeredmessage.ReplyToSessionId = context.TracingContext.ReplyToRequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.RequestId))
            {
                brokeredmessage.SessionId = context.TracingContext.RequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.PartitionId))
            {
                brokeredmessage.SessionId = context.TracingContext.PartitionId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.OperationId))
            {
                brokeredmessage.UserProperties.Add(OperationId, context.TracingContext.OperationId);
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.ParentId))
            {
                brokeredmessage.UserProperties.Add(ParentId, context.TracingContext.ParentId);
            }

            return brokeredmessage;
        }
    }
}