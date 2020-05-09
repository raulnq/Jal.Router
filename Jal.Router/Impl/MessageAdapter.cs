using Jal.Router.Interface;
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

        protected override async Task<MessageContext> Read(object message, Route route, EndPoint endpoint, Channel channel, IMessageSerializer serializer, IMessageStorage messagestorage, IEntityStorage entitystorage)
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

                var content = await ReadContent(fmessage, claimcheckid, channel.UseClaimCheck, messagestorage).ConfigureAwait(false);

                var context = MessageContext.CreateFromListen(Bus, serializer, entitystorage, route, endpoint, channel, tracingcontext, trackings, new Origin(from, key), content, sagaid, claimcheckid, DateTime.UtcNow, version);

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

            var message = new Message(){ ContentType = "application/json", Content = data };

            foreach (var header in context.Headers)
            {
                message.Headers.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.From))
            {
                message.From = context.Origin.From;
            }

            if (!string.IsNullOrWhiteSpace(context.Version))
            {
                message.Version = context.Version;
            }

            if (!string.IsNullOrWhiteSpace(context.SagaContext.Id))
            {
                message.SagaId = context.SagaContext.Id;
            }

            if (!string.IsNullOrWhiteSpace(context.ContentContext.ClaimCheckId))
            {
                message.ClaimCheckId = context.ContentContext.ClaimCheckId;
            }

            if (context.TrackingContext != null)
            {
                message.Trackings = context.TrackingContext.Trackings;
            }

            if (context.ScheduledEnqueueDateTimeUtc != null)
            {
                message.ScheduledEnqueueTimeUtc = context.ScheduledEnqueueDateTimeUtc.Value;
            }

            if (!string.IsNullOrWhiteSpace(context.Origin.Key))
            {
                message.Origin = context.Origin.Key;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.Id))
            {
                message.Id = context.TracingContext.Id;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.ReplyToRequestId))
            {
                message.ReplyToRequestId = context.TracingContext.ReplyToRequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.RequestId))
            {
                message.RequestId = context.TracingContext.RequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.PartitionId))
            {
                message.PartitionId = context.TracingContext.PartitionId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.OperationId))
            {
                message.OperationId = context.TracingContext.OperationId;
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.ParentId))
            {
                message.ParentId = context.TracingContext.ParentId;
            }

            return message;
        }
    }
}