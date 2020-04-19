using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelResourceManager<T,S>
    {
        Task<bool> CreateIfNotExist(T channel);

        Task<bool> DeleteIfExist(T channel);

        Task<S> Get(T channel);
    }
}