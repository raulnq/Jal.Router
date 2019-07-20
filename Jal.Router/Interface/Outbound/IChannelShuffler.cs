using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IChannelShuffler
    {
        Channel[] Shuffle(Channel[] channel);
    }
}