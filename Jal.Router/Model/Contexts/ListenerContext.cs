using Jal.Router.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model
{
    public class ListenerContext
    {
        public Channel Channel { get; private set; }

        public Group Group { get; private set; }

        public IListenerChannel ListenerChannel { get; private set; }

        public List<Route> Routes { get; private set; }

        public ListenerContext(Channel channel)
        {
            Channel = channel;
            Routes = new List<Route>();
        }

        public string Id
        {
            get
            {
                return $"{Group?.ToString()} {Channel.FullPath} {Channel.ToString()} channel ({Routes.Count}): {string.Join(",", Routes.Select(x => x.Saga == null ? x.Name : $"{x.Saga.Name}/{x.Name}"))}";
            }
        }

        public void UpdateGroup(Group group)
        {
            Group = group;
        }

        public void UpdateListenerChannel(IListenerChannel listener)
        {
            ListenerChannel = listener;
        }
    }
}