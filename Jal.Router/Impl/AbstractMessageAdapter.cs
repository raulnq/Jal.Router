using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
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

        private async Task<MessageContext> ReadContent(object message, MessageContext context, Type contenttype, bool useclaimcheck)
        {
            string data;

            if (useclaimcheck && !string.IsNullOrWhiteSpace(context.ContentContext.Id))
            {
                var storage = Factory.CreateMessageStorage();

                data = await storage.Read(context.ContentContext.Id).ConfigureAwait(false);
            }
            else
            {
                data = ReadContent(message);
            }

            var contentcontext = new ContentContext(context,  context.ContentContext.Id, useclaimcheck, contenttype, data);

            context.SetContent(contentcontext);

            return context;
        }

        public Task<MessageContext> ReadMetadataAndContentFromPhysicalMessage(object message, Type contenttype, bool useclaimcheck)
        {
            var context = ReadMetadataFromPhysicalMessage(message);

            return ReadContent(message, context, contenttype, useclaimcheck);
        }

        public async Task<object> WritePhysicalMessage(MessageContext context)
        {
            if(context.ContentContext.IsClaimCheck)
            {
                var storage = Factory.CreateMessageStorage();

                context.ContentContext.GenerateId();

                await storage.Write(context.ContentContext.Id, context.ContentContext.Data).ConfigureAwait(false);
            }

            var serializer = Factory.CreateMessageSerializer();

            var message = Write(context, serializer);

            return message;
        }

        protected abstract object Write(MessageContext context, IMessageSerializer serializer);

        public MessageContext ReadMetadataFromPhysicalMessage(object message)
        {
            var serializer = Factory.CreateMessageSerializer();

            return ReadMetadata(message, serializer);
        }

        protected abstract MessageContext ReadMetadata(object message, IMessageSerializer serializer);

        protected abstract string ReadContent(object message);
    }
}