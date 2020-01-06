using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel : AbstractFileSystemRequestReply, IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
    {
        public FileSystemRequestReplyFromSubscriptionToPublishSubscribeChannel(IComponentFactoryGateway factory, ILogger logger, IParameterProvider provider, IFileSystem filesystem)
            : base(factory, logger, provider, filesystem)
        {
        }

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            Thread.Sleep(500);

            var path = _filesystem.CreateSubscriptionToPublishSubscribeChannelPath(_parameter, sendercontext.Channel.ToReplyConnectionString, sendercontext.Channel.ToReplyPath, sendercontext.Channel.ToSubscription);

            var message = _filesystem.ReadFile(path);

            var outputcontext = adapter.ReadMetadataFromPhysicalMessage(message);

            return Task.FromResult(outputcontext);
        }
    }
}