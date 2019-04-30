using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractOutboundMessageHandler : AbstractMessageHandler
    {
        protected AbstractOutboundMessageHandler(IConfiguration configuration, IComponentFactoryGateway factory) : base(configuration, factory)
        {

        }

        protected override MessageEntity MessageContextToMessageEntity(MessageContext context, SagaEntity sagaentity)
        {
            var entity =  base.MessageContextToMessageEntity(context, sagaentity);

            entity.Type = MessageEntityType.Outbound;

            entity.ContentType = context.EndPoint.MessageType.FullName;

            entity.Name = context.EndPoint.Name;

            return entity;
        }
    }
}