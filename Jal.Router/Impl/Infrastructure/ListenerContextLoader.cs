using System;
using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ListenerContextLoader : IListenerContextLoader
    {
        private readonly IListenerContextCreator _loader;

        private readonly IComponentFactoryGateway _factory;

        public ListenerContextLoader(IListenerContextCreator loader, IComponentFactoryGateway factory)
        {
            _loader = loader;
            _factory = factory;
        }

        public void AddPointToPointChannel<TContent>(string name, string connectionstring, string path)
        {
            var channels = new List<Channel>();

            var newchannel = new Channel(ChannelType.PointToPoint, connectionstring, path);

            channels.Add(newchannel);

            var newroute = new Route(name, typeof(TContent), channels);

            var listenercontext = _loader.Create(newchannel);

            _factory.Configuration.Runtime.ListenerContexts.Add(listenercontext);

            listenercontext.Routes.Add(newroute);

            _loader.Open(listenercontext);
        }

        public void AddPublishSubscribeChannel<TContent>(string name, string connectionstring, string path, string subscription)
        {
            var channels = new List<Channel>();

            var newchannel = new Channel(ChannelType.PublishSubscribe, connectionstring, path, subscription);

            channels.Add(newchannel);

            var newroute = new Route(name, typeof(TContent), channels);

            var listenercontext = _loader.Create(newchannel);

            _factory.Configuration.Runtime.ListenerContexts.Add(listenercontext);

            listenercontext.Routes.Add(newroute);

            _loader.Open(listenercontext);
        }
    }
}