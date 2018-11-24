namespace Jal.Router.Model.Management
{
    public class PointToPointChannelInfo
    {
        public PointToPointChannelInfo(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public long MessageCount { get; set; }

        public long SizeInBytes { get; set; }

        public long ScheduledMessageCount { get; set; }

        public long DeadLetterMessageCount { get; set; }
    }
}