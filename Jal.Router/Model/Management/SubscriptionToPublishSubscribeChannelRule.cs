namespace Jal.Router.Model.Management
{
    public class SubscriptionToPublishSubscribeChannelRule
    {
        public static SubscriptionToPublishSubscribeChannelRule True = new TrueSubscriptionToPublishSubscribeChannelRule();
        public string Filter { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }

}