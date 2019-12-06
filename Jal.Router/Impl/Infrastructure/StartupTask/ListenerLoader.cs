using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public interface IDynamicListenerLoader
    {
        void AddPointToPointChannel<TContent, THandler, TConcreteConsumer>(string name, string connectionstring, string path);

        void AddPublishSubscribeChannel<TContent, THandler, TConcreteConsumer>(string name, string connectionstring, string path, string subscription);
    }

    public class DynamicListenerLoader
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        void AddPointToPointChannel<TContent, THandler, TConcreteConsumer>(string name, string connectionstring, string path)
        {
            var channels = new List<Channel>();

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PointToPoint, typeof(NullValueFinder), provider, path);

            channels.Add(newchannel);

            var newroute = new Route<TContent, THandler>(name, typeof(TConcreteConsumer), channels);

            CreateOpenAndListen(newchannel, newroute);
        }

        void AddPublishSubscribeChannel<TContent, THandler, TConcreteConsumer>(string name, string connectionstring, string path, string subscription)
        {
            var channels = new List<Channel>();

            Func<IValueFinder, string> provider = x => connectionstring;

            var newchannel = new Channel(ChannelType.PublishSubscribe, typeof(NullValueFinder), provider, path, subscription);

            channels.Add(newchannel);

            var newroute = new Route<TContent, THandler>(name, typeof(TConcreteConsumer), channels);

            CreateOpenAndListen(newchannel, newroute);
        }

        private void CreateOpenAndListen<TContent, THandler>(Channel newchannel, Route<TContent, THandler> newroute)
        {
            var listener = _factory.Configuration.Runtime.ListenerContexts.FirstOrDefault(x => x.Channel.Id == newchannel.Id);

            if (listener != null)
            {
                listener.Routes.Add(newroute);
            }
            else
            {
                var newlistener = new ListenerContext(newchannel);

                newlistener.Routes.Add(newroute);

                _factory.Configuration.Runtime.ListenerContexts.Add(newlistener);

                var partition = _factory.Configuration.Runtime.Partitions.FirstOrDefault(x => x.Channel.Id == newlistener.Channel.Id);

                if (newlistener != null)
                {
                    newlistener.UpdatePartition(partition);
                }

                var listenerchannel = _factory.CreateListenerChannel(newlistener.Channel.Type);

                if (listenerchannel != null)
                {
                    newlistener.UpdateListenerChannel(listenerchannel);

                    listenerchannel.Open(newlistener);

                    listenerchannel.Listen(newlistener);

                    _logger.Log($"Listening {newlistener.Id}");
                }
            }
        }
    }
    public class ListenerLoader : AbstractStartupTask, IStartupTask
    {
        public ListenerLoader(IComponentFactoryGateway factory,  IRouterConfigurationSource[] sources, ILogger logger)
            :base(factory, logger)
        {

        }

        public Task Run()
        {
            Logger.Log("Loading listeners");

            Create();

            Update();

            OpenAndListen();

            Logger.Log("Listeners loaded");

            return Task.CompletedTask;
        }

        private void OpenAndListen()
        {
            foreach (var listenercontext in Factory.Configuration.Runtime.ListenerContexts)
            {
                var listenerchannel = Factory.CreateListenerChannel(listenercontext.Channel.Type);

                if(listenerchannel!=null)
                {
                    listenercontext.UpdateListenerChannel(listenerchannel);

                    listenerchannel.Open(listenercontext);

                    listenerchannel.Listen(listenercontext);

                    Logger.Log($"Listening {listenercontext.Id}");
                }
            }
        }

        private void Update()
        {
            foreach (var partition in Factory.Configuration.Runtime.Partitions)
            {
                var listener = Factory.Configuration.Runtime.ListenerContexts.FirstOrDefault(x => x.Channel.Id == partition.Channel.Id);

                if (listener != null)
                {
                    listener.UpdatePartition(partition);
                }
            }
        }

        private void Create()
        {
            foreach (var item in Factory.Configuration.Runtime.Routes)
            {
                foreach (var channel in item.Channels)
                {
                    var listener = Factory.Configuration.Runtime.ListenerContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);

                    if (listener != null)
                    {
                        listener.Routes.Add(item);
                    }
                    else
                    {
                        var newlistener = new ListenerContext(channel);

                        newlistener.Routes.Add(item);

                        Factory.Configuration.Runtime.ListenerContexts.Add(newlistener);
                    }
                }
            }
        }
    }
}