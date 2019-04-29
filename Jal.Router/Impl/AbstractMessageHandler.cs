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

        protected readonly IComponentFactory Factory;

        protected AbstractMessageHandler(IConfiguration configuration, IComponentFactory factory)
        {
            Configuration = configuration;
            Factory = factory;
        }

        protected Task<SagaEntity> GetSagaEntity(MessageContext messagecontext)
        {
            var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

            return storage.GetSagaEntity(messagecontext.SagaContext.Id);
        }

        protected async Task<SagaEntity> CreateSagaEntity(MessageContext context)
        {
            var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

            var sagaentity = MessageContextToSagaEntity(context);

            await storage.CreateSagaEntity(context, sagaentity).ConfigureAwait(false);

            context.SagaContext.Id = sagaentity.Id;

            return sagaentity;
        }

        protected Task UpdateSagaEntity(MessageContext messagecontext, SagaEntity sagaentity)
        {
            var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

            sagaentity.Status = messagecontext.SagaContext.Status;

            return storage.UpdateSagaEntity(messagecontext, sagaentity);
        }

        protected async Task<MessageEntity> CreateMessageEntity(MessageContext messagecontext, MessageEntityType type = MessageEntityType.Inbound, SagaEntity sagaentity = null)
        {
            if (Configuration.Storage.Enabled)
            {
                try
                {
                    var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

                    var messageentity = MessageContextToMessageEntity(messagecontext, type, sagaentity);

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

        private SagaEntity MessageContextToSagaEntity(MessageContext context)
        {
            return new SagaEntity
            {
                Created = context.DateTimeUtc,
                Updated = context.DateTimeUtc,
                Name = context.Saga.Name,
                DataType = context.Saga.DataType.FullName,
                Timeout = context.Saga.Timeout,
                Status = context.SagaContext.Status,
                Data = string.Empty
            };
        }

        private MessageEntity MessageContextToMessageEntity(MessageContext context, MessageEntityType type, SagaEntity sagaentity)
        {

            var entity = new MessageEntity()
            {
                Content = context.Content,
                Identity = context.IdentityContext,
                Version = context.Version,
                RetryCount = context.RetryCount,
                LastRetry = context.LastRetry,
                Origin = context.Origin,
                Saga = context.SagaContext,
                Headers = context.Headers,
                DateTimeUtc = context.DateTimeUtc,
                Tracks = context.Tracks,
                ContentId = context.ContentId,
                Type = type,
            };

            if(sagaentity!=null)
            {
                entity.Data = sagaentity.Data;
                entity.SagaId = sagaentity.Id;
            }

            if(type== MessageEntityType.Inbound)
            {
                entity.ContentType = context.Route.ContentType.FullName;
                entity.Name = context.Route.Name;
            }

            if (type == MessageEntityType.Outbound)
            {
                entity.ContentType = context.EndPoint.MessageType.FullName;
                entity.Name = context.EndPoint.Name;
            }

            return entity;
        }
    }
}