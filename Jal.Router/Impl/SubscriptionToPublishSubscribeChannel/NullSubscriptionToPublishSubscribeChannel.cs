using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class NullSubscriptionToPublishSubscribeChannel : ISubscriptionToPublishSubscribeChannel
    {
        public void Open(ListenerContext listenercontext)
        {

        }

        public Task<Statistic> GetStatistic(Channel channel)
        {
            return Task.FromResult(default(Statistic));
        }

        public Task<bool> DeleteIfExist(Channel channel)
        {
            return Task.FromResult(false);
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

        public Task<bool> CreateIfNotExist(Channel channel)
        {
            return Task.FromResult(false);
        }
    }
}