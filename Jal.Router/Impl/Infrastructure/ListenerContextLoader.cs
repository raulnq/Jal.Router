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

        public ListenerContext Create(Channel channel)
        {
            var listenerchannel = _factory.CreateListenerChannel(channel.Type);

            var partition = _factory.Configuration.Runtime.Partitions.FirstOrDefault(x => x.Channel.Id == channel.Id);

            var listenercontext = new ListenerContext(channel, listenerchannel, partition);

            return listenercontext;
        }

        public void Open(ListenerContext listenercontext)
        {
            if (listenercontext.ListenerChannel != null)
            {
                listenercontext.ListenerChannel.Open(listenercontext);

                listenercontext.ListenerChannel.Listen(listenercontext);

                _logger.Log($"Listening {listenercontext.Id}");
            }
        }
    }
}