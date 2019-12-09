using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RuntimeSenderContextLoader : IRuntimeSenderContextLoader
    {
        private readonly ISenderContextLoader _loader;

        public RuntimeSenderContextLoader(ISenderContextLoader loader)
        {
            _loader = loader;
        }

        public void AddPointToPointChannel<TMessage>(string name, string connectionstring, string path)
        {
            var newendpoint = new EndPoint(name);

            newendpoint.UpdateContentType(typeof(TMessage));

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PointToPoint, typeof(NullValueFinder), provider, path);

            newendpoint.Channels.Add(newchannel);

            _loader.Load(newendpoint, newchannel);
        }

        public void AddPublishSubscribeChannel<TMessage>(string name, string connectionstring, string path)
        {
            var newendpoint = new EndPoint(name);

            newendpoint.UpdateContentType(typeof(TMessage));

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PublishSubscribe, typeof(NullValueFinder), provider, path);

            newendpoint.Channels.Add(newchannel);

            _loader.Load(newendpoint, newchannel);
        }
    }
}