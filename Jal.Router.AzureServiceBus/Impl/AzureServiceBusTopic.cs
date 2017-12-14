using System;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    public class AzureServiceBusTopic : AbstractPublishSubscribeChannel
    {
        public override void Send<TContent>(MessageContext<TContent> context, IMessageAdapter adapter)
        {
            var topicClient = TopicClient.CreateFromConnectionString(context.ToConnectionString, context.ToPath);

            var message = adapter.Write<TContent, BrokeredMessage>(context);

            topicClient.Send(message);

            topicClient.Close();
        }

        public override void Listen(string connectionstring, string path, string subscription, Saga saga, Route route, bool startingroute)
        {
            var subscriptionclient = SubscriptionClient.CreateFromConnectionString(connectionstring, path, subscription);

            var entityPath = SubscriptionClient.FormatSubscriptionPath(path, subscription);

            var messagereceiver = subscriptionclient.MessagingFactory.CreateMessageReceiver(entityPath);

            var options = new OnMessageOptions() {AutoComplete = false};

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
                var result = ProcessMessage(path, subscription, saga, route, startingroute, brokeredmessage.MessageId, brokeredmessage, typeof(BrokeredMessage) );

                if (result)
                {
                    try
                    {
                        subscriptionclient.Complete(brokeredmessage.LockToken);
                    }
                    catch (Exception ex)
                    {

                        if (saga != null)
                        {
                            Console.WriteLine($"Message {brokeredmessage.MessageId} failed to publish subscriber channel {saga.Name}/{route.Name}/{path}/{subscription} {ex}");
                        }
                        else
                        {
                            Console.WriteLine($"Message {brokeredmessage.MessageId} failed to publish subscriber channel {route.Name}/{path}/{subscription} {ex}");
                        }
                    }
                }

            }, options);

            route.ShutdownAction = () => { messagereceiver.Close(); subscriptionclient.Close(); };
        }

        private readonly int _maxconcurrentcalls;

        private readonly TimeSpan? _autorenewtimeout;
        public AzureServiceBusTopic(IComponentFactory factory, IConfiguration configuration, IRouter router, int maxconcurrentcalls=0, TimeSpan? autorenewtimeout=null) : base(factory, configuration, router)
        {
            _maxconcurrentcalls = maxconcurrentcalls;
            _autorenewtimeout = autorenewtimeout;
        }
    }
}