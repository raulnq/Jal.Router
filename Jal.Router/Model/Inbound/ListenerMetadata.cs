using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Inbound
{
    public class ListenerMetadata
    {
        public List<Action<object>> Handlers { get; set; }

        public List<string> Names { get; set; }

        public string ToConnectionString { get; set; }

        public string ToPath { get; set; }

        public string ToSubscription { get; set; }

        public string GetId()
        {
            return ToPath + ToSubscription + ToConnectionString;
        }

        public bool IsPointToPoint()
        {
            return !string.IsNullOrWhiteSpace(ToPath) && string.IsNullOrWhiteSpace(ToSubscription);
        }

        public bool IsPublishSubscriber()
        {
            return !string.IsNullOrWhiteSpace(ToPath) && !string.IsNullOrWhiteSpace(ToSubscription);
        }

        public override string ToString()
        {
            if (IsPointToPoint())
            {
                return "point to point";
            }
            if (IsPublishSubscriber())
            {
                return "publish subscriber";
            }
            return string.Empty;
        }

        public string GetPath(string prefix = "")
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

            //if (!string.IsNullOrWhiteSpace(ToReplyPath))
            //{
            //    description = $"{description}/{ToReplyPath}";
            //}

            //if (!string.IsNullOrWhiteSpace(ToReplySubscription))
            //{
            //    description = $"{description}/{ToReplySubscription}";
            //}

            return description;
        }

        public Action Shutdown { get; set; }

        public bool CanShutdown()
        {
            return Shutdown != null;
        }
        public ListenerMetadata(string topath, string toconnectionstring, string tosubscription)
        {
            ToPath = topath;
            ToConnectionString = toconnectionstring;
            ToSubscription = tosubscription;
            Handlers = new List<Action<object>>();
            Names = new List<string>();
        }
    }
}