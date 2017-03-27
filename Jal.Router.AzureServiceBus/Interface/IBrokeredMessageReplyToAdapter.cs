using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Interface
{
    public interface IBrokeredMessageReplyToAdapter//TODO delete
    {
        string ReadPath(BrokeredMessage message);

        void WritePath(string path, BrokeredMessage message);

        string Read(BrokeredMessage message);

        void Write(string replyTo, BrokeredMessage message);

        string ReadConnectionString(BrokeredMessage message);

        void WriteConnectionString(string connectionstring, BrokeredMessage message);
    }
}