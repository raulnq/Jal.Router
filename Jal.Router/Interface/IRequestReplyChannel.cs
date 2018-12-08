using System;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Interface
{
    public interface IRequestReplyChannel
    {
        Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnPointToPointChannelMethodFactory(SenderMetadata metadata);

        Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnPublishSubscribeChannelMethodFactory(SenderMetadata metadata);

        Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata);

        Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata);

        Func<object[], object, string> SendMethodFactory(SenderMetadata metadata);
    }
}