using System;

namespace Jal.Router.Model
{
    public class EndPoint
    {
        public EndPoint(string name)
        {
            Name = name;
        }
        public Origin Origin { get; set; }

        public string Name { get; set; }

        public Type ExtractorType { get; set; }

        public Type ConnectionStringExtractorType { get; set; }

        public Type MessageType { get; set; }

        public object ToConnectionStringExtractor { get; set; }

        public string ToPath { get; set; }

        public string ToReplyPath { get; set; }

        public int ToReplyTimeOut { get; set; }

        public string ToReplySubscription { get; set; }

        public Type ReplyConnectionStringExtractorType { get; set; }

        public object ToReplyConnectionStringExtractor { get; set; }
    }
}