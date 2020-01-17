using Jal.Router.Interface;
using Jal.Router.Model;
using System;

namespace Jal.Router.Impl
{
    public class MessageAdapter : AbstractMessageAdapter
    {
        public MessageAdapter(IComponentFactoryGateway factory, IBus bus) : base(factory, bus)
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

        protected override MessageContext ReadMetadata(object message, IMessageSerializer serializer)
        {
            var fmessage = message as Message;

            if (fmessage != null)
            {
                var operationid = fmessage.OperationId;

                var parentid = fmessage.ParentId;

                var identitycontext = new IdentityContext(id: fmessage.Id, operationid: operationid, parentid: parentid, partitionid: fmessage.PartitionId, replytorequestid: fmessage.ReplyToRequestId, requestid: fmessage.RequestId);

                var trackings = fmessage.Trackings;

                var from = fmessage.From;

                var key = fmessage.Origin;

                var version = fmessage.Version;

                var sagaid = fmessage.SagaId;

                var contentid = fmessage.ContentId;

                var context = new MessageContext(Bus, identitycontext, DateTime.UtcNow, trackings, new Origin(from, key), sagaid, version, contentid);

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

            if (context.ContentContext.IsClaimCheck)
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
                filemessage.ContentId = context.ContentContext.ClaimCheckId;
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

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.Id))
            {
                filemessage.Id = context.IdentityContext.Id;
            }

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.ReplyToRequestId))
            {
                filemessage.ReplyToRequestId = context.IdentityContext.ReplyToRequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.RequestId))
            {
                filemessage.RequestId = context.IdentityContext.RequestId;
            }

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.PartitionId))
            {
                filemessage.PartitionId = context.IdentityContext.PartitionId;
            }

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.OperationId))
            {
                filemessage.OperationId = context.IdentityContext.OperationId;
            }

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.ParentId))
            {
                filemessage.ParentId = context.IdentityContext.ParentId;
            }

            return filemessage;
        }
    }
}