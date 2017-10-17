using System;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Model.Outbount
{

    public class OutboundMessageContext<TContent> : OutboundMessageContext
    {
        public TContent Content { get; set; }
    }


    public class OutboundMessageContext : MessageContext
    {
        public string ToConnectionString { get; set; }
        public string ToPath { get; set; }
        
    }
}
