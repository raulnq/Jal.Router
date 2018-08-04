using Jal.Router.Model;

namespace Jal.Router.Interface.Outbound
{
    public interface IChannelShuffler
    {
        Channel[] Shuffle(Channel[] channel);
    }
}