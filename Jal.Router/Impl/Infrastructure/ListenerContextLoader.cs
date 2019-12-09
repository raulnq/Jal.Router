using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ListenerContextLoader : IListenerContextLoader
    {
        private IComponentFactoryGateway _factory;

        private ILogger _logger;

        public ListenerContextLoader(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public void Load(Route route, Channel channel)
        {
            var newlistenercontext = new ListenerContext(channel);

            newlistenercontext.Routes.Add(route);

            _factory.Configuration.Runtime.ListenerContexts.Add(newlistenercontext);

            var partition = _factory.Configuration.Runtime.Partitions.FirstOrDefault(x => x.Channel.Id == channel.Id);

            if(partition!=null)
            {
                newlistenercontext.UpdatePartition(partition);
            }

            var listenerchannel = _factory.CreateListenerChannel(newlistenercontext.Channel.Type);

            if (listenerchannel != null)
            {
                newlistenercontext.UpdateListenerChannel(listenerchannel);

                listenerchannel.Open(newlistenercontext);

                listenerchannel.Listen(newlistenercontext);

                _logger.Log($"Listening {newlistenercontext.Id}");
            }
        }
    }
}