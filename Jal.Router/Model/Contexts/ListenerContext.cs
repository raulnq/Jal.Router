using Jal.Router.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Jal.Router.Model
{
    public class ListenerContext
    {
        public Channel Channel { get; private set; }

        public Partition Partition { get; private set; }

        public IListenerChannel ListenerChannel { get; private set; }

        public List<Route> Routes { get; private set; }

        public ListenerContext(Channel channel, IListenerChannel listener, Partition partition)
        {
            Channel = channel;
            Routes = new List<Route>();
            Partition = partition;
            ListenerChannel = listener;
        }

        public string Id
        {
            get
            {
                return $"{Partition?.ToString()} {Channel.FullPath} {Channel.ToString()} channel ({Routes.Count}): {string.Join(",", Routes.Select(x => x.Saga == null ? x.Name : $"{x.Saga.Name}/{x.Name}"))}";
            }
        }
    }
}