using System;

namespace Jal.Router.Model
{
    public class Partition
    {
        public Func<MessageContext, bool> Until { get; private set; }

        public Channel Channel { get; private set; }

        public string Name { get; private set; }

        public Partition(string name)
        {
            Name = name;

            Until = x => false;
        }

        public override string ToString()
        {
            return "by partition: " + Name;
        }

        public void UpdateChannel(Channel channel)
        {
            Channel = channel;
        }

        public void UpdateUntil(Func<MessageContext, bool> until)
        {
            Until = until;
        }
    }
}