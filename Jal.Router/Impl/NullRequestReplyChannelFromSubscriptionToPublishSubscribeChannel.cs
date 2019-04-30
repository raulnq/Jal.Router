using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl
{
    public class NullRequestReplyChannelFromSubscriptionToPublishSubscribeChannel : IRequestReplyChannelFromSubscriptionToPublishSubscribeChannel
    {
        public void Open(SenderMetadata metadata)
        {

        }

        public Task<string> Send(object message)
        {
            return Task.FromResult(string.Empty);
        }

        public Task Close()
        {
            return Task.CompletedTask;
        }

        public Task<MessageContext> Read(MessageContext context, IMessageAdapter adapter)
        {
            return Task.FromResult(default(MessageContext));
        }
    }
}