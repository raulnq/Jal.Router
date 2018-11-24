using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl
{
    public class NullRequestReplyChannel : IRequestReplyChannel
    {
        public Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnPointToPointChannelMethodFactory(SenderMetadata metadata)
        {
            return (c, a) => null;
        }

        public Func<MessageContext, IMessageAdapter, MessageContext> ReceiveOnPublishSubscriberChannelMethodFactory(SenderMetadata metadata)
        {
            return (c, a) => null;
        }


        public Func<object[]> CreateSenderMethodFactory(SenderMetadata metadata)
        {
            return () => new object[] { };
        }

        public Action<object[]> DestroySenderMethodFactory(SenderMetadata metadata)
        {
            return o => { };
        }

        public Func<object[], object, string> SendMethodFactory(SenderMetadata metadata)
        {
            return (o, m) => string.Empty;
        }
    }
}