using Jal.Router.Interface;
using Jal.Router.Model;
using System.Linq;

namespace Jal.Router.Impl
{
    public class ResourceContextLifecycle : IResourceContextLifecycle
    {
        private IComponentFactoryFacade _factory;

        public ResourceContextLifecycle(IComponentFactoryFacade factory)
        {
            _factory = factory;
        }

        public ResourceContext Add(Resource resource)
        {
            var manager = _factory.CreateResource(resource.ChannelType/*, resource.Type*/);

            var serializer = _factory.CreateMessageSerializer();

            var resourcecontext = new ResourceContext(resource, manager, serializer);

            _factory.Configuration.Runtime.ResourceContexts.Add(resourcecontext);

            return resourcecontext;
        }

        public ResourceContext Get(Resource resource)
        {
            return _factory.Configuration.Runtime.ResourceContexts.FirstOrDefault(x => x.Resource.Id == resource.Id);
        }

        public bool Exist(Resource channel)
        {
            return _factory.Configuration.Runtime.ResourceContexts.Any(x => x.Resource.Id == channel.Id);
        }

        public ResourceContext Remove(Resource resource)
        {
            var resourcecontext = Get(resource);

            if (resourcecontext == null)
            {
                _factory.Configuration.Runtime.ResourceContexts.Remove(resourcecontext);
            }

            return resourcecontext;
        }
    }
}