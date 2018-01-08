using System;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class NullMessageAdapter : IMessageAdapter
    {
        public MessageContext Read(object message, Type contenttype)
        {
            return new MessageContext();
        }

        public string GetBody(object message)
        {
            return string.Empty;
        }

        public object Write(MessageContext context)
        {
            return null;
        }
    }
}