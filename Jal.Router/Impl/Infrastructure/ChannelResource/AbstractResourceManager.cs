using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public abstract class AbstractResourceManager : IResourceManager
    {
        public virtual Task<bool> CreateIfNotExist(Resource resource)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> DeleteIfExist(Resource resource)
        {
            return Task.FromResult(false);
        }

        public virtual Task<Statistic> Get(Resource resource)
        {
            return Task.FromResult(default(Statistic));
        }
    }
}