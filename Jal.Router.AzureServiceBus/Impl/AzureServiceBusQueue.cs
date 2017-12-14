using System;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusQueue : AbstractPointToPointChannel
    {
        public override void Send<TContent>(MessageContext<TContent> context, IMessageAdapter adapter)
        {
            var queueclient = QueueClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = adapter.Write<TContent, BrokeredMessage>(context);

            queueclient.Send(message);

            queueclient.Close();
        }

        public override void Listen(string connectionstring, string path, Saga saga, Route route, bool startingroute)
        {
            var queueclient = QueueClient.CreateFromConnectionString(connectionstring, path);

            var messagereceiver = queueclient.MessagingFactory.CreateMessageReceiver(path);

            var options = new OnMessageOptions() { AutoComplete = false };

            if (_maxconcurrentcalls > 0)
            {
                options.MaxConcurrentCalls = _maxconcurrentcalls;
            }
            if (_autorenewtimeout != null)
            {
                options.AutoRenewTimeout = _autorenewtimeout.Value;
            }

            messagereceiver.OnMessage(brokeredmessage =>
            {
                var result = ProcessMessage(path, saga, route, startingroute, brokeredmessage.MessageId, brokeredmessage, typeof(BrokeredMessage));

                if (result)
                {
                    try
                    {
                        queueclient.Complete(brokeredmessage.LockToken);
                    }
                    catch (Exception ex)
                    {
                        if (saga != null)
                        {
                            Console.WriteLine($"Message {brokeredmessage.MessageId} failed to point to point channel {saga.Name}/{route.Name}/{path} {ex}");
                        }
                        else
                        {
                            Console.WriteLine($"Message {brokeredmessage.MessageId} failed to point to point channel {route.Name}/{path} {ex}");
                        }
                    }
                }
            }, options);

            route.ShutdownAction = () => { messagereceiver.Close(); queueclient.Close(); };
        }

        private readonly int _maxconcurrentcalls;

        private readonly TimeSpan? _autorenewtimeout;

        public AzureServiceBusQueue(IComponentFactory factory, IConfiguration configuration, IRouter router, int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null) : base(factory, configuration, router)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}