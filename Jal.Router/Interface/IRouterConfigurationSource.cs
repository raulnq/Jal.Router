using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterConfigurationSource
    {
        Route[] GetRoutes();

        EndPoint[] GetEndPoints();
    }
}