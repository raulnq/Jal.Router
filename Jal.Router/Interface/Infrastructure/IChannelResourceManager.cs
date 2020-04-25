using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IResourceManager
    {
        Task<bool> CreateIfNotExist(Resource resource);

        Task<bool> DeleteIfExist(Resource resource);

        Task<Statistic> Get(Resource resource);
    }
}