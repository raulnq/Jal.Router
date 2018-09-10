using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound.Sagas
{
    public abstract class AbstractMessageHandler
    {
        protected readonly IConfiguration Configuration;

        protected AbstractMessageHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected void SaveSaga(MessageContext messagecontext, ISagaStorage storage, SagaEntity sagaentity, IMessageSerializer serializer, object data)
        {
            sagaentity.Data = serializer.Serialize(data);

            sagaentity.Status = messagecontext.SagaContext.Status;

            storage.UpdateSaga(messagecontext, messagecontext.SagaContext.Id, sagaentity);
        }

        protected void SaveMessage(MessageContext messagecontext, ISagaStorage storage, SagaEntity sagaentity)
        {
            if (Configuration.Storage.SaveMessage)
            {
                try
                {
                    var messageentity = CreateMessageEntity(messagecontext, sagaentity);

                    storage.CreateMessage(messagecontext, messagecontext.SagaContext.Id, sagaentity, messageentity);
                }
                catch (Exception)
                {
                    if (!Configuration.Storage.IgnoreExceptionOnSaveMessage)
                    {
                        throw;
                    }
                }
            }
        }

        protected SagaEntity CreateSagaEntity(MessageContext context)
        {
            return new SagaEntity
            {
                Created = context.DateTimeUtc,
                Updated = context.DateTimeUtc,
                Name = context.Saga.Name,
                DataType = context.Saga.DataType.FullName,
                Timeout = context.Saga.Timeout,
                Status = context.SagaContext.Status,
                Key = Guid.NewGuid().ToString()
            };
        }

        protected static MessageEntity CreateMessageEntity(MessageContext context, SagaEntity sagaentity)
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
                ContentId = context.ContentId
            };
        }
    }
}