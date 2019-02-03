using System;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl
{

    public class NullMessageAdapter : IMessageAdapter
    {
        public MessageContext ReadMetadataAndContent(object message, Type type, bool useclaimcheck, Identity identityconfiguration)
        {
            return new MessageContext(new EndPoint(string.Empty), new Options());
        }

        public object WriteMetadataAndContent(MessageContext context, bool useclaimcheck)
        {
            return null;
        }

        public object Deserialize(string content, Type type)
        {
            return null;
        }

        public TContent Deserialize<TContent>(string content)
        {
            return default(TContent);
        }

        public string Serialize(object content)
        {
            return string.Empty;
        }

        public MessageContext ReadContent(object message, MessageContext context, Type contenttype, bool useclaimcheck, Identity identityconfiguration = null)
        {
            return context;
        }

        public MessageContext ReadMetadata(object message)
        {
            return new MessageContext(new EndPoint(string.Empty), new Options());
        }
    }
}