using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ListenerContextLifecycle : IListenerContextLifecycle
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly IRouter _router;

        private readonly ILogger _logger;

        private readonly IHasher _hasher;

        public ListenerContextLifecycle(IComponentFactoryFacade factory, IRouter router, ILogger logger, IHasher hasher)
        {
            _factory = factory;
            _router = router;
            _logger = logger;
            _hasher = hasher;
        }

        public ListenerContext Add(Route route, Channel channel)
        {
            var context = ListenerContext.Create(_factory, _router, _logger, _hasher, channel, route);

            _factory.Configuration.Runtime.ListenerContexts.Add(context);

            return context;
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