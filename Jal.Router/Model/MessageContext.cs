using System;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model
{
    public class MessageContext
    {
        public string ToConnectionString { get; set; }
        public string ToSubscription { get; set; }
        public string ToPath { get; set; }
        public string Id { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Version { get; set; }
        public int RetryCount { get; set; }
        public bool LastRetry { get; set; }
        public Route Route { get; set; }     
        public Origin Origin { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public SagaInfo SagaInfo { get; set; }
        public Saga Saga { get; set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }
        public Type ContentType { get; set; }
        public string ContentAsString { get; set; }
        public MessageContext()
        {
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            SagaInfo = new SagaInfo();
        }

        public Dictionary<string, string> CopyHeaders()
        {
            return Headers.ToDictionary(header => header.Key, header => header.Value);
        }
    }

    public class MessageContext<TContent> : MessageContext
    {
        public TContent Content { get; set; }

        public MessageContext()
        {
            
        }

        public MessageContext(MessageContext context, TContent content)
        {
            Id = context.Id;
            Headers = context.Headers;
            Version = context.Version;
            RetryCount = context.RetryCount;
            LastRetry = context.LastRetry;
            Origin = context.Origin;
            DateTimeUtc = context.DateTimeUtc;
            Content = content;
            SagaInfo = context.SagaInfo;
            Route = context.Route;
            Saga = context.Saga;
            ContentType = context.ContentType;
            ContentAsString = context.ContentAsString;
        }
    }
}