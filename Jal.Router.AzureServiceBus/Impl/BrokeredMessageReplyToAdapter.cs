using Jal.Router.AzureServiceBus.Interface;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class BrokeredMessageReplyToAdapter : IBrokeredMessageReplyToAdapter
    {
        public string ReadPath(BrokeredMessage message)
        {
            if (message.Properties.ContainsKey("replytoqueue"))
            {
                return message.Properties["replytoqueue"].ToString();
            }

            return string.Empty;
        }

        public string Read(BrokeredMessage message)
        {
            if (message.Properties.ContainsKey("replyto"))
            {
                return message.Properties["replyto"].ToString();
            }

            return string.Empty;
        }

        public void WritePath(string path, BrokeredMessage message)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                message.Properties.Add("replytoqueue", path);
            }
        }

        public void Write(string replyTo, BrokeredMessage message)
        {
            if (!string.IsNullOrWhiteSpace(replyTo))
            {
                message.Properties.Add("replyto", replyTo);
            }
        }

        public string ReadConnectionString(BrokeredMessage message)
        {
            if (message.Properties.ContainsKey("replytoconnectionstring"))
            {
                return message.Properties["replytoconnectionstring"].ToString();
            }

            return string.Empty;
        }

        public void WriteConnectionString(string connectionstring, BrokeredMessage message)
        {
            if (!string.IsNullOrWhiteSpace(connectionstring))
            {
                message.Properties.Add("replytoconnectionstring", connectionstring);
            }
        }
    }
}