using System;
using Jal.Router.Model;

namespace Jal.Router.Interface.Inbound
{
    public interface IMessageAdapter
    {
        MessageContext ReadMetadataAndContent(object message, Type contenttype, bool useclaimcheck, Identity identityconfiguration=null);

        MessageContext ReadContent(object message, MessageContext context, Type contenttype, bool useclaimcheck, Identity identityconfiguration = null);

        MessageContext ReadMetadata(object message);

        object WriteMetadataAndContent(MessageContext context, bool useclaimcheck);

        object Deserialize(string content, Type contenttype);

        TContent Deserialize<TContent>(string content);

        string Serialize(object content);
    }
}