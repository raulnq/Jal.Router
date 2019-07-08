namespace Jal.Router.Model
{
    public class ChannelEntity
    {
        public string Path { get; }

        public string Subscription { get; }

        public ChannelType Type { get; }

        public ChannelEntity()
        {

        }

        public ChannelEntity(string path, string subscription, ChannelType type)
        {
            Path = path;
            Subscription = subscription;
            Type = type;
        }
    }
}
