using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RuntimeSenderContextLoader : IRuntimeSenderContextLoader
    {
        private readonly ISenderContextLoader _loader;

        private IComponentFactoryGateway _factory;

        public RuntimeSenderContextLoader(ISenderContextLoader loader, IComponentFactoryGateway factory)
        {
            _loader = loader;
            _factory = factory;
        }

        public void AddPointToPointChannel<TMessage>(string name, string connectionstring, string path)
        {
            var newendpoint = new EndPoint(name);

            newendpoint.UpdateContentType(typeof(TMessage));

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PointToPoint, typeof(NullValueFinder), provider, path);

            newendpoint.Channels.Add(newchannel);

            var sendercontext = _loader.Create(newchannel);

            _factory.Configuration.Runtime.SenderContexts.Add(sendercontext);

            sendercontext.Endpoints.Add(newendpoint);

            _loader.Open(sendercontext);
        }

        public void AddPublishSubscribeChannel<TMessage>(string name, string connectionstring, string path)
        {
            var newendpoint = new EndPoint(name);

            newendpoint.UpdateContentType(typeof(TMessage));

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PublishSubscribe, typeof(NullValueFinder), provider, path);

            newendpoint.Channels.Add(newchannel);

            var sendercontext = _loader.Create(newchannel);

            _factory.Configuration.Runtime.SenderContexts.Add(sendercontext);

            sendercontext.Endpoints.Add(newendpoint);

            _loader.Open(sendercontext);
        }
    }
}