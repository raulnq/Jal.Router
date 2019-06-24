using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageHandler
    {
        protected readonly IConfiguration Configuration;

        protected readonly IComponentFactoryGateway Factory;

        protected AbstractMessageHandler(IConfiguration configuration, IComponentFactoryGateway factory)
        {
            Configuration = configuration;

            Factory = factory;
        }

        protected async Task<MessageEntity> CreateMessageEntityAndSave(MessageContext messagecontext)
        {
            if (Configuration.Storage.Enabled)
            {
                try
                {
                    var storage = Factory.CreateEntityStorage();

                    var messageentity = MessageContextToMessageEntity(messagecontext);

                    await storage.CreateMessageEntity(messagecontext, messageentity).ConfigureAwait(false);

                    return messageentity;
                }
                catch (Exception)
                {
                    if (!Configuration.Storage.IgnoreExceptions)
                    {
                        throw;
                    }
                }
            }

            return null;
        }

        protected virtual MessageEntity MessageContextToMessageEntity(MessageContext context)
        {

            var entity = new MessageEntity()
            {
                Content = context.Content,
                Identity = context.IdentityContext,
                Version = context.Version,
                Origin = context.Origin,
                Saga = context.SagaContext,
                Headers = context.Headers,
                DateTimeUtc = context.DateTimeUtc,
                Tracks = context.Tracks,
                ContentId = context.ContentId,
            };

            return entity;
        }
    }
}