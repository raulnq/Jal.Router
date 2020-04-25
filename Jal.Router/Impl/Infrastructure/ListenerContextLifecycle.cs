using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ListenerContextLifecycle : IListenerContextLifecycle
    {
        private IComponentFactoryFacade _factory;

        public ListenerContextLifecycle(IComponentFactoryFacade factory)
        {
            _factory = factory;
        }

        public ListenerContext AddOrGet(Channel channel)
        {
            var listenercontext = Get(channel);

            if (listenercontext == null)
            {
                listenercontext = Add(channel);
            }

            return listenercontext;
        }

        public ListenerContext Add(Channel channel)
        {
            var listenerchannel = _factory.CreateListenerChannel(channel.ChannelType, channel.Type);

            var adapter = _factory.CreateMessageAdapter(channel.AdapterType);

            var partition = _factory.Configuration.Runtime.Partitions.FirstOrDefault(x => x.Channel.Id == channel.Id);

            var listenercontext = new ListenerContext(channel, listenerchannel, adapter,  partition);

            _factory.Configuration.Runtime.ListenerContexts.Add(listenercontext);

            return listenercontext;
        }

        public ListenerContext Get(Channel channel)
        {
            return _factory.Configuration.Runtime.ListenerContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);
        }

        public bool Exist(Channel channel)
        {
            return _factory.Configuration.Runtime.ListenerContexts.Any(x => x.Channel.Id == channel.Id);
        }

        public ListenerContext Remove(Channel channel)
        {
            var listenercontext = Get(channel);

            if (listenercontext != null)
            {
                _factory.Configuration.Runtime.ListenerContexts.Remove(listenercontext);
            }

            return listenercontext;
        }
    }
}