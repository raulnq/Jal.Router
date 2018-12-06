using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class MessageEntity
    {
        public string Id { get; set; }
        public string Data { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public string Content { get; set; }
        public string ContentId { get; set; }
        public string Version { get; set; }
        public int RetryCount { get; set; }
        public bool LastRetry { get; set; }
        public Origin Origin { get; set; }
        public SagaContext Saga { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public List<Track> Tracks { get; set; }
        public Identity Identity { get; set; }
        public MessageEntityType Type { get; set; }

        public MessageEntity()
        {
            Origin = new Origin();
            Saga = new SagaContext();
            Tracks = new List<Track>();
            Identity = new Identity();
        }
    }
}
