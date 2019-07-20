using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class DefaultChannelShuffler : IChannelShuffler
    {
        public Channel[] Shuffle(Channel[] channel)
        {
            return channel;
        }
    }
}