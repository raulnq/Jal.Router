namespace Jal.Router.Model.Outbound
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
