using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class ResourceContext
    {
        public Resource Resource { get; private set; }

        public IResourceManager ResourceManager { get; private set; }

        public ResourceContext(Resource resource, IResourceManager manager)
        {
            Resource = resource;

            ResourceManager = manager;
        }

        public Task<bool> CreateIfNotExist()
        {
            return ResourceManager.CreateIfNotExist(Resource);
        }

        public Task<bool> DeleteIfExist()
        {
            return ResourceManager.DeleteIfExist(Resource);
        }

        public Task<Statistic> Get()
        {
            return ResourceManager.Get(Resource);
        }
    }
}