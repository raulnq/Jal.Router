using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Inbound
{
    public class ListenerMetadata
    {
        public List<Action<object>> Handlers { get; set; }

        public List<string> Names { get; set; }

        public ChannelType Type { get; set; }

        public string ToConnectionString { get; set; }

        public string ToPath { get; set; }

        public string ToSubscription { get; set; }

        public string GetId()
        {
            return ToPath + ToSubscription + ToConnectionString;
        }

        public override string ToString()
        {
            if (Type == ChannelType.PointToPoint)
            {
                return "point to point";
            }
            if (Type == ChannelType.PublishSubscriber)
            {
                return "publish subscriber";
            }
            return string.Empty;
        }

        public string GetPath()
        {
            var description = string.Empty;

            if (!string.IsNullOrWhiteSpace(ToPath))
            {
                description = $"{description}/{ToPath}";
            }

            if (!string.IsNullOrWhiteSpace(ToSubscription))
            {
                description = $"{description}/{ToSubscription}";
            }

            return description;
        }

        public Func<object[]> CreateListenerMethod { get; set; }

        public Action<object[]> DestroyListenerMethod { get; set; }

        public Action<object[]> ListenMethod { get; set; }

        public List<Route> Routes { get; set; }

        public object[] Listener { get; set; }

        public ListenerMetadata(string topath, string toconnectionstring, string tosubscription, ChannelType channeltype)
        {
            ToPath = topath;
            ToConnectionString = toconnectionstring;
            ToSubscription = tosubscription;
            Handlers = new List<Action<object>>();
            Names = new List<string>();
            Type = channeltype;
            Routes = new List<Route>();
        }
    }
}