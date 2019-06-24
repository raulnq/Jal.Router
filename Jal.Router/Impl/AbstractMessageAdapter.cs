using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageAdapter : IMessageAdapter
    {
        protected readonly IComponentFactoryGateway Factory;

        protected readonly IBus Bus;

        public const string ContentId = "contentid";

        public const string OperationId = "operationid";

        public const string ParentId = "parentid";

        public const string Tracks = "tracking";

        public const string From = "from";

        public const string SagaId = "sagaid";

        public const string Version = "version";

        public const string Origin = "origin";

        public const string EnclosedType = "enclosedtype";

        protected AbstractMessageAdapter(IComponentFactoryGateway factory, IBus bus)
        {
            Factory = factory;
            Bus = bus;
        }

        private Task<MessageContext> ReadContentFromRoute(object message, MessageContext context, Route route)
        {
            return ReadContent(message, context, route.ContentType, route.UseClaimCheck);
        }

        private Task<MessageContext> ReadContentFromEndpoint(object message, MessageContext context, EndPoint endpoint)
        {
            return ReadContent(message, context, endpoint.ReplyType, endpoint.UseClaimCheck);
        }

        private async Task<MessageContext> ReadContent(object message, MessageContext context, Type contenttype, bool useclaimcheck)
        {
            context.ContentType = contenttype;

            if (useclaimcheck && !string.IsNullOrWhiteSpace(context.ContentId))
            {
                var storage = Factory.CreateMessageStorage();

                context.Content = await storage.Read(context.ContentId).ConfigureAwait(false);
            }
            else
            {
                context.Content = ReadContent(message);
            }

            return context;
        }

        public Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint enpdoint)
        {
            var context = ReadMetadata(message);

            return ReadContentFromEndpoint(message, context, enpdoint);
        }

        public Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route, Channel channel, Saga saga = null)
        {
            var context = ReadMetadata(message);

            context.UpdateFromRoute(route, channel, saga);

            return ReadContentFromRoute(message, context, route);
        }

        public async Task<object> WriteMetadataAndContent(MessageContext context, EndPoint enpdoint)
        {
            var content = context.Content;

            if(enpdoint.UseClaimCheck)
            {
                var storage = Factory.CreateMessageStorage();

                context.ContentId = Guid.NewGuid().ToString();

                await storage.Write(context.ContentId, context.Content).ConfigureAwait(false);

                context.Content = string.Empty;
            }

            var message = WriteMetadataAndContent(context);

            context.Content = content;

            return message;
        }

        protected abstract object WriteMetadataAndContent(MessageContext context);

        public abstract MessageContext ReadMetadata(object message);

        protected abstract string ReadContent(object message);
    }
}