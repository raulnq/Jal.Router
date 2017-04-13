using System;

namespace Jal.Router.Model
{

    public class OutboundMessageContext<TContent> : OutboundMessageContext
    {
        public TContent Content { get; set; }
    }


    public class OutboundMessageContext : InboundMessageContext
    {
        public string ToConnectionString { get; set; }
        public string ToPath { get; set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }
    }
}
