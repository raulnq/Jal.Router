using System;
using Jal.Locator.LightInject.Installer;
using Jal.Router.AzureServiceBus.Standard.Extensions;
using Jal.Router.AzureServiceBus.Standard.LightInject.Installer;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.LightInject.Installer;
using Jal.Router.Model;
using LightInject;
using Jal.Router.Extensions;
using Jal.Router.Impl.Management;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.AzureStorage.LightInject.Installer;
using Jal.Router.Model.Management;
using Jal.Router.Impl.Outbound;
using Jal.Router.Impl.MonitoringTask;
using Jal.ChainOfResponsability.LightInject.Installer;
using Jal.Router.Impl.Outbound.ChannelShuffler;
using Jal.Router.Impl.Patterns;
using Jal.Router.Interface.Patterns;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Impl.Inbound.RetryPolicy;

namespace Jal.Router.Sample.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new ServiceContainer();
            container.RegisterRouter(new IRouterConfigurationSource[] { new RouterConfigurationSmokeTest() });
            container.RegisterFrom<ServiceLocatorCompositionRoot>();
            container.RegisterAzureServiceBusRouter();
            container.RegisterFrom<ChainOfResponsabilityCompositionRoot>();
            container.RegisterAzureSagaStorage("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==", "sagacore", "messagescore", DateTime.UtcNow.ToString("yyyyMMdd"));
            container.RegisterAzureMessageStorage("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==", "messages");
            //container.Register<IMessageHandler, MessageHandler>(typeof(MessageHandler).FullName, new PerContainerLifetime());
            //container.Register<IMessageHandler, MessageHandlerB>(typeof(MessageHandlerB).FullName, new PerContainerLifetime());

            container.Register<IMessageHandler<Message>, QueueListenByOneHandler>(typeof(QueueListenByOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByTwoAHandlers>(typeof(QueueListenByTwoAHandlers).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByTwoBHandlers>(typeof(QueueListenByTwoBHandlers).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, TopicListenByOneHandler>(typeof(TopicListenByOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, HandlingTwoQueuesInOneHandler>(typeof(HandlingTwoQueuesInOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByOneHandlerWithException>(typeof(QueueListenByOneHandlerWithException).FullName, new PerContainerLifetime());
            container.Register<IMiddleware<MessageContext>, Middleware>(typeof(Middleware).FullName, new PerContainerLifetime());

            var host = container.GetInstance<IHost>();
            host.Configuration.UsingAzureServiceBus();
            host.Configuration.UsingAzureSagaStorage();
            host.Configuration.UsingAzureMessageStorage();
            //host.Configuration.UsingChannelShuffler<FisherYatesChannelShuffler>();
            host.Configuration.AddMonitoringTask<HeartBeatLogger>(1000);
            host.RunAndBlock();
        }
    }

    public class RouterConfigurationSmokeTest : AbstractRouterConfigurationSource
    {
        private readonly string _queuelistenbyonehandler = "queuelistenbyonehandler";

        private readonly string _queuelistenbyonehandlerwithwhen = "queuelistenbyonehandlerwithwhen";

        private readonly string _queuelistenbytwohandlers = "queuelistenbytwohandlers";

        private readonly string _handlingtwoqueuesinonehandlera = "handlingtwoqueuesinonehandlera";

        private readonly string _handlingtwoqueuesinonehandlerb = "handlingtwoqueuesinonehandlerb";

        private readonly string _queuelistenbyonehandlerwithmiddleware = "queuelistenbyonehandlerwithmiddleware";

        private readonly string _topiclistenbyonehandler = "topiclistenbyonehandler";

        private readonly string _queuelistenbyonehandlerwithexception = "queuelistenbyonehandlerwithexception";

        private readonly string _queuelistenbyonehandlerwithexceptionandretry = "queuelistenbyonehandlerwithexceptionandretry";

        private readonly string _subscription = "subscription";

        private readonly string _errorqueue = "errorqueue";

        private readonly string _errorqueueendpoint = "errorqueueendpoint";

        public RouterConfigurationSmokeTest()
        {
            var config = new ServiceBusConfiguration()
            {
                ClientId = "e40d9bbb-c50f-436e-8a5f-8494e0f84242",
                ClientSecret = "OkDfucL/DT9h1FISlh79OfAnmwu9/h/TRx4ryFG+hIc=",
                ConnectionString = "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=",
                ResourceGroupName = "TestQueueApps",
                ResourceName = "raulqueuetests",
                SubscriptionId = "e759b3f9-6ac3-4f9d-b479-1ba4471235cd",
                TenantId = "77f43f1b-5708-46dd-92a2-5f99f19e9b1f"
            };

            RegisterOrigin("smoketestapp", "123");

            this.RegisterQueue(_queuelistenbyonehandler, config);

            this.RegisterQueue(_errorqueue, config);
            
            this.RegisterQueue(_queuelistenbyonehandlerwithexceptionandretry, config);

            this.RegisterQueue(_queuelistenbyonehandlerwithexception, config);

            this.RegisterQueue(_queuelistenbyonehandlerwithmiddleware, config);

            this.RegisterQueue(_queuelistenbyonehandlerwithwhen, config);
            
            this.RegisterQueue(_handlingtwoqueuesinonehandlera, config);

            this.RegisterQueue(_handlingtwoqueuesinonehandlerb, config);

            this.RegisterQueue(_queuelistenbytwohandlers, config);

            this.RegisterTopic(_topiclistenbyonehandler,  config);

            this.RegisterSubscriptionToTopic(_subscription, _topiclistenbyonehandler, config, "1=1");

            RegisterHandler<IMessageHandler<Message>>("handlingtwoqueuesinonehandler_handler")
            .ToListen(x =>
            {
                x.AddQueue(_handlingtwoqueuesinonehandlera, config.ConnectionString);
                x.AddQueue(_handlingtwoqueuesinonehandlerb, config.ConnectionString);
            })
            .ForMessage<Message>().Using<HandlingTwoQueuesInOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandler+"_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandler, config.ConnectionString);
            })
            .ForMessage<Message>().Using<QueueListenByOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbytwohandlers + "_A_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbytwohandlers, config.ConnectionString);
            })
            .ForMessage<Message>().Using<QueueListenByTwoAHandlers>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbytwohandlers + "_B_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbytwohandlers, config.ConnectionString);
            })
            .ForMessage<Message>().Using<QueueListenByTwoBHandlers>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_topiclistenbyonehandler + "_handler")
            .ToListen(x =>
            {
                x.AddPublishSubscribeChannel(_topiclistenbyonehandler, _subscription, config.ConnectionString);
            })
            .ForMessage<Message>().Using<TopicListenByOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandlerwithwhen + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithwhen, config.ConnectionString);
            })
            .ForMessage<Message>().Using<QueueListenByOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).When(x=>x.Headers.ContainsKey("test"));

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandlerwithmiddleware + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithmiddleware, config.ConnectionString);
            })
            .ForMessage<Message>().Using<QueueListenByOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).UseMiddleware(x => x.Add<Middleware>());

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandlerwithexception + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithexception, config.ConnectionString);
            })
            .ForMessage<Message>().Using<QueueListenByOneHandlerWithException>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).OnErrorSendFailedMessageTo(_errorqueueendpoint);

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandlerwithexceptionandretry + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithexceptionandretry, config.ConnectionString);
            })
            .ForMessage<Message>().Using<QueueListenByOneHandlerWithException>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            })
            .OnExceptionRetryFailedMessageTo<ApplicationException>("retryendpoint").Using(new LinearRetryPolicy(5,3))
            .OnErrorSendFailedMessageTo(_errorqueueendpoint); 

            RegisterEndPoint(_errorqueueendpoint)
                .ForMessage<Message>()
                .To(x => x.AddQueue(config.ConnectionString, _errorqueue));

            RegisterEndPoint("retryendpoint")
                .ForMessage<Message>()
                .To(x => x.AddQueue(config.ConnectionString, _queuelistenbyonehandlerwithexceptionandretry));
        }
    }

    public class Middleware : IMiddleware<MessageContext>
    {
        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            Console.WriteLine("Start " + GetType().Name);
            next(context);
            Console.WriteLine("End " + GetType().Name);
        }
    }

    public class TopicListenByOneHandler : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
        }
    }

    public class QueueListenByOneHandler : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
        }
    }

    public class QueueListenByOneHandlerWithException : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
            throw new ApplicationException("Error");
        }
    }

    public class HandlingTwoQueuesInOneHandler : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
        }
    }

    public class QueueListenByTwoAHandlers : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
        }
    }

    public class QueueListenByTwoBHandlers : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
        }
    }

    public class RouterConfigurationSourceSample : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSourceSample()
        {
            RegisterHandler<IMessageHandler>("handler")
                .ToListen(x =>
                {
                    x.AddPublishSubscribeChannel("inputtopicnewrelease2", "subs", "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");
                    x.AddPointToPointChannel("inputqueuenewrelease2", "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");
                })
                .ForMessage<Message>().Using<MessageHandler>(x =>
                {
                    x.With((request, handler, context) => handler.Handle(request, context)).When((request, handler, context) => true);
                }).OnEntry(x => x.BuildOperationIdWith(y => "operationid"));

            RegisterHandler<IMessageHandler>("handler")
                .ToListen(x => {
                    x.AddPointToPointChannel("outputqueuenewrelease1", "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");
                    x.AddPointToPointChannel("outputqueuenewrelease2", "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");
                    x.AddPointToPointChannel("outputqueuenewrelease3", "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");
                })
                .ForMessage<Message>().Using<MessageHandlerB>(x =>
                {
                    x.With((request, handler, context) => handler.Handle(request, context)).When((request, handler, context) => true);
                });


            RegisterOrigin("newcoreapp", "123");

            RegisterEndPoint("endpoint")
             .ForMessage<Message>()
             //.AsClaimCheck()
             .To(x =>
                {
                    x.AddPointToPointChannel("Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueuenewrelease1");
                    x.AddPointToPointChannel("Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueuenewrelease2");
                    x.AddPointToPointChannel("Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueuenewrelease3");
                });

            var config = new ServiceBusConfiguration()
            {
                ClientId = "e40d9bbb-c50f-436e-8a5f-8494e0f84242",
                ClientSecret = "OkDfucL/DT9h1FISlh79OfAnmwu9/h/TRx4ryFG+hIc=",
                ConnectionString = "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=",
                ResourceGroupName = "TestQueueApps",
                ResourceName = "raulqueuetests",
                SubscriptionId = "e759b3f9-6ac3-4f9d-b479-1ba4471235cd",
                TenantId = "77f43f1b-5708-46dd-92a2-5f99f19e9b1f"
            };

            this.RegisterQueue("inputqueuenewrelease2", config);

            this.RegisterTopic("inputtopicnewrelease2",  config);

            this.RegisterSubscriptionToTopic("subs", "inputtopicnewrelease2", config);

            this.RegisterQueue("outputqueuenewrelease1", config);

            this.RegisterQueue("outputqueuenewrelease2", config);

            this.RegisterQueue("outputqueuenewrelease3", config);
        }
    }

    public interface IMessageHandler
    {
        void Handle(Message message, MessageContext context);
    }

    public class MessageHandler : IMessageHandler
    {
        public void Handle(Message message, MessageContext context)
        {
            //throw new Exception("Errorr");
            context.Send(new Message() { Name = "Hi" }, "endpoint", Guid.NewGuid().ToString(), context.Identity.Id);
        }

    }

    public class MessageHandlerB : IMessageHandler
    {
        public void Handle(Message message, MessageContext context)
        {
            Console.WriteLine(message.Name);
        }

    }


    public class Message
    {
        public string Name { get; set; }
    }
}
