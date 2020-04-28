using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class ResourceContext
    {
        public Resource Resource { get; private set; }

        public IResource IResource { get; private set; }

        public IMessageSerializer MessageSerializer { get; private set; }

        public ResourceContext(Resource resource, IResource manager, IMessageSerializer serializer)
        {
            Resource = resource;
            MessageSerializer = serializer;
            IResource = manager;
        }

        public Task<bool> CreateIfNotExist()
        {
            return IResource.CreateIfNotExist(this);
        }

        public Task<bool> DeleteIfExist()
        {
            return IResource.DeleteIfExist(this);
        }

        public Task<Statistic> Get()
        {
            return IResource.Get(this);
        }
    }
}