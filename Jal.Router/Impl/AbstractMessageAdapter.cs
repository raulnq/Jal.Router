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

        public const string Trackings = "tracking";

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
            return ReadContent(message, context, endpoint.ReplyContentType, endpoint.UseClaimCheck);
        }

        private async Task<MessageContext> ReadContent(object message, MessageContext context, Type contenttype, bool useclaimcheck)
        {
            context.ContentContext.UpdateType(contenttype);

            if (useclaimcheck && !string.IsNullOrWhiteSpace(context.ContentContext.Id))
            {
                var storage = Factory.CreateMessageStorage();

                context.ContentContext.UpdateData(await storage.Read(context.ContentContext.Id).ConfigureAwait(false));
            }
            else
            {
                context.ContentContext.UpdateData(ReadContent(message));
            }

            return context;
        }

        public Task<MessageContext> ReadMetadataAndContentFromEndpoint(object message, EndPoint enpdoint)
        {
            var context = ReadMetadata(message);

            return ReadContentFromEndpoint(message, context, enpdoint);
        }

        public Task<MessageContext> ReadMetadataAndContentFromRoute(object message, Route route)
        {
            var context = ReadMetadata(message);

            context.UpdateRoute(route);

            return ReadContentFromRoute(message, context, route);
        }

        public async Task<object> WriteMetadataAndContent(MessageContext context, EndPoint enpdoint)
        {
            var content = context.ContentContext.Data;

            if(enpdoint.UseClaimCheck)
            {
                var storage = Factory.CreateMessageStorage();

                context.ContentContext.CreateId();

                await storage.Write(context.ContentContext.Id, context.ContentContext.Data).ConfigureAwait(false);

                context.ContentContext.CleanData();
            }

            var message = WriteMetadataAndContent(context);

            context.ContentContext.UpdateData(content);

            return message;
        }

        protected abstract object WriteMetadataAndContent(MessageContext context);

        public abstract MessageContext ReadMetadata(object message);

        protected abstract string ReadContent(object message);
    }
}