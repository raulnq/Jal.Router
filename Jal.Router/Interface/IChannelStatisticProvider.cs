using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelStatisticProvider
    {
        Task<Statistic> GetStatistic(Channel channel);
    }
}