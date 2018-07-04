using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class ListenerStartupTask : IStartupTask
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly IRouterConfigurationSource[] _sources;

        private readonly IRouter _router;

        private readonly ISagaRouter _sagarouter;

        private readonly ILogger _logger;

        public ListenerStartupTask(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, ILogger logger, ISagaRouter sagarouter)
        {
            _factory = factory;
            _configuration = configuration;
            _sources = sources;
            _router = router;
            _logger = logger;
            _sagarouter = sagarouter;
        }


        public void Run()
        {
            var pointtopointchannel = _factory.Create<IPointToPointChannel>(_configuration.PointToPointChannelType);

            var publishsubscriberchannel = _factory.Create<IPublishSubscribeChannel>(_configuration.PublishSubscribeChannelType);

            var listeners = new List<Listener>();

            foreach (var source in _sources)
            {
                listeners.AddRange(source.GetRoutes().Select(route => new Listener() { Route = route, Action = message => _router.Route(message, route), Prefix = route.Name }));

                foreach (var saga in source.GetSagas())
                {
                    if (saga.StartingRoute != null)
                    {
                        listeners.Add(new Listener() { Route = saga.StartingRoute, Action = message => _sagarouter.Start(message, saga, saga.StartingRoute), Prefix = $"{saga.Name}/{saga.StartingRoute.Name}" });
                    }
                    if (saga.EndingRoute != null)
                    {
                        listeners.Add(new Listener() { Route = saga.EndingRoute, Action = message => _sagarouter.End(message, saga, saga.EndingRoute), Prefix = $"{saga.Name}/{saga.EndingRoute.Name}" });
                    }
                    listeners.AddRange(saga.NextRoutes.Select(route => new Listener() { Route = route, Action = message => _sagarouter.Continue(message, saga, route), Prefix = $"{saga.Name}/{route.Name}" }));
                }
            }

            var groupsbychannel = new Dictionary<string, List<Listener>>();

            foreach (var listener in listeners)
            {
                foreach (var channel in listener.Route.Channels)
                {
                    if (!groupsbychannel.ContainsKey(channel.GetId()))
                    {
                        groupsbychannel.Add(channel.GetId(), new List<Listener>() { listener });
                    }
                    else
                    {
                        groupsbychannel[channel.GetId()].Add(listener);
                    }
                }
            }

            foreach (var groupbychannel in groupsbychannel)
            {
                if (groupbychannel.Value.Any())
                {
                    var listener = groupbychannel.Value.First();

                    var channel = listener.Route.Channels.First(x => groupbychannel.Key == x.GetId());

                    var channelpath = channel.GetPath();

                    var prefixes = string.Join(",", groupbychannel.Value.Select(x => x.Prefix));

                    var actions = groupbychannel.Value.Select(x => x.Action).ToArray();

                    if (channel.IsPointToPoint())
                    {
                        pointtopointchannel.Listen(channel, actions, channelpath);

                        _logger.Log($"Listening {channelpath} {channel.ToString()} channel ({actions.Length}): {prefixes}");
                    }

                    if (channel.IsPublishSubscriber())
                    {
                        publishsubscriberchannel.Listen(channel, actions, channelpath);

                        _logger.Log($"Listening {channelpath} {channel.ToString()} channel ({actions.Length}): {prefixes}");
                    }
                }
            }
        }
    }
}