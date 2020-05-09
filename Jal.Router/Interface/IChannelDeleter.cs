using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelDeleter
    {
        Task<bool> DeleteIfExist(Channel channel);
    }
}