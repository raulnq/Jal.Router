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
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.AzureStorage.LightInject.Installer;
using Jal.Router.Impl.MonitoringTask;
using Jal.ChainOfResponsability.LightInject.Installer;
using Jal.Router.Impl.Patterns;
using Jal.Router.Interface.Patterns;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Impl.Inbound.RetryPolicy;
using Jal.Router.Impl.Management.ShutdownWatcher;
using Jal.Router.Impl.ValueFinder;
using System.Threading.Tasks;
using Jal.Router.Newtonsoft.Extensions;
using Jal.Router.Newtonsoft.LightInject.Installer;
using Jal.Router.Impl.Inbound.Middleware;
using System.Collections.Generic;
using Jal.Router.Impl.Inbound.RouteErrorMessageHandler;
using Jal.Router.Impl.Inbound.RouteEntryMessageHandler;

namespace Jal.Router.Sample.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new ServiceContainer();
            container.RegisterRouter(new IRouterConfigurationSource[] { new RouterConfigurationSmokeTest() });
            container.RegisterFrom<ServiceLocatorCompositionRoot>();
            container.RegisterFrom<AzureServiceBusCompositionRoot>();
            container.RegisterFrom<ChainOfResponsabilityCompositionRoot>();
            container.RegisterFrom<AzureStorageCompositionRoot>();
            container.RegisterFrom<NewtonsoftCompositionRoot>();

            container.Register<IMessageHandler<Message>, QueueListenByOneHandler>(typeof(QueueListenByOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByTwoAHandlers>(typeof(QueueListenByTwoAHandlers).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByTwoBHandlers>(typeof(QueueListenByTwoBHandlers).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, TopicListenByOneHandler>(typeof(TopicListenByOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, HandlingTwoQueuesInOneHandler>(typeof(HandlingTwoQueuesInOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByOneHandlerWithException>(typeof(QueueListenByOneHandlerWithException).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, ToPublishHandler>(typeof(ToPublishHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, FromPublishHandler>(typeof(FromPublishHandler).FullName, new PerContainerLifetime());
            container.Register<IMiddlewareAsync<MessageContext>, Middleware>(typeof(Middleware).FullName, new PerContainerLifetime());
            container.Register<IMessageHandlerWithData<Message,Data>, StartHandler>(typeof(StartHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandlerWithData<Message, Data>, AlternativeStartHandler>(typeof(AlternativeStartHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandlerWithData<Message, Data>, ContinueHandler>(typeof(ContinueHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandlerWithData<Message, Data>, EndHandler>(typeof(EndHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, ToReplyHandler>(typeof(ToReplyHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, FromReplyHandler>(typeof(FromReplyHandler).FullName, new PerContainerLifetime());

            container.Register<IMessageHandler<Message>, QueueToRead>(typeof(QueueToRead).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueToSend>(typeof(QueueToSend).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueToReadGroup>(typeof(QueueToReadGroup).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenSessionSenderHandlers>(typeof(QueueListenSessionSenderHandlers).FullName, new PerContainerLifetime());
            
            var host = container.GetInstance<IHost>();
            host.Configuration
                .UseAzureServiceBus(new AzureServiceBusParameter() { AutoRenewTimeoutInMinutes = 60, MaxConcurrentCalls=4, MaxConcurrentGroups=1, TimeoutInSeconds = 60 })
                .UseAzureStorage(new AzureStorage.Model.AzureStorageParameter("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==") { SagaTableName = "sagasmoke", MessageTableName = "messagessmoke", TableSufix = DateTime.UtcNow.ToString("yyyyMMdd"), ContainerName = "messages", TableStorageMaxColumnSizeOnKilobytes = 64 })
                .AddMonitoringTask<HeartBeatLogger>(15)
                .UseNewtonsoft()
                //.AddMonitoringTask<ListenerMonitor>(30)
                //.AddMonitoringTask<ListenerRestartMonitor>(60)
                //.AddMonitoringTask<PointToPointChannelMonitor>(60)
                //.EnableEntityStorage()
                .AddShutdownWatcher<SignTermShutdownWatcher>();

            var facade = container.GetInstance<IEntityStorageGateway>();

            var messages = facade.GetMessages(new DateTime(2019, 7,9), new DateTime(2019, 7, 10), "queuelistenbyonehandler_handler", new Dictionary<string, string> { { "messagestoragename", "messagessmoke20190709" } }).GetAwaiter().GetResult();

            host.RunAndBlock();
        }
    }

    public class RouterConfigurationSmokeTest : AbstractRouterConfigurationSource
    {
        private readonly string _queueperformancetosend = "queueperformancetosend";

        private readonly string _queueperformancetoread = "queueperformancetoread";

        private readonly string _toreplyqueue = "toreplyqueue";

        private readonly string _replyqueue = "replyqueue";

        private readonly string _fromreplyqueue = "fromreplyqueue";

        private readonly string _queuetopublishtopic = "queuetopublishtopic";

        private readonly string _topicpublishedfromqueue = "topicpublishedfromqueue";

        private readonly string _queuestart = "queuestart";

        private readonly string _alternativequeuestart = "alternativequeuestart";

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

        private readonly string _sessionqueue = "sessionqueue";

        private readonly string _sessiontopic = "sessiontopic";

        private readonly string _forwardqueue = "forwardqueue";

        private readonly string _forwardqueueendpoint = "forwardqueueendpoint";

        private readonly string _sendersessionqueue = "sendersessionqueue";

        public RouterConfigurationSmokeTest()
        {
            var config = new AzureServiceBusConfiguration()
            {
                ClientId = "e40d9bbb-c50f-436e-8a5f-8494e0f84242",
                ClientSecret = "OkDfucL/DT9h1FISlh79OfAnmwu9/h/TRx4ryFG+hIc=",
                ConnectionString = "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=",
                ResourceGroupName = "TestQueueApps",
                ResourceName = "raulqueuetests",
                SubscriptionId = "e759b3f9-6ac3-4f9d-b479-1ba4471235cd",
                TenantId = "77f43f1b-5708-46dd-92a2-5f99f19e9b1f"
            };

            RegisterGroup("xx").ForQueue(_sessionqueue, config.ConnectionString).Until(x => false);

            RegisterGroup("yy").ForSubscriptionToTopic(_sessiontopic, _subscription, config.ConnectionString).Until(x => false);


            RegisterHandler<IMessageHandler<Message>>(_sendersessionqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_sendersessionqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueListenSessionSenderHandlers>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_sessionqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_sessionqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueToReadGroup>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_sessiontopic + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToTopic(_sessiontopic, _subscription, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueToReadGroup>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("sessionqueueendpoint")
            .ForMessage<Message>()
            .To(x => x.AddQueue(config.ConnectionString, _sessionqueue));

            RegisterEndPoint("sessiontopicendpoint")
            .ForMessage<Message>()
            .To(x => x.AddTopic(config.ConnectionString, _sessiontopic));

            RegisterOrigin("smoketestapp", "123");

            this.RegisterQueue(new AzureServiceBusQueue(_sendersessionqueue) { }, config);

            this.RegisterQueue(new AzureServiceBusQueue(_sessionqueue) { SessionEnabled = true }, config);

            this.RegisterTopic(new AzureServiceBusTopic(_sessiontopic) { }, config);

            this.RegisterSubscriptionToTopic(new AzureServiceBusSubscriptionToTopic(_subscription, _sessiontopic) { SessionEnabled = true }, config, "1=1");

            this.RegisterQueue(new AzureServiceBusQueue(_queueperformancetosend), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queueperformancetoread), config);

            this.RegisterQueue(new AzureServiceBusQueue(_replyqueue) { SessionEnabled = true }, config);

            this.RegisterQueue(new AzureServiceBusQueue(_fromreplyqueue), config);

            this.RegisterQueue(new AzureServiceBusQueue(_toreplyqueue), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandler), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queuetopublishtopic), config);

            this.RegisterTopic(new AzureServiceBusTopic(_topicpublishedfromqueue), config);

            this.RegisterSubscriptionToTopic(new AzureServiceBusSubscriptionToTopic(_subscription, _topicpublishedfromqueue), config, "1=1");

            this.RegisterQueue(new AzureServiceBusQueue(_queuestart), config);

            this.RegisterQueue(new AzureServiceBusQueue(_alternativequeuestart), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queuecontinue), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queueend), config);

            this.RegisterQueue(new AzureServiceBusQueue(_errorqueue), config);
            
            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandlerwithexceptionandretry), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandlerwithexception), config);

            this.RegisterQueue(new AzureServiceBusQueue(_forwardqueue), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandlerwithmiddleware), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandlerwithwhen), config);
            
            this.RegisterQueue(new AzureServiceBusQueue(_handlingtwoqueuesinonehandlera), config);

            this.RegisterQueue(new AzureServiceBusQueue(_handlingtwoqueuesinonehandlerb), config);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbytwohandlers), config);

            this.RegisterTopic(new AzureServiceBusTopic(_topiclistenbyonehandler),  config);

            this.RegisterSubscriptionToTopic(new AzureServiceBusSubscriptionToTopic(_subscription, _topiclistenbyonehandler), config, "1=1");


            RegisterHandler<IMessageHandler<Message>>(_queueperformancetoread + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queueperformancetoread, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueToRead>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queueperformancetosend + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queueperformancetosend, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueToSend>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("queueperformanceendpoint")
            .ForMessage<Message>()
            .To(x => x.AddPointToPointChannel(config.ConnectionString, _queueperformancetoread));

            RegisterHandler<IMessageHandler<Message>>(_toreplyqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_toreplyqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Use<ToReplyHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_fromreplyqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_fromreplyqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Use<FromReplyHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("toreplyendpoint")
            .ForMessage<Message>()
            .To<Message>(x => x.AddQueue(config.ConnectionString, _fromreplyqueue).AndWaitReplyFromQueue(_replyqueue, config.ConnectionString));

            RegisterEndPoint("replyendpoint")
            .ForMessage<Message>()
            .To(x => x.AddQueue(config.ConnectionString, _replyqueue));

            RegisterHandler<IMessageHandler<Message>>(_queuetopublishtopic + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuetopublishtopic, config.ConnectionString);
            })
            .ForMessage<Message>().Use<ToPublishHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_topicpublishedfromqueue + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToTopic(_topicpublishedfromqueue, _subscription, config.ConnectionString);
            })
            .ForMessage<Message>().Use<FromPublishHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("fromqueueendpoint")
            .ForMessage<Message>()
            .To(x => x.AddPublishSubscribeChannel(config.ConnectionString, _topicpublishedfromqueue));


            RegisterHandler<IMessageHandler<Message>>("handlingtwoqueuesinonehandler_handler")
            .ToListen(x =>
            {
                x.AddQueue(_handlingtwoqueuesinonehandlera, config.ConnectionString);
                x.AddQueue(_handlingtwoqueuesinonehandlerb, config.ConnectionString);
            })
            .ForMessage<Message>().Use<HandlingTwoQueuesInOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandler+"_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandler, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueListenByOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbytwohandlers + "_A_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbytwohandlers, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueListenByTwoAHandlers>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbytwohandlers + "_B_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbytwohandlers, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueListenByTwoBHandlers>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_topiclistenbyonehandler + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToPublishSubscribeChannel(_topiclistenbyonehandler, _subscription, config.ConnectionString);
            })
            .ForMessage<Message>().Use<TopicListenByOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandlerwithwhen + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithwhen, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueListenByOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).When(x=>x.Headers.ContainsKey("test"));

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandlerwithmiddleware + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithmiddleware, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueListenByOneHandler>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).UseMiddleware(x => x.Add<Middleware>());

            Func<MessageContext, Handler, Task> init = (c, r) => {
                Console.WriteLine("HIiii");
                return Task.CompletedTask;
            };

            RegisterEndPoint(_forwardqueueendpoint)
            .ForMessage<Message>()
            .To(x => x.AddPointToPointChannel(config.ConnectionString, _forwardqueue));

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandlerwithexception + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithexception, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueListenByOneHandlerWithException>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).OnError(e=>e.Use<ForwardRouteErrorMessageHandler>(new Dictionary<string, object>() { { "endpoint", _errorqueueendpoint } }) )
            .OnEntry(e=> {
                e.Use<RouteEntryMessageHandler>(new Dictionary<string, object>() { { "init", init } });
                e.Use<ForwardRouteEntryMessageHandler>(new Dictionary<string, object>() { { "endpoint", _forwardqueueendpoint } });
            });

            Func<MessageContext, Exception, ErrorHandler, Task > func = (c, e, r) => {
                Console.WriteLine("HIiii");
                return Task.CompletedTask;
            }; 

            RegisterHandler<IMessageHandler<Message>>(_queuelistenbyonehandlerwithexceptionandretry + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithexceptionandretry, config.ConnectionString);
            })
            .ForMessage<Message>().Use<QueueListenByOneHandlerWithException>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            })
            //.OnExceptionRetryFailedMessageTo("retryendpoint", x=>x.For<ApplicationException>()).With(new LinearRetryPolicy(5, 3))
            .OnError(x=>x.Use<RetryRouteErrorMessageHandler>(new Dictionary<string, object>() { { "endpoint", "retryendpoint" } , { "policy", new LinearRetryPolicy(5, 3) }, { "fallback", func } }).For<ApplicationException>())
            //OnException(x=>x.OfType<ApplicationException>().Do<StoreLocally>(parameter));
            //.OnEntry(x=>x.EnableEntityStorage(true))
            //.OnErrorSendFailedMessageTo(_errorqueueendpoint)
            ; 

            RegisterEndPoint(_errorqueueendpoint)
                .ForMessage<Message>()
                .To(x => x.AddQueue(config.ConnectionString, _errorqueue));

            RegisterEndPoint("retryendpoint")
                .ForMessage<Message>()
                .To(x => x.AddQueue(config.ConnectionString, _queuelistenbyonehandlerwithexceptionandretry));

            RegisterSaga<Data>("saga", x => {
                x.RegisterHandler<IMessageHandlerWithData<Message, Data>>("start")
                .ToListen(y => y.AddQueue(_queuestart, config.ConnectionString))
                .ForMessage<Message>().Use<StartHandler>(y => {
                    y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "START");
                });

                x.RegisterHandler<IMessageHandlerWithData<Message, Data>>("alternative")
                .ToListen(y => y.AddQueue(_alternativequeuestart, config.ConnectionString))
                .ForMessage<Message>().Use<AlternativeStartHandler>(y => {
                    y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "START");
                });
            },
                x =>
                {
                    x.RegisterHandler<IMessageHandlerWithData<Message, Data>>("continue")
                    .ToListen(y => y.AddQueue(_queuecontinue, config.ConnectionString))
                    .ForMessage<Message>().Use<ContinueHandler>(y =>
                    {
                        y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "CONTINUE");
                    });
                },
                x =>
                {
                    x.RegisterHandler<IMessageHandlerWithData<Message, Data>>("end")
                    .ToListen(y => y.AddQueue(_queueend, config.ConnectionString))
                    .ForMessage<Message>().Use<EndHandler>(y =>
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
        public override Task HandleWithContextAndData(Message message, MessageContext context, Data data)
        {
            Console.WriteLine("message handled by " + GetType().Name + " " + data.Status);

            data.Status = "end";

            data.Name = message.Name;

            return Task.CompletedTask;
        }
    }

    public class ContinueHandler : AbstractMessageHandlerWithData<Message, Data>
    {
        public override Task HandleWithContextAndData(Message message, MessageContext context, Data data)
        {
            Console.WriteLine("message handled by " + GetType().Name + " " + data.Status);

            data.Status = "continue";

            data.Name = "continue";

            var identity = new IdentityContext(context.IdentityContext.Id + "_end", context.IdentityContext.OperationId);

            return context.Send(new Message() { Name = message.Name }, "endendpoint", identity);
        }
    }

    public class StartHandler : AbstractMessageHandlerWithData<Message, Data>
    {
        public override Task HandleWithContextAndData(Message message, MessageContext context, Data data)
        {
            data.Status = "initial";

            Console.WriteLine("message handled by " + GetType().Name + " " + data.Status);

            data.Status = "start";

            data.Name = message.Name;

            var identity = new IdentityContext(context.IdentityContext.Id + "_continue", context.IdentityContext.OperationId);

            return context.Send( new Message() { Name=message.Name }, "continueendpoint", identity);
        }
    }

    public class AlternativeStartHandler : AbstractMessageHandlerWithData<Message, Data>
    {
        public override Task HandleWithContextAndData(Message message, MessageContext context, Data data)
        {
            data.Status = "initial";

            Console.WriteLine("message handled by " + GetType().Name + " " + data.Status);

            data.Status = "start";

            data.Name = message.Name;

            var identity = new IdentityContext(context.IdentityContext.Id + "_continue", context.IdentityContext.OperationId);

            return context.Send(new Message() { Name = message.Name }, "continueendpoint", identity);
        }
    }

    public class Middleware : IMiddlewareAsync<MessageContext>
    {
        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            Console.WriteLine("Start " + GetType().Name);

            await next(context);

            Console.WriteLine("End " + GetType().Name);
        }
    }

    public class FromPublishHandler : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return Task.CompletedTask;
        }
    }

    
    public class ToReplyHandler : AbstractMessageHandler<Message>
    {
        public override async Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            var result = await context.Reply<Message, Message>(new Message() { }, "toreplyendpoint", context.IdentityContext);
        }
    }

    public class FromReplyHandler : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return context.Send(new Message() { Name = "Hi" }, "replyendpoint", context.IdentityContext);
        }
    }

    
    public class ToPublishHandler : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return context.Publish(new Message() { }, "fromqueueendpoint", context.IdentityContext, context.Origin.Key);
        }
    }

    public class TopicListenByOneHandler : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return Task.CompletedTask;
        }
    }

    public class QueueListenByOneHandler : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return Task.CompletedTask;
        }
    }

    public class QueueToRead : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name + " " + message.Name);

            return Task.CompletedTask;
        }
    }

    public class QueueToReadGroup : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name + " " + message.Name+ " groupid "+ context.IdentityContext.GroupId);

            return Task.CompletedTask;
        }
    }

    public class QueueToSend : AbstractMessageHandler<Message>
    {
        public override async Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            for (int i = 0; i < 100; i++)
            {
                await context.Send(new Message() { Name = "Hi"+i }, "queueperformanceendpoint", context.IdentityContext);
            }
        }
    }

    public class QueueListenByOneHandlerWithException : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
            throw new ApplicationException("Error");
        }
    }

    public class HandlingTwoQueuesInOneHandler : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
            return Task.CompletedTask;
        }
    }

    public class QueueListenByTwoAHandlers : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
            return Task.CompletedTask;
        }
    }

    public class QueueListenByTwoBHandlers : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);
            return Task.CompletedTask;
        }
    }

    public class QueueListenSessionSenderHandlers : AbstractMessageHandler<Message>
    {
        public override async Task HandleWithContext(Message message, MessageContext context)
        {
            for (int i = 0; i < 5; i++)
            {
                await context.Publish(new Message() { Name = "Hi 1 " + i }, "sessiontopicendpoint", new IdentityContext($"1-{i}", context.IdentityContext.OperationId, context.IdentityContext.Id, i.ToString()),"X");
                await context.Publish(new Message() { Name = "Hi 2 " + i }, "sessiontopicendpoint", new IdentityContext($"2-{i}", context.IdentityContext.OperationId, context.IdentityContext.Id, i.ToString()), "X");
                await context.Publish(new Message() { Name = "Hi 3 " + i }, "sessiontopicendpoint", new IdentityContext($"3-{i}", context.IdentityContext.OperationId, context.IdentityContext.Id, i.ToString()), "X");
                await context.Publish(new Message() { Name = "Hi 4 " + i }, "sessiontopicendpoint", new IdentityContext($"4-{i}", context.IdentityContext.OperationId, context.IdentityContext.Id, i.ToString()), "X");
            }
        }
    }


    public class Data
    {
        public string Status { get; set; }
        public string Name { get; set; }
    }

    public class Message
    {
        public string Name { get; set; }
    }
}
