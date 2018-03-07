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

        private readonly IChannelPathBuilder _builder;

        public ListenerStartupTask(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, IChannelPathBuilder builder, ISagaRouter sagarouter)
        {
            _factory = factory;
            _configuration = configuration;
            _sources = sources;
            _router = router;
            _builder = builder;
            _sagarouter = sagarouter;
        }


        public void Run()
        {
            var pointtopointchannel = _factory.Create<IPointToPointChannel>(_configuration.PointToPointChannelType);

            var publishsubscriberchannel = _factory.Create<IPublishSubscribeChannel>(_configuration.PublishSubscribeChannelType);

            var routes = new List<RouteToListen>();

            foreach (var source in _sources)
            {
                routes.AddRange(source.GetRoutes().Select(x=>new RouteToListen() {Route = x}));

                foreach (var saga in source.GetSagas())
                {
                    if (saga.StartingRoute != null)
                    {
                        routes.Add(new RouteToListen() { Route = saga.StartingRoute, Saga = saga, IsStart = true });
                    }
                    if (saga.EndingRoute != null)
                    {
                        routes.Add(new RouteToListen() { Route = saga.EndingRoute, Saga = saga, IsEnd = true });
                    }
                    routes.AddRange(saga.NextRoutes.Select(x=>new RouteToListen() {Route = x, Saga = saga, IsNext = true}));
                }
            }

            var groups = routes.GroupBy(x => x.Route.Name + x.Route.ToPath + x.Route.ToSubscription + x.Route.ToConnectionString);

            foreach (var @group in groups)
            {
                var actions = new List<Action<object>>();

                foreach (var item in group)
                {
                    var route = item.Route;

                    if (item.Saga == null)
                    {
                        actions.Add(message => _router.Route(message, route));
                    }
                    else
                    {
                        var saga = item.Saga;

                        if (item.IsStart)
                        {
                            actions.Add(message => _sagarouter.Start(message, saga, route));
                        }
                        if (item.IsEnd)
                        {
                            actions.Add(message => _sagarouter.End(message, saga, route));
                        }
                        if (item.IsNext)
                        {
                            actions.Add(message => _sagarouter.Continue(message, saga, route));
                        }
                    }
                }

                if (group.Any())
                {
                    if (group.Count() == 1)
                    {
                        var item = group.First();

                        var route = item.Route;

                        var saga = item.Saga;

                        var channelpath = item.Saga == null ? _builder.BuildFromRoute(route): _builder.BuildFromSagaAndRoute(saga, route);

                        if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                        {
                            pointtopointchannel.Listen(route, actions.ToArray(), channelpath);

                            Console.WriteLine($"Listening {channelpath} point to point channel");
                        }

                        if (!string.IsNullOrWhiteSpace(route.ToPath) && !string.IsNullOrWhiteSpace(route.ToSubscription))
                        {
                            publishsubscriberchannel.Listen(route, actions.ToArray(), channelpath);

                            Console.WriteLine($"Listening {channelpath} publish subscriber channel");
                        }
                    }
                    else
                    {
                        var item = group.First();

                        var route = item.Route;

                        var channelpath = _builder.BuildFromRoute(route);

                        if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                        {
                            pointtopointchannel.Listen(route, actions.ToArray(), channelpath);

                            Console.WriteLine($"Listening {channelpath} point to point channel ({actions.Count})");
                        }

                        if (!string.IsNullOrWhiteSpace(route.ToPath) && !string.IsNullOrWhiteSpace(route.ToSubscription))
                        {
                            publishsubscriberchannel.Listen(route, actions.ToArray(), channelpath);

                            Console.WriteLine($"Listening {channelpath} publish subscriber channel ({actions.Count})");
                        }
                    }
                }
            }
        }
    }
}