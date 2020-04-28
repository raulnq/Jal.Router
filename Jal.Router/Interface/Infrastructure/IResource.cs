using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IResource
    {
        Task<bool> CreateIfNotExist(ResourceContext resource);

        Task<bool> DeleteIfExist(ResourceContext resource);

        Task<Statistic> Get(ResourceContext resource);
    }
}