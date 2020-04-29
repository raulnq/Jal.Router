namespace Jal.Router.Model
{
    public enum ChannelType
    {
        None,
        PointToPoint,
        PublishSubscribe,
        //RequestReplyToPointToPoint,
        //RequestReplyToSubscriptionToPublishSubscribe,
        SubscriptionToPublishSubscribe
    }

    public enum ReplyType
    {
        None,
        FromPointToPoint,
        FromSubscriptionToPublishSubscribe
    }
}