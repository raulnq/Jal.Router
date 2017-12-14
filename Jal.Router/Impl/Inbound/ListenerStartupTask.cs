using System;
using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class ListenerStartupTask : IStartupTask
    {
        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        private readonly IRouterConfigurationSource[] _sources;

        public ListenerStartupTask(IComponentFactory factory, IConfiguration configuration, IRouterConfigurationSource[] sources)
        {
            _factory = factory;
            _configuration = configuration;
            _sources = sources;
        }

        public void ListenPointToPointChannel(IPointToPointChannel channel, string connectionstring, string path, Saga saga,
            Route route, bool startingroute)
        {
            channel.Listen(connectionstring, path, saga, route, startingroute);
        }

        public void ListenPublishSubscriberChannel(IPublishSubscribeChannel channel, string connectionstring, string path,
            string subscription, Saga saga, Route route, bool startingroute)
        {
            channel.Listen(connectionstring, path, subscription, saga, route, startingroute);
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
                        ListenPointToPointChannel(pointtopointchannel, route.ToConnectionString, route.ToPath, saga, route, true);

                        Console.WriteLine($"Listening {saga.Name}/{route.Name}/{route.ToPath} point to point channel");
                    }

                    if (!string.IsNullOrWhiteSpace(route.ToPath) && !string.IsNullOrWhiteSpace(route.ToSubscription))
                    {
                        ListenPublishSubscriberChannel(publishsubscriberchannel, route.ToConnectionString, route.ToPath, route.ToSubscription, saga, route, true);

                        Console.WriteLine($"Listening {saga.Name}/{route.Name}/{route.ToPath}/{route.ToSubscription} publish subscriber channel");
                    }
                }

                foreach (var route in saga.NextRoutes)
                {
                    if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                    {
                        ListenPointToPointChannel(pointtopointchannel, route.ToConnectionString, route.ToPath, saga, route, false);

                        Console.WriteLine($"Listening {saga.Name}/{route.Name}/{route.ToPath} point to point channel");
                    }

                    if (!string.IsNullOrWhiteSpace(route.ToPath) && !string.IsNullOrWhiteSpace(route.ToSubscription))
                    {
                        ListenPublishSubscriberChannel(publishsubscriberchannel, route.ToConnectionString, route.ToPath, route.ToSubscription, saga, route, false);

                        Console.WriteLine($"Listening {saga.Name}/{route.Name}/{route.ToPath}/{route.ToSubscription} publish subscriber channel");
                    }
                }
            }


            foreach (var route in routes)
            {
                if (!string.IsNullOrWhiteSpace(route.ToPath) && string.IsNullOrWhiteSpace(route.ToSubscription))
                {
                    ListenPointToPointChannel(pointtopointchannel, route.ToConnectionString, route.ToPath, null, route, false);

                    Console.WriteLine($"Listening {route.Name}/{route.ToPath} point to point channel");
                }

                if (!string.IsNullOrWhiteSpace(route.ToPath) && !string.IsNullOrWhiteSpace(route.ToSubscription))
                {
                    ListenPublishSubscriberChannel(publishsubscriberchannel, route.ToConnectionString, route.ToPath, route.ToSubscription, null, route, false);

                    Console.WriteLine($"Listening {route.Name}/{route.ToPath}/{route.ToSubscription} publish subscriber channel");
                }
            }
        }
    }
}