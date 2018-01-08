using System;
using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class ListenerStartupTask : IStartupTask
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly IRouterConfigurationSource[] _sources;

        private readonly IRouter _router;

        private readonly IChannelPathBuilder _builder;

        public ListenerStartupTask(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources, IRouter router, IChannelPathBuilder builder)
        {
            _factory = factory;
            _configuration = configuration;
            _sources = sources;
            _router = router;
            _builder = builder;
        }


        public void Run()
        {
            var pointtopointchannel = _factory.Create<IPointToPointChannel>(_configuration.PointToPointChannelType);

            var publishsubscriberchannel = _factory.Create<IPublishSubscribeChannel>(_configuration.PublishSubscribeChannelType);

            var routes = new List<Route>();

            var sagas = new List<Saga>();

            foreach (var source in _sources)
            {
                routes.AddRange(source.GetRoutes());

                sagas.AddRange(source.GetSagas());
            }

            foreach (var saga in sagas)
            {
                if (saga.StartingRoute != null)
                {
                    var route = saga.StartingRoute;

                    if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                    {
                        var channelpath = _builder.BuildFromSagaAndRoute(saga, route);

                        pointtopointchannel.Listen(route, message => _router.RouteToStartingSaga(message, saga, route), channelpath);

                        Console.WriteLine($"Listening {channelpath} point to point channel");
                    }

                    if (!string.IsNullOrWhiteSpace(route.ToPath) && !string.IsNullOrWhiteSpace(route.ToSubscription))
                    {
                        var channelpath = _builder.BuildFromSagaAndRoute(saga, route);

                        publishsubscriberchannel.Listen(route, message => _router.RouteToStartingSaga(message, saga, route), channelpath);

                        Console.WriteLine($"Listening {channelpath} publish subscriber channel");
                    }
                }

                foreach (var route in saga.NextRoutes)
                {
                    if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                    {
                        var channelpath = _builder.BuildFromSagaAndRoute(saga, route);

                        pointtopointchannel.Listen(route, message => _router.RouteToContinueSaga(message, saga, route), channelpath);

                        Console.WriteLine($"Listening {channelpath} point to point channel");
                    }

                    if (!string.IsNullOrWhiteSpace(route.ToPath) && !string.IsNullOrWhiteSpace(route.ToSubscription))
                    {
                        var channelpath = _builder.BuildFromSagaAndRoute(saga, route);

                        publishsubscriberchannel.Listen(route, message => _router.RouteToContinueSaga(message, saga, route), channelpath);

                        Console.WriteLine($"Listening {channelpath} publish subscriber channel");
                    }
                }
            }


            foreach (var route in routes)
            {
                if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                {
                    var channelpath = _builder.BuildFromRoute(route);

                    pointtopointchannel.Listen(route, message => _router.Route(message, route), channelpath);

                    Console.WriteLine($"Listening {channelpath} point to point channel");
                }

                if (!string.IsNullOrWhiteSpace(route.ToPath) && !string.IsNullOrWhiteSpace(route.ToSubscription))
                {
                    var channelpath = _builder.BuildFromRoute(route);

                    publishsubscriberchannel.Listen(route, message => _router.Route(message, route), channelpath);

                    Console.WriteLine($"Listening {channelpath} publish subscriber channel");
                }
            }
        }
    }
}