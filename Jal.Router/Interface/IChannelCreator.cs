using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelCreator
    {
        Task<bool> CreateIfNotExist(Channel channel);
    }
}