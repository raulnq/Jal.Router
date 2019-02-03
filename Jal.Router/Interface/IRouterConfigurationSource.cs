using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Interface
{
    public interface IRouterConfigurationSource
    {
        Route[] GetRoutes();

        Saga[] GetSagas();

        EndPoint[] GetEndPoints();

        Group[] GetGroups();

        SubscriptionToPublishSubscribeChannel[] GetSubscriptionsToPublishSubscribeChannel();

        PublishSubscribeChannel[] GetPublishSubscribeChannels();

        PointToPointChannel[] GetPointToPointChannels();
    }
}