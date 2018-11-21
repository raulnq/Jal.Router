﻿using System;
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
            container.RegisterAzureSagaStorage("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==", "sagasmoke", "messagessmoke", DateTime.UtcNow.ToString("yyyyMMdd"));
            container.RegisterAzureMessageStorage("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==", "messages");
            //container.Register<IMessageHandler, MessageHandler>(typeof(MessageHandler).FullName, new PerContainerLifetime());
            //container.Register<IMessageHandler, MessageHandlerB>(typeof(MessageHandlerB).FullName, new PerContainerLifetime());

            container.Register<IMessageHandler<Message>, QueueListenByOneHandler>(typeof(QueueListenByOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByTwoAHandlers>(typeof(QueueListenByTwoAHandlers).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByTwoBHandlers>(typeof(QueueListenByTwoBHandlers).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, TopicListenByOneHandler>(typeof(TopicListenByOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, HandlingTwoQueuesInOneHandler>(typeof(HandlingTwoQueuesInOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByOneHandlerWithException>(typeof(QueueListenByOneHandlerWithException).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, ToPublishHandler>(typeof(ToPublishHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, FromPublishHandler>(typeof(FromPublishHandler).FullName, new PerContainerLifetime());
            container.Register<IMiddleware<MessageContext>, Middleware>(typeof(Middleware).FullName, new PerContainerLifetime());
            container.Register<IMessageHandlerWithData<Message,Data>, StartHandler>(typeof(StartHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandlerWithData<Message, Data>, ContinueHandler>(typeof(ContinueHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandlerWithData<Message, Data>, EndHandler>(typeof(EndHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, ToReplyHandler>(typeof(ToReplyHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, FromReplyHandler>(typeof(FromReplyHandler).FullName, new PerContainerLifetime());
            
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
        private readonly string _toreplyqueue = "toreplyqueue";

        private readonly string _replyqueue = "replyqueue";

        private readonly string _fromreplyqueue = "fromreplyqueue";

        private readonly string _queuetopublishtopic = "queuetopublishtopic";

        private readonly string _topicpublishedfromqueue = "topicpublishedfromqueue";

        private readonly string _queuestart = "queuestart";

        private readonly string _queuecontinue = "queuecontinue";

        private readonly string _queueend = "queueend";

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

            this.RegisterQueue(_replyqueue, config);

            this.RegisterQueue(_fromreplyqueue, config);

            this.RegisterQueue(_toreplyqueue, config);

            this.RegisterQueue(_queuelistenbyonehandler, config);

            this.RegisterQueue(_queuetopublishtopic, config);

            this.RegisterTopic(_topicpublishedfromqueue, config);

            this.RegisterSubscriptionToTopic(_subscription, _topicpublishedfromqueue, config, "1=1");

            this.RegisterQueue(_queuestart, config);

            this.RegisterQueue(_queuecontinue, config);

            this.RegisterQueue(_queueend, config);

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

            RegisterHandler<IMessageHandler<Message>>(_toreplyqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_toreplyqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Using<ToReplyHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_fromreplyqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_fromreplyqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Using<FromReplyHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("toreplyendpoint")
            .ForMessage<Message>()
            .To(x => x.AddQueue(config.ConnectionString, _fromreplyqueue).AndWaitReplyFromQueue(_replyqueue, config.ConnectionString));

            RegisterEndPoint("replyendpoint")
            .ForMessage<Message>()
            .To(x => x.AddQueue(config.ConnectionString, _replyqueue));

            RegisterHandler<IMessageHandler<Message>>(_queuetopublishtopic + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuetopublishtopic, config.ConnectionString);
            })
            .ForMessage<Message>().Using<ToPublishHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_topicpublishedfromqueue + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToTopic(_topicpublishedfromqueue, _subscription, config.ConnectionString);
            })
            .ForMessage<Message>().Using<FromPublishHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("fromqueueendpoint")
            .ForMessage<Message>()
            .To(x => x.AddTopic(config.ConnectionString, _topicpublishedfromqueue));


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

            RegisterSaga<Data>("saga", x => {
                x.RegisterHandler<IMessageHandlerWithData<Message, Data>>("start")
                .ToListen(y => y.AddQueue(_queuestart, config.ConnectionString))
                .ForMessage<Message>().Using<StartHandler>(y => {
                    y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "START");
                });
            },
                x =>
                {
                    x.RegisterHandler<IMessageHandlerWithData<Message, Data>>("continue")
                    .ToListen(y => y.AddQueue(_queuecontinue, config.ConnectionString))
                    .ForMessage<Message>().Using<ContinueHandler>(y =>
                    {
                        y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "CONTINUE");
                    });
                },
                x =>
                {
                    x.RegisterHandler<IMessageHandlerWithData<Message, Data>>("end")
                    .ToListen(y => y.AddQueue(_queueend, config.ConnectionString))
                    .ForMessage<Message>().Using<EndHandler>(y =>
                    {
                        y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data),"END");
                    });
                }
                );

            RegisterEndPoint("continueendpoint")
            .ForMessage<Message>()
            .To(x => x.AddQueue(config.ConnectionString, _queuecontinue));

            RegisterEndPoint("endendpoint")
            .ForMessage<Message>()
            .To(x => x.AddQueue(config.ConnectionString, _queueend));

        }
    }

    public class EndHandler : AbstractMessageHandlerWithData<Message, Data>
    {
        public override void HandleWithContextAndData(Message message, MessageContext context, Data data)
        {
            Console.WriteLine("message handled by " + GetType().Name + " " + data.Status);

            data.Status = "end";
        }
    }

    public class ContinueHandler : AbstractMessageHandlerWithData<Message, Data>
    {
        public override void HandleWithContextAndData(Message message, MessageContext context, Data data)
        {
            Console.WriteLine("message handled by " + GetType().Name + " " + data.Status);

            data.Status = "continue";

            context.Send(data, new Message() { }, "endendpoint", context.Identity.Id+"_end", context.Identity.OperationId, context.SagaContext.Id);
        }
    }

    public class StartHandler : AbstractMessageHandlerWithData<Message, Data>
    {
        public override void HandleWithContextAndData(Message message, MessageContext context, Data data)
        {
            data.Status = "initial";

            Console.WriteLine("message handled by " + GetType().Name + " " + data.Status);

            data.Status = "start";

            context.Send(data, new Message() { }, "continueendpoint", context.Identity.Id+ "_continue", context.Identity.OperationId, context.SagaContext.Id);
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

    public class FromPublishHandler : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

        }
    }

    
    public class ToReplyHandler : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            var result = context.Reply<Message, Message>(new Message() { }, "toreplyendpoint", context.Identity.Id, context.Identity.OperationId);
        }
    }

    public class FromReplyHandler : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            context.Send(new Message() { Name = "Hi" }, "replyendpoint", context.Identity.Id, context.Identity.OperationId);
        }
    }

    
    public class ToPublishHandler : AbstractMessageHandler<Message>
    {
        public override void HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            context.Publish(new Message() { }, "fromqueueendpoint", context.Identity.Id, context.Identity.OperationId);
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

  
    public class Data
    {
        public string Status { get; set; }
    }

    public class Message
    {
        public string Name { get; set; }
    }
}
