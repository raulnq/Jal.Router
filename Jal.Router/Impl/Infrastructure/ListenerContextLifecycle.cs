using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ListenerContextLifecycle : IListenerContextLifecycle
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly IRouter _router;

        public ListenerContextLifecycle(IComponentFactoryFacade factory, IRouter router)
        {
            _factory = factory;
            _router = router;
        }

        public ListenerContext Add(Route route, Channel channel)
        {
            var listenerchannel = _factory.CreateListenerChannel(channel.ChannelType, channel.Type);

            var adapter = _factory.CreateMessageAdapter(channel.AdapterType);

            var serializer = _factory.CreateMessageSerializer();

            var storage = _factory.CreateMessageStorage();

            var listenercontext = new ListenerContext(route, channel, listenerchannel, adapter, _router, serializer, storage);

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