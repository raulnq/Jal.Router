using Jal.Router.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class ListenerContext
    {
        public Channel Channel { get; private set; }

        public Partition Partition { get; private set; }

        public IListenerChannel ListenerChannel { get; private set; }

        public IMessageAdapter MessageAdapter { get; private set; }

        public List<Route> Routes { get; private set; }

        public ListenerContext(Channel channel, IListenerChannel listener, IMessageAdapter adapter, Partition partition)
        {
            Channel = channel;
            Routes = new List<Route>();
            Partition = partition;
            ListenerChannel = listener;
            MessageAdapter = adapter;
        }

        public async Task<bool> Close()
        {
            if (ListenerChannel != null)
            {
                await ListenerChannel.Close(this).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public bool IsActive()
        {
            if (ListenerChannel != null)
            {
                return ListenerChannel.IsActive(this);
            }

            return false;
        }

        public bool Open()
        {
            if (ListenerChannel != null)
            {
                ListenerChannel.Open(this);

                ListenerChannel.Listen(this);

                return true;
            }

            return false;
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