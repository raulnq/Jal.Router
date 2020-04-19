using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel : AbstractInMemoryRequestReply, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
    {
        public InMemoryRequestReplyFromSubscriptionToPublishSubscribeChannel(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider, IInMemoryTransport transport)
            : base(factory, logger, provider, transport)
        {
        }

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            throw new System.NotSupportedException();
        }
    }
}