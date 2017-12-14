namespace Jal.Router.Model.Management
{
    public class PublishSubscribeChannelInfo
    {
        public PublishSubscribeChannelInfo(string path)
        {
            Path = path;
        }

        public string Path { get; set; }

        public long MessageCount { get; set; }

        public long SizeInBytes { get; set; }

        public long ScheduledMessageCount { get; set; }

        public long DeadLetterMessageCount { get; set; }
    }
}