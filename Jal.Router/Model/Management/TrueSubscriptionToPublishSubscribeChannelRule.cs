namespace Jal.Router.Model.Management
{
    public class TrueSubscriptionToPublishSubscribeChannelRule : SubscriptionToPublishSubscribeChannelRule
    {
        public TrueSubscriptionToPublishSubscribeChannelRule()
        {
            Name = "$Default";
            Filter = "1=1";
            IsDefault = false;
        }
    }

}