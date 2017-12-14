using System;
using System.Linq;
using System.Reflection;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractPointToPointChannel : IPointToPointChannel
    {
        protected readonly IComponentFactory Factory;

        private readonly IConfiguration _configuration;

        private readonly IRouter _router;

        protected AbstractPointToPointChannel(IComponentFactory factory, IConfiguration configuration, IRouter router)
        {
            Factory = factory;
            _configuration = configuration;
            _router = router;
        }

        public abstract void Send<TContent>(MessageContext<TContent> context, IMessageAdapter adapter);

        public abstract void Listen(string connectionstring, string path, Saga saga, Route route, bool startingroute);

        public void Send<TContent>(MessageContext<TContent> context)
        {
            var adapter = Factory.Create<IMessageAdapter>(_configuration.MessageAdapterType);

            Send(context, adapter);
        }

        public bool ProcessMessage(string path, Saga saga, Route route, bool startingroute, string messageid, object message, Type messagetype)
        {
            if (saga != null)
            {
                Console.WriteLine($"Message {messageid} arrived to point to point channel {saga.Name}/{route.Name}/{path}");
            }
            else
            {
                Console.WriteLine($"Message {messageid} arrived to point to point channel {route.Name}/{path}");
            }

            try
            {
                if (saga != null)
                {
                    var routemethod = typeof(IRouter).GetMethods().First(x => x.Name == nameof(IRouter.Route) && x.GetParameters().Count() == 4);

                    var genericroutemethod = routemethod?.MakeGenericMethod(route.ContentType, messagetype);

                    genericroutemethod?.Invoke(_router, new [] { message, saga, route, startingroute });
                }
                else
                {
                    var routemethod = typeof(IRouter).GetMethods().First(x => x.Name == nameof(IRouter.Route) && x.GetParameters().Count() == 2);

                    var genericroutemethod = routemethod?.MakeGenericMethod(route.ContentType, messagetype);

                    genericroutemethod?.Invoke(_router, new [] { message, route });
                }
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                {
                    if (saga != null)
                    {
                        Console.WriteLine($"Message {messageid} failed to publish subscriber channel {saga.Name}/{route.Name}/{path} {ex.InnerException}");
                    }
                    else
                    {
                        Console.WriteLine($"Message {messageid} failed to publish subscriber channel {route.Name}/{path} {ex.InnerException}");
                    }

                    return false;
                }
                else
                {
                    if (saga != null)
                    {
                        Console.WriteLine($"Message {messageid} failed to publish subscriber channel {saga.Name}/{route.Name}/{path} {ex}");
                    }
                    else
                    {
                        Console.WriteLine($"Message {messageid} failed to publish subscriber channel {route.Name}/{path} {ex}");
                    }

                    return false;
                }
            }

            if (saga != null)
            {
                Console.WriteLine($"Message {messageid} completed to point to point channel {saga.Name}/{route.Name}/{path}");
            }
            else
            {
                Console.WriteLine($"Message {messageid} completed to point to point channel {route.Name}/{path}");
            }

            return true;
        }

    }
}