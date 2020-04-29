using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IResourceContextLifecycle
    {
        ResourceContext Remove(Resource resource);

        ResourceContext Add(Resource resource);

        ResourceContext Get(Resource resource);

        bool Exist(Resource channel);
    }
}