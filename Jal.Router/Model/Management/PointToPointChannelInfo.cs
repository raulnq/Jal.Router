namespace Jal.Router.Model.Management
{
    public class PointToPointChannelInfo
    {
        public PointToPointChannelInfo(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public long MessageCount { get; set; }

        public long SizeInBytes { get; set; }

        public long ScheduledMessageCount { get; set; }

        public long DeadLetterMessageCount { get; set; }
    }
}