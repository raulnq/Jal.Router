using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterConfigurationSource
    {
        Route[] GetRoutes();

        Saga[] GetSagas();

        EndPoint[] GetEndPoints();

        Subscription[] GetSubscriptions();

        Topic[] GetTopics();

        Queue[] GetQueues();
    }
}