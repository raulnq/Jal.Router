﻿using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class MessageAdapter : AbstractMessageAdapter
    {
        public MessageAdapter(IComponentFactoryFacade factory, IBus bus) : base(factory, bus)
        {
        }

        protected override string ReadContent(object message)
        {
            var fmessage = message as Message;

            if (fmessage != null)
            {
                return fmessage.Content;
            }
            throw new ApplicationException($"Invalid message type {message.GetType().FullName}");
        }

        protected override async Task<MessageContext> Read(object message, Route route, EndPoint endpoint, Channel channel, IMessageSerializer serializer, IMessageStorage storage)
        {
            var fmessage = message as Message;

            if (fmessage != null)
            {
                var operationid = fmessage.OperationId;

                var parentid = fmessage.ParentId;

                var tracingcontext = new TracingContext(id: fmessage.Id, operationid: operationid, parentid: parentid, partitionid: fmessage.PartitionId, replytorequestid: fmessage.ReplyToRequestId, requestid: fmessage.RequestId);

                var trackings = fmessage.Trackings;

                var from = fmessage.From;

                var key = fmessage.Origin;

                var version = fmessage.Version;

                var sagaid = fmessage.SagaId;

                var claimcheckid = fmessage.ClaimCheckId;

                var content = await ReadContent(fmessage, claimcheckid, channel.UseClaimCheck, storage).ConfigureAwait(false);

                var context = new MessageContext(Bus, serializer, route, endpoint, channel, tracingcontext, DateTime.UtcNow, trackings, new Origin(from, key), sagaid, version, claimcheckid, content);

                if (fmessage.Headers != null)
                {
                    foreach (var property in fmessage.Headers)
                    {
                        context.Headers.Add(property.Key, property.Value?.ToString());
                    }
                }

                return context;
            }
            throw new ApplicationException($"Invalid message type {message.GetType().FullName}");
        }

        protected override object Write(MessageContext context, IMessageSerializer serializer)
        {
            var data = context.ContentContext.Data;

            if (context.ContentContext.UseClaimCheck)
            {
                data = string.Empty;
            }

            var filemessage = new Message(){ ContentType = "application/json", Content = data };

            foreach (var header in context.Headers)
            {
                filemessage.Headers.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.From))
            {
                filemessage.From = context.Origin.From;
            }

            if (!string.IsNullOrWhiteSpace(context.Version))
            {
                filemessage.Version = context.Version;
            }

            if (!string.IsNullOrWhiteSpace(context.SagaContext.Id))
            {
                filemessage.SagaId = context.SagaContext.Id;
            }

            if (!string.IsNullOrWhiteSpace(context.ContentContext.ClaimCheckId))
            {
                filemessage.ClaimCheckId = context.ContentContext.ClaimCheckId;
            }

            if (context.TrackingContext != null)
            {
                filemessage.Trackings = context.TrackingContext.Trackings;
            }

            if (context.ScheduledEnqueueDateTimeUtc != null)
            {
                filemessage.ScheduledEnqueueTimeUtc = context.ScheduledEnqueueDateTimeUtc.Value;
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.Key))
            {
                filemessage.Origin = context.Origin.Key;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.Id))
            {
                filemessage.Id = context.TracingContext.Id;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.ReplyToRequestId))
            {
                filemessage.ReplyToRequestId = context.TracingContext.ReplyToRequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.RequestId))
            {
                filemessage.RequestId = context.TracingContext.RequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.PartitionId))
            {
                filemessage.PartitionId = context.TracingContext.PartitionId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.OperationId))
            {
                filemessage.OperationId = context.TracingContext.OperationId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.ParentId))
            {
                filemessage.ParentId = context.TracingContext.ParentId;
            }

            return filemessage;
        }
    }
}