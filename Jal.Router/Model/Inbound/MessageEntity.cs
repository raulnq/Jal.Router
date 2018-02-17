using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Inbound
{
    public class MessageEntity
    {
        public string Data { get; set; }
        public IList<string> ParentIds { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public string Content { get; set; }
        public string Id { get; set; }
        public string Version { get; set; }
        public int RetryCount { get; set; }
        public bool LastRetry { get; set; }
        public Origin Origin { get; set; }
        public SagaInfo Saga { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
    }
}
