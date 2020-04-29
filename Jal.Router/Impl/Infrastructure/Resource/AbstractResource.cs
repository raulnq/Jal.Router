using Jal.Router.Interface;
using Jal.Router.Model;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public abstract class AbstractResource : IResource
    {
        public virtual Task<bool> CreateIfNotExist(ResourceContext context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> DeleteIfExist(ResourceContext context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<Statistic> Get(ResourceContext context)
        {
            return Task.FromResult(default(Statistic));
        }
    }
}