using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullRequestReplyChannelFromPointToPointChannel : IRequestReplyChannelFromPointToPointChannel
    {
        public void Open(SenderContext sendercontext)
        {

        }

        public Task<string> Send(SenderContext sendercontext, object message)
        {
            return Task.FromResult(string.Empty);
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }

        public Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            return Task.FromResult(default(MessageContext));
        }

        public bool IsActive(SenderContext context)
        {
            return false;

        }
    }
}