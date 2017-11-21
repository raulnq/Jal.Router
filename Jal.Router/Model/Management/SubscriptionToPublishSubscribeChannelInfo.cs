namespace Jal.Router.Model.Management
{
    public class SubscriptionToPublishSubscribeChannelInfo
    {
        public SubscriptionToPublishSubscribeChannelInfo(string name, string path)
        {
            Name = name;
            Path = path;
        }

        public string Name { get; set; }

        public string Path { get; set; }

        public long MessageCount { get; set; }

        public long ScheduledMessageCount { get; set; }

        public long DeadLetterMessageCount { get; set; }
    }
}