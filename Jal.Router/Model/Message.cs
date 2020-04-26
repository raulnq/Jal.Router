using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Message
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string From { get; set; }

        public string Version { get; set; }

        public string SagaId { get; set; }

        public string ClaimCheckId { get; set; }

        public List<Tracking> Trackings { get; set; }

        public DateTime ScheduledEnqueueTimeUtc { get; set; }

        public string Origin { get; set; }

        public string ReplyToRequestId { get; set; }

        public string RequestId { get; set; }

        public string PartitionId { get; set; }

        public string OperationId { get; set; }

        public string ParentId { get; set; }

        public string ContentType { get; set; }

        public Message()
        {
            Headers = new Dictionary<string, string>();
        }
    }
}