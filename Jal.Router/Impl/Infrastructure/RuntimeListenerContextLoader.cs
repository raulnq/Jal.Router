using System;
using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RuntimeListenerContextLoader : IRuntimeListenerContextLoader
    {
        private readonly IListenerContextLoader _loader;

        private IComponentFactoryGateway _factory;

        public RuntimeListenerContextLoader(IListenerContextLoader loader, IComponentFactoryGateway factory)
        {
            _loader = loader;
            _factory = factory;
        }

        public void AddPointToPointChannel<TContent, THandler, TConcreteConsumer>(string name, string connectionstring, string path)
        {
            var channels = new List<Channel>();

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PointToPoint, typeof(NullValueFinder), provider, path);

            channels.Add(newchannel);

            var newroute = new Route<TContent, THandler>(name, typeof(TConcreteConsumer), channels);

            var listenercontext = _loader.Create(newchannel);

            _factory.Configuration.Runtime.ListenerContexts.Add(listenercontext);

            listenercontext.Routes.Add(newroute);

            _loader.Open(listenercontext);
        }

        public void AddPublishSubscribeChannel<TContent, THandler, TConcreteConsumer>(string name, string connectionstring, string path, string subscription)
        {
            var channels = new List<Channel>();

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PublishSubscribe, typeof(NullValueFinder), provider, path, subscription);

            channels.Add(newchannel);

            var newroute = new Route<TContent, THandler>(name, typeof(TConcreteConsumer), channels);

            var listenercontext = _loader.Create(newchannel);

            _factory.Configuration.Runtime.ListenerContexts.Add(listenercontext);

            listenercontext.Routes.Add(newroute);

            _loader.Open(listenercontext);
        }
    }
}