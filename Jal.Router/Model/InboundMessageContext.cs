using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class InboundMessageContext
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string Origin { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Version { get; set; }
        public int RetryCount { get; set; }
        public bool LastRetry { get; set; }
        public InboundMessageContext()
        {
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
        }
    }

    public class InboundMessageContext<TContent> : InboundMessageContext
    {
        public TContent Content { get; set; }
    }
}
