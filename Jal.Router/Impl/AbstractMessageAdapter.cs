using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageAdapter : IMessageAdapter
    {
        protected readonly IComponentFactoryFacade Factory;

        protected readonly IBus Bus;

        public const string ClaimCheckId = "claimcheckid";

        public const string OperationId = "operationid";

        public const string ParentId = "parentid";

        public const string Trackings = "tracking";

        public const string From = "from";

        public const string SagaId = "sagaid";

        public const string Version = "version";

        public const string Origin = "origin";

        public const string EnclosedType = "enclosedtype";

        protected AbstractMessageAdapter(IComponentFactoryFacade factory, IBus bus)
        {
            Factory = factory;
            Bus = bus;
        }

        protected async Task<string> ReadContent(object message, string claimcheckid, bool useclaimcheck, IMessageStorage storage)
        {
            string data;

            if (useclaimcheck && !string.IsNullOrWhiteSpace(claimcheckid))
            {
                data = await storage.Read(claimcheckid).ConfigureAwait(false);
            }
            else
            {
                data = ReadContent(message);
            }

            return data;
        }

        public Task<MessageContext> ReadFromPhysicalMessage(object message, ListenerContext listener)
        {
            return Read(message, listener.Route, null, listener.Channel, listener.MessageSerializer, listener.MessageStorage, listener.EntityStorage);

        }

        public Task<MessageContext> ReadFromPhysicalMessage(object message, SenderContext sender)
        {
            //TODO Bug: Should be the a claim from the reply configuration
            return Read(message, null, sender.EndPoint, sender.Channel, sender.MessageSerializer, sender.MessageStorage, sender.EntityStorage);
        }

        public async Task<object> WritePhysicalMessage(MessageContext context, SenderContext sender)
        {
            if(context.ContentContext.UseClaimCheck)
            {
                await sender.MessageStorage.Write(context.ContentContext.ClaimCheckId, context.ContentContext.Data).ConfigureAwait(false);
            }

            var message = Write(context, sender.MessageSerializer);

            return message;
        }

        protected abstract object Write(MessageContext context, IMessageSerializer serializer);

        protected abstract Task<MessageContext> Read(object message, Route route, EndPoint endpoint, Channel channel, IMessageSerializer serializer, IMessageStorage messagestorage, IEntityStorage entitystorage);

        protected abstract string ReadContent(object message);
    }
}