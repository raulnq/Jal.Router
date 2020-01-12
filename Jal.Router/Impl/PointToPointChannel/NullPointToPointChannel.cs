using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class NullPointToPointChannel : IPointToPointChannel
    {
        public void Open(ListenerContext listenercontext)
        {

        }

        public bool IsActive(ListenerContext listenercontext)
        {
            return false;
        }

        public Task Close(ListenerContext listenercontext)
        {
            return Task.CompletedTask;
        }

        public void Listen(ListenerContext listenercontext)
        {

        }

        public void Open(SenderContext sendercontext)
        {

        }

        public Task<string> Send(SenderContext sendercontext, object message)
        {
            return Task.FromResult(string.Empty);
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return false;
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }
    }
}