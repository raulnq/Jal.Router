using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class InboundMessageContext
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string Origin { get; set; }
        public IDictionary<string, string> Headers { get; set; }

        public InboundMessageContext()
        {
            Headers = new Dictionary<string, string>();
        }
    }

    public class InboundMessageContext<TContent> : InboundMessageContext
    {
        public TContent Content { get; set; }
    }
}
