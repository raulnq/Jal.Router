using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel : AbstractFileSystemRequestReply, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
    {
        public FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider, IFileSystemTransport transport)
            : base(factory, logger, provider, transport)
        {
        }

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            Thread.Sleep(500);

            var path = _transport.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, sendercontext.Channel.ReplyConnectionString, sendercontext.Channel.ReplyPath, sendercontext.Channel.Subscription);

            var message = _transport.ReadFile(path, sendercontext.MessageSerializer);

            return adapter.ReadFromPhysicalMessage(message, sendercontext);
        }
    }
}