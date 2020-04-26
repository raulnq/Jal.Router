using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class ListenerContext
    {
        public Channel Channel { get; private set; }

        public IListenerChannel ListenerChannel { get; private set; }

        public IMessageAdapter MessageAdapter { get; private set; }

        public IMessageSerializer MessageSerializer { get; private set; }

        public IMessageStorage MessageStorage { get; private set; }

        public IRouter Router { get; private set; }

        public Route Route { get; private set; }

        public ListenerContext(Route route, Channel channel, IListenerChannel listener, IMessageAdapter adapter, IRouter router, IMessageSerializer serializer, IMessageStorage storage)
        {
            Channel = channel;
            Route = route;
            ListenerChannel = listener;
            MessageAdapter = adapter;
            Router = router;
            MessageSerializer = serializer;
            MessageStorage = storage;
        }

        public Task Consume(MessageContext context)
        {
            var when = true;

            if(Channel.Condition!=null)
            {
                when = Channel.Condition(context);
            }

            if(when)
            {
                return Router.Route(context);
            }
            else
            {
                return Task.CompletedTask;
            }
        }

        public Task<MessageContext> Read(object message)
        {
            return MessageAdapter.ReadFromPhysicalMessage(message, this);
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

        public override string ToString()
        {
            return $"{Channel.FullPath} {Channel.ToString()} channel: {Route.ToString()} partition: {Channel.Partition}";
        }
    }
}