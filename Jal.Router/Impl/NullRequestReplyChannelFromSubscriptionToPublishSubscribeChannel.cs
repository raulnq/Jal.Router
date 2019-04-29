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

        public string Send(object message)
        {
            return string.Empty;
        }

        public Task Close()
        {
            return Task.CompletedTask;
        }

        public MessageContext Read(MessageContext context, IMessageAdapter adapter)
        {
            return null;
        }
    }
}