using System;

namespace Jal.Router.Model
{
    public class Channel
    {
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

        public Type ConnectionStringValueFinderType { get; set; }

        public object ToConnectionStringProvider { get; set; }

        public string ToConnectionString { get; set; }

        public string ToPath { get; set; }

        public string ToSubscription { get; set; }

        public string ToReplyPath { get; set; }

        public int ToReplyTimeOut { get; set; }

        public string ToReplySubscription { get; set; }

        public Type ReplyConnectionStringValueFinderType { get; set; }

        public object ToReplyConnectionStringProvider { get; set; }

        public string ToReplyConnectionString { get; set; }
    }
}