using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterConfigurationSource
    {
        Route[] GetRoutes();

        Saga[] GetSagas();

        EndPoint[] GetEndPoints();

        Partition[] GetPartitions();

        SubscriptionToPublishSubscribeChannelResource[] GetSubscriptionsToPublishSubscribeChannelResources();

        PublishSubscribeChannelResource[] GetPublishSubscribeChannelResources();

        PointToPointChannelResource[] GetPointToPointChannelResources();
    }
}