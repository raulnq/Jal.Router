using System;

namespace Jal.Router.Model
{
    public class Channel
    {
        public bool IsPointToPoint()
        {
            return !string.IsNullOrWhiteSpace(ToPath) && string.IsNullOrWhiteSpace(ToSubscription);
        }

        public bool IsPublishSubscriber()
        {
            return !string.IsNullOrWhiteSpace(ToPath) && !string.IsNullOrWhiteSpace(ToSubscription);
        }

        public string GetId()
        {
            return ToPath + ToSubscription + ToConnectionString;
        }

        public bool IsValidEndpoint()
        {
            return !string.IsNullOrWhiteSpace(ToConnectionString) && !string.IsNullOrWhiteSpace(ToPath);
        }

        public bool IsValidReplyEndpoint()
        {
            return IsValidEndpoint() && !string.IsNullOrWhiteSpace(ToReplyConnectionString) && !string.IsNullOrWhiteSpace(ToReplyPath);
        }

        public bool IsActive()
        {
            return Shutdown != null;
        }

        public override string ToString()
        {
            if(IsPointToPoint())
            {
                return "point to point";
            }
            if(IsPublishSubscriber())
            {
                return "publish subscriber";
            }
            return string.Empty;
        }

        public string GetPath(string prefix="")
        {
            var description = string.Empty;

            if (!string.IsNullOrWhiteSpace(prefix))
            {
                description = $"{description}/{prefix}";
            }

            if (!string.IsNullOrWhiteSpace(ToPath))
            {
                description = $"{description}/{ToPath}";
            }

            if (!string.IsNullOrWhiteSpace(ToSubscription))
            {
                description = $"{description}/{ToSubscription}";
            }

            if (!string.IsNullOrWhiteSpace(ToReplyPath))
            {
                description = $"{description}/{ToReplyPath}";
            }

            if (!string.IsNullOrWhiteSpace(ToReplySubscription))
            {
                description = $"{description}/{ToReplySubscription}";
            }

            return description;
        }

        public Type ConnectionStringExtractorType { get; set; }

        public object ToConnectionStringExtractor { get; set; }

        public string ToConnectionString { get; set; }

        public string ToPath { get; set; }

        public string ToSubscription { get; set; }

        public Action Shutdown { get; set; }

        public string ToReplyPath { get; set; }

        public int ToReplyTimeOut { get; set; }

        public string ToReplySubscription { get; set; }

        public Type ReplyConnectionStringExtractorType { get; set; }

        public object ToReplyConnectionStringExtractor { get; set; }

        public string ToReplyConnectionString { get; set; }
    }
}