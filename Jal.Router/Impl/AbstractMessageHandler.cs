using System;
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

        protected SagaEntity GetSagaEntity(MessageContext messagecontext)
        {
            var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

            var sagaentity = storage.GetSagaEntity(messagecontext.SagaContext.Id);

            return sagaentity;
        }

        protected SagaEntity CreateSagaEntity(MessageContext context, object data)
        {
            var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

            var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            var sagaentity = MessageContextToSagaEntity(context);

            sagaentity.Data = serializer.Serialize(data);

            storage.CreateSagaEntity(context, sagaentity);

            context.SagaContext.Id = sagaentity.Id;

            return sagaentity;
        }

        protected void UpdateSagaEntity(MessageContext messagecontext, SagaEntity sagaentity, object data)
        {
            var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

            var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

            sagaentity.Data = serializer.Serialize(data);

            sagaentity.Status = messagecontext.SagaContext.Status;

            storage.UpdateSagaEntity(messagecontext, sagaentity);
        }

        protected MessageEntity CreateInboundMessageEntity(MessageContext messagecontext)
        {
            if (Configuration.Storage.Enabled)
            {
                try
                {
                    var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

                    var messageentity = MessageContextToInboundMessageEntity(messagecontext);

                    storage.CreateMessageEntity(messagecontext, messageentity);

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

        protected MessageEntity CreateInboundMessageEntity(MessageContext messagecontext, SagaEntity sagaentity)
        {
            if (Configuration.Storage.Enabled)
            {
                try
                {
                    var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

                    var messageentity = MessageContextToMessageEntity(messagecontext, sagaentity);

                    storage.CreateMessageEntity(messagecontext, sagaentity, messageentity);

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

        protected MessageEntity CreateOutboundMessageEntity(MessageContext messagecontext)
        {
            if (Configuration.Storage.Enabled)
            {
                try
                {
                    var storage = Factory.Create<IEntityStorage>(Configuration.StorageType);

                    var messageentity = MessageContextToOuboundMessageEntity(messagecontext);

                    storage.CreateMessageEntity(messagecontext, messageentity);

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
            };
        }

        private MessageEntity MessageContextToMessageEntity(MessageContext context, SagaEntity sagaentity)
        {
            return new MessageEntity
            {
                Content = context.Content,
                ContentType = context.Route.ContentType.FullName,
                Identity = context.Identity,
                Version = context.Version,
                RetryCount = context.RetryCount,
                LastRetry = context.LastRetry,
                Origin = context.Origin,
                Saga = context.SagaContext,
                Headers = context.Headers,
                DateTimeUtc = context.DateTimeUtc,
                Data = sagaentity.Data,
                Name = context.Route.Name,
                Tracks = context.Tracks,
                ContentId = context.ContentId,
                Type = MessageEntityType.Input,
            };
        }
        private MessageEntity MessageContextToInboundMessageEntity(MessageContext context)
        {

            return new MessageEntity()
            {
                Content = context.Content,
                ContentType = context.Route.ContentType.FullName,
                Identity = context.Identity,
                Version = context.Version,
                RetryCount = context.RetryCount,
                LastRetry = context.LastRetry,
                Origin = context.Origin,

                Headers = context.Headers,
                DateTimeUtc = context.DateTimeUtc,
                Data = string.Empty,
                Name = context.Route.Name,
                Tracks = context.Tracks,
                ContentId = context.ContentId,
                Type = MessageEntityType.Input,
            };
        }

        private MessageEntity MessageContextToOuboundMessageEntity(MessageContext context)
        {

            return new MessageEntity()
            {
                Content = context.Content,
                ContentType = context.EndPoint.MessageType.FullName,
                Identity = context.Identity,
                Version = context.Version,
                Origin = context.Origin,
                Saga = context.SagaContext,
                Headers = context.Headers,
                DateTimeUtc = context.DateTimeUtc,
                Data = string.Empty,
                Name = context.EndPoint.Name,
                Tracks = context.Tracks,
                ContentId = context.ContentId,
                Type = MessageEntityType.Output,
            };
        }
    }
}