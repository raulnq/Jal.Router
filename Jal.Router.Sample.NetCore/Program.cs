﻿using System;
using Jal.Locator.LightInject.Installer;
using Jal.Router.AzureServiceBus.Standard.Extensions;
using Jal.Router.AzureServiceBus.Standard.LightInject.Installer;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.LightInject.Installer;
using Jal.Router.Model;
using LightInject;
using Jal.Router.Extensions;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.AzureStorage.LightInject.Installer;
using Jal.ChainOfResponsability.LightInject.Installer;
using Jal.Router.Impl.Patterns;
using Jal.Router.Interface.Patterns;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using System.Threading.Tasks;
using Jal.Router.Newtonsoft.Extensions;
using Jal.Router.Newtonsoft.LightInject.Installer;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;

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
            container.Register<IMessageHandler<Message>, QueueToReadPartition>(typeof(QueueToReadPartition).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenSessionSenderHandlers>(typeof(QueueListenSessionSenderHandlers).FullName, new PerContainerLifetime());

            container.Register<IMessageHandler<Message>, HandlerQueueA>(typeof(HandlerQueueA).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, HandlerTopic1Subscription1>(typeof(HandlerTopic1Subscription1).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, HandlerQueue3>(typeof(HandlerQueue3).FullName, new PerContainerLifetime());
            //var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);//appPathMatcher.Match(path).Value;
            var host = container.GetInstance<IHost>();
            var bus = container.GetInstance<IBus>();
            var parameter = new FileSystemParameter() { Path = appRoot };
            var messagecontext = new MessageContext(bus);
            parameter.AddEndpointHandler("sendtoqueue1", (ms, me) =>
             {
                 //var path = fs.CreateSubscriptionToPublishSubscribeChannelPath(parameter, "connectionstring", "topic1", "subscription1");

                 //fs.CreateFile(path, fn, me);

                 var m = ms.Deserialize<Message>(me.Content);

                 return messagecontext.Send(m, "sendtotopic1");
                 
             });
            parameter.AddEndpointHandler("sendtoqueue2", (ms, me) =>
            {
                var m = ms.Deserialize<Message>(me.Content);

                return messagecontext.Send(m, "sendtoqueue3");
            });
            host.Configuration
                .UseAzureServiceBusAsTransport(new AzureServiceBusParameter() { AutoRenewTimeoutInMinutes = 60, MaxConcurrentCalls=4, MaxConcurrentPartitions=1, TimeoutInSeconds = 60 }, useazureservicemanagemet: false)
                //.UseFileSystemAsTransport(parameter)
                //.UseAzureStorage(new AzureStorage.Model.AzureStorageParameter("") { SagaTableName = "sagasmoke", MessageTableName = "messagessmoke", TableSufix = DateTime.UtcNow.ToString("yyyyMMdd"), ContainerName = "messages", TableStorageMaxColumnSizeOnKilobytes = 64 })
                //.AddMonitoringTask<HeartBeatLogger>(150)
                .UseNewtonsoftAsSerializer()
                //.AddMonitoringTask<ListenerMonitor>(30)
                //.AddMonitoringTask<ListenerRestartMonitor>(60)
                //.AddMonitoringTask<PointToPointChannelMonitor>(60)
                //.EnableEntityStorage()
                .AddShutdownWatcher<SignTermShutdownWatcher>();

            //var facade = container.GetInstance<IEntityStorageGateway>();

            //var messages = facade.GetMessages(new DateTime(2019, 7,9), new DateTime(2019, 7, 10), "queuelistenbyonehandler_handler", new Dictionary<string, string> { { "messagestoragename", "messagessmoke20190709" } }).GetAwaiter().GetResult();

            

            host.RunAndBlock(/*()=> messagecontext.Send(new Message(), "sendtoqueuea")*/);

            //var bus = container.GetInstance<IBus>();

            //var context = new MessageContext(bus);

            //context.Send(new Message() { Name = "Hi" }, "endpoint");

            //Console.ReadLine();
        }
    }

    public class FileRouterConfigurationSmokeTest : AbstractRouterConfigurationSource
    {
        public FileRouterConfigurationSmokeTest()
        {
            var connectionstring = "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=";

            RegisterOrigin("smoketestapp", "123");

            RegisterHandler("handler1")
            .ToListen(x =>
            {
                x.AddPointToPointChannel("queuea", connectionstring);
            })
            .ForMessage<Message>().Use<IMessageHandler<Message>, HandlerQueueA>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler("handler2")
            .ToListen(x =>
            {
                x.AddSubscriptionToPublishSubscribeChannel("topic1", "subscription1", connectionstring);
            })
            .ForMessage<Message>().Use<IMessageHandler<Message>, HandlerTopic1Subscription1>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler("handler3")
            .ToListen(x =>
            {
                x.AddPointToPointChannel("queue3", connectionstring);
            })
            .ForMessage<Message>().Use<IMessageHandler<Message>, HandlerQueue3>(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("sendtoqueuea")
                .ForMessage<Message>()
                .To(x => x.AddPointToPointChannel(connectionstring, "queuea"));

            RegisterEndPoint("sendtoqueue1")
                .ForMessage<Message>()
                .To(x => x.AddPointToPointChannel(connectionstring, "queue1"));

            RegisterEndPoint("sendtoqueue2")
                .ForMessage<Message>()
                .To(x => x.AddPointToPointChannel(connectionstring, "queue2"));

            RegisterPointToPointChannel("queuea", connectionstring, new Dictionary<string, string>());

            RegisterPointToPointChannel("queue3", connectionstring, new Dictionary<string, string>());

            RegisterSubscriptionToPublishSubscribeChannel("subscription1", "topic1", connectionstring, new Dictionary<string, string>());

            ////////////////////////

            RegisterEndPoint("sendtotopic1")
                .ForMessage<Message>()
                .To(x => x.AddPublishSubscribeChannel(connectionstring, "topic1"));

            RegisterEndPoint("sendtoqueue3")
                .ForMessage<Message>()
                .To(x => x.AddPointToPointChannel(connectionstring, "queue3"));

            RegisterPublishSubscribeChannel("topic1", connectionstring, new Dictionary<string, string>());

            RegisterPointToPointChannel("queue1", connectionstring, new Dictionary<string, string>());
        }
    }

    public class HandlerQueueA : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return context.Send(new Message() { }, "sendtoqueue1");
        }
    }

    public class HandlerTopic1Subscription1 : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return context.Send(new Message() { }, "sendtoqueue2");
        }
    }

    public class HandlerQueue3 : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            //return context.Send(new Message() { }, "sendtoqueueb");
            return Task.CompletedTask;
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

            RegisterPartition("xx").ForQueue(_sessionqueue, config.ConnectionString).Until(x => false);

            RegisterPartition("yy").ForSubscriptionToTopic(_sessiontopic, _subscription, config.ConnectionString).Until(x => false);


            RegisterHandler(_sendersessionqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_sendersessionqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenSessionSenderHandlers >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_sessionqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_sessionqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Use<IMessageHandler<Message>, QueueToReadPartition >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_sessiontopic + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToTopic(_sessiontopic, _subscription, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , QueueToReadPartition >(x =>
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

            this.RegisterQueue(new AzureServiceBusQueue(_sendersessionqueue) { }, config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_sessionqueue) { SessionEnabled = true }, config.ConnectionString);

            this.RegisterTopic(new AzureServiceBusTopic(_sessiontopic) { }, config.ConnectionString);

            this.RegisterSubscriptionToTopic(new AzureServiceBusSubscriptionToTopic(_subscription, _sessiontopic) { SessionEnabled = true }, config.ConnectionString, "1=1");

            this.RegisterQueue(new AzureServiceBusQueue(_queueperformancetosend), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queueperformancetoread), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_replyqueue) { SessionEnabled = true }, config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_fromreplyqueue), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_toreplyqueue), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandler), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queuetopublishtopic), config.ConnectionString);

            this.RegisterTopic(new AzureServiceBusTopic(_topicpublishedfromqueue), config.ConnectionString);

            this.RegisterSubscriptionToTopic(new AzureServiceBusSubscriptionToTopic(_subscription, _topicpublishedfromqueue), config.ConnectionString, "1=1");

            this.RegisterQueue(new AzureServiceBusQueue(_queuestart), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_alternativequeuestart), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queuecontinue), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queueend), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_errorqueue), config.ConnectionString);
            
            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandlerwithexceptionandretry), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandlerwithexception), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_forwardqueue), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandlerwithmiddleware), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbyonehandlerwithwhen), config.ConnectionString);
            
            this.RegisterQueue(new AzureServiceBusQueue(_handlingtwoqueuesinonehandlera), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_handlingtwoqueuesinonehandlerb), config.ConnectionString);

            this.RegisterQueue(new AzureServiceBusQueue(_queuelistenbytwohandlers), config.ConnectionString);

            this.RegisterTopic(new AzureServiceBusTopic(_topiclistenbyonehandler),  config.ConnectionString);

            this.RegisterSubscriptionToTopic(new AzureServiceBusSubscriptionToTopic(_subscription, _topiclistenbyonehandler), config.ConnectionString, "1=1");


            RegisterHandler(_queueperformancetoread + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queueperformancetoread, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , QueueToRead >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queueperformancetosend + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queueperformancetosend, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message >, QueueToSend >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("queueperformanceendpoint")
            .ForMessage<Message>()
            .To(x => x.AddPointToPointChannel(config.ConnectionString, _queueperformancetoread));

            RegisterHandler(_toreplyqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_toreplyqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , ToReplyHandler >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_fromreplyqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_fromreplyqueue, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , FromReplyHandler >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("toreplyendpoint")
            .ForMessage<Message>()
            .To<Message>(x => x.AddQueue(config.ConnectionString, _fromreplyqueue).AndWaitReplyFromQueue(_replyqueue, config.ConnectionString));

            RegisterEndPoint("replyendpoint")
            .ForMessage<Message>()
            .To(x => x.AddQueue(config.ConnectionString, _replyqueue));

            RegisterHandler(_queuetopublishtopic + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuetopublishtopic, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > ,ToPublishHandler >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_topicpublishedfromqueue + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToTopic(_topicpublishedfromqueue, _subscription, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , FromPublishHandler >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("fromqueueendpoint")
            .ForMessage<Message>()
            .To(x => x.AddPublishSubscribeChannel(config.ConnectionString, _topicpublishedfromqueue));


            RegisterHandler("handlingtwoqueuesinonehandler_handler")
            .ToListen(x =>
            {
                x.AddQueue(_handlingtwoqueuesinonehandlera, config.ConnectionString);
                x.AddQueue(_handlingtwoqueuesinonehandlerb, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , HandlingTwoQueuesInOneHandler >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queuelistenbyonehandler+"_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandler, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , QueueListenByOneHandler >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queuelistenbytwohandlers + "_A_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbytwohandlers, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , QueueListenByTwoAHandlers >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queuelistenbytwohandlers + "_B_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbytwohandlers, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > ,QueueListenByTwoBHandlers >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_topiclistenbyonehandler + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToPublishSubscribeChannel(_topiclistenbyonehandler, _subscription, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > ,TopicListenByOneHandler >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queuelistenbyonehandlerwithwhen + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithwhen, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > ,QueueListenByOneHandler >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).When(x=>x.Headers.ContainsKey("test"));

            RegisterHandler(_queuelistenbyonehandlerwithmiddleware + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithmiddleware, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > ,QueueListenByOneHandler >(x =>
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

            RegisterHandler(_queuelistenbyonehandlerwithexception + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithexception, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > ,QueueListenByOneHandlerWithException >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            })
            .OnError(e=>e.ForwardMessageTo(_errorqueueendpoint))
            .OnEntry(e=> {
                e.Execute(init);
                e.ForwardMessageTo(_forwardqueueendpoint);
            });

            Func<MessageContext, Exception, ErrorHandler, Task > func = (c, e, r) => {
                Console.WriteLine("HIiii");
                return Task.CompletedTask;
            }; 

            RegisterHandler(_queuelistenbyonehandlerwithexceptionandretry + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithexceptionandretry, config.ConnectionString);
            })
            .ForMessage<Message>().Use< IMessageHandler < Message > , QueueListenByOneHandlerWithException >(x =>
            {
                x.With((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            })
            //.OnExceptionRetryFailedMessageTo("retryendpoint", x=>x.For<ApplicationException>()).With(new LinearRetryPolicy(5, 3))
            //.OnEntry(x => x.Use<CustomEntryMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value1" } , { "parameter2", "value2" }, { "parameter3", "value3" } }))
            //.OnError(x => x.Use<CustomErrorMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value1" }, { "parameter2", "value2" } }).For<ApplicationException>())
            //.OnExit(x => x.Use<CustomExitMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value2" } }))
            //OnException(x=>x.OfType<ApplicationException>().Do<StoreLocally>(parameter));
            //.OnEntry(x=>x.EnableEntityStorage(true))
            //.OnErrorSendFailedMessageTo(_errorqueueendpoint)
            ;

            RegisterEndPoint(_errorqueueendpoint)
                .ForMessage<Message>()
                .To(x => x.AddQueue(config.ConnectionString, _errorqueue));
                //.OnEntry(x => x.Use<CustomEntryMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value1" }, { "parameter2", "value2" }, { "parameter3", "value3" } }))
                //.OnError(x => x.Use<CustomErrorMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value1" }, { "parameter2", "value2" } }))
                //.OnExit(x => x.Use<CustomExitMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value2" } }));

            RegisterEndPoint("retryendpoint")
                .ForMessage<Message>()
                .To(x => x.AddQueue(config.ConnectionString, _queuelistenbyonehandlerwithexceptionandretry));

        RegisterSaga<Data>("saga", 
            x => {
                x.RegisterHandler("start")
                .ToListen(y => y.AddQueue(_queuestart, config.ConnectionString))
                .ForMessage<Message>().Use< IMessageHandlerWithData < Message, Data > , StartHandler >(y => {
                    y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "START");
                });

                x.RegisterHandler("alternative")
                .ToListen(y => y.AddQueue(_alternativequeuestart, config.ConnectionString))
                .ForMessage<Message>().Use< IMessageHandlerWithData < Message, Data > , AlternativeStartHandler >(y => {
                    y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "START");
                });
            },
            x =>
            {
                x.RegisterHandler("continue")
                .ToListen(y => y.AddQueue(_queuecontinue, config.ConnectionString))
                .ForMessage<Message>().Use< IMessageHandlerWithData < Message, Data > , ContinueHandler >(y =>
                {
                    y.With((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "CONTINUE");
                });
            },
            x =>
            {
                x.RegisterHandler("end")
                .ToListen(y => y.AddQueue(_queueend, config.ConnectionString))
                .ForMessage<Message >().Use<IMessageHandlerWithData<Message, Data>, EndHandler>(y =>
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

            var identity = new IdentityContext(id: context.IdentityContext.Id + "_end", operationid: context.IdentityContext.OperationId);

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

            var identity = new IdentityContext(id: context.IdentityContext.Id + "_continue", operationid: context.IdentityContext.OperationId);

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

            var identity = new IdentityContext(id: context.IdentityContext.Id + "_continue", operationid: context.IdentityContext.OperationId);

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

    public class QueueToReadPartition : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name + " " + message.Name+ " groupid "+ context.IdentityContext.PartitionId);

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
                await context.Publish(new Message() { Name = "Hi 1 " + i }, "sessiontopicendpoint", new IdentityContext(id: $"1-{i}", operationid: context.IdentityContext.OperationId, parentid: context.IdentityContext.Id, partitionid: i.ToString()),"X");
                await context.Publish(new Message() { Name = "Hi 2 " + i }, "sessiontopicendpoint", new IdentityContext(id: $"2-{i}", operationid: context.IdentityContext.OperationId, parentid: context.IdentityContext.Id, partitionid: i.ToString()), "X");
                await context.Publish(new Message() { Name = "Hi 3 " + i }, "sessiontopicendpoint", new IdentityContext(id: $"3-{i}", operationid: context.IdentityContext.OperationId, parentid: context.IdentityContext.Id, partitionid: i.ToString()), "X");
                await context.Publish(new Message() { Name = "Hi 4 " + i }, "sessiontopicendpoint", new IdentityContext(id: $"4-{i}", operationid: context.IdentityContext.OperationId, parentid: context.IdentityContext.Id, partitionid: i.ToString()), "X");
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
