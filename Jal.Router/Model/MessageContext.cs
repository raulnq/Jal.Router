using System;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model
{
    public class MessageContext
    {
        public string Id { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Version { get; set; }
        public int RetryCount { get; set; }
        public bool LastRetry { get; set; }
        public Origin Origin { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public SagaSetting Saga { get; set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }
        public MessageContext()
        {
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            Saga = new SagaSetting();
        }

        public Dictionary<string, string> CopyHeaders()
        {
            return Headers.ToDictionary(header => header.Key, header => header.Value);
        }
    }
}