using Jal.Router.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model.Inbound
{
    public class ListenerMetadata
    {
        public Channel Channel { get; }

        public Group Group { get; set; }

        public IListenerChannel Listener { get; set; }

        public List<Route> Routes { get;  }

        public ListenerMetadata(Channel channel)
        {
            Channel = channel;
            Routes = new List<Route>();
        }

        public string Signature()
        {
            return $"{Group?.ToString()} {Channel.GetPath()} {Channel.ToString()} channel ({Routes.Count}): {string.Join(",", Routes.Select(x => x.Saga == null ? x.Name : $"{x.Saga.Name}/{x.Name}"))}";
        }
    }
}