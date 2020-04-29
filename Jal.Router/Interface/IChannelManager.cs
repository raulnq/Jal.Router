using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelManager
    {
        Task<bool> CreateIfNotExist(Channel channel);

        Task<bool> DeleteIfExist(Channel channel);

        Task<Statistic> GetStatistic(Channel channel);
    }
}