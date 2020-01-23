using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterConfigurationSource
    {
        Route[] GetRoutes();

        Saga[] GetSagas();

        EndPoint[] GetEndPoints();

        Partition[] GetPartitions();

        SubscriptionToPublishSubscribeChannelResource[] GetSubscriptionsToPublishSubscribeChannelResource();

        PublishSubscribeChannelResource[] GetPublishSubscribeChannelResources();

        PointToPointChannelResource[] GetPointToPointChannelResources();
    }
}