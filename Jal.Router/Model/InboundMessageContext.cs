using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class InboundMessageContext
    {
        public string Id { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Version { get; set; }
        public int RetryCount { get; set; }
        public bool LastRetry { get; set; }
        public Origin Origin { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public SagaSetting Saga { get; set; }
        public InboundMessageContext()
        {
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            Saga = new SagaSetting();
        }
    }

    public class InboundMessageContext<TContent> : InboundMessageContext
    {
        public TContent Content { get; set; }

        public InboundMessageContext(InboundMessageContext context, TContent content)
        {
            Id = context.Id;
            Headers = context.Headers;
            Version = context.Version;
            RetryCount = context.RetryCount;
            LastRetry = context.LastRetry;
            Origin = context.Origin;
            DateTimeUtc = context.DateTimeUtc;
            Content = content;
            Saga = context.Saga;

        }
    }
}
