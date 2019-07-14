namespace Jal.Router.Model
{
    public class ChannelEntity
    {
        public string Path { get; private set; }

        public string Subscription { get; private set; }

        public ChannelType Type { get; private set; }

        private ChannelEntity()
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
