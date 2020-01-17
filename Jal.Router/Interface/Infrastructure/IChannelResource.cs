using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IChannelResource<T,S>
    {
        Task<bool> CreateIfNotExist(T channel);

        Task<bool> DeleteIfExist(T channel);

        Task<S> Get(T channel);
    }
}