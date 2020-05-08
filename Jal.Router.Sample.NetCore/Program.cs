using System;
using Jal.Router.AzureServiceBus.Standard;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.LightInject.Installer;
using Jal.Router.Model;
using LightInject;
using Jal.Router.Extensions;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.Patterns.Impl;
using Jal.Router.Patterns.Interface;
using Jal.ChainOfResponsability;
using System.Threading.Tasks;
using Jal.Router.Newtonsoft.Extensions;
using System.Collections.Generic;
using System.Reflection;
using Jal.Router.Newtonsoft;
using Jal.Router.AzureStorage;
using Microsoft.Extensions.Configuration;
using System.IO;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Jal.Router.Sample.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>()
            .Build();

            var container = new ServiceContainer();
            container.AddRouter( c=>
            {
                c.AddSource<RouterConfigurationSmokeTest>();
                c.AddAzureServiceBus();
                c.AddAzureStorage();
                c.AddNewtonsoft();
                c.AddMessageHandlerAsSingleton<IMessageHandler<Message>, QueueListenByOneHandler>();
            });

            container.Register<IConfiguration>(x=>config, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByTwoAHandlers>(typeof(QueueListenByTwoAHandlers).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByTwoBHandlers>(typeof(QueueListenByTwoBHandlers).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, TopicListenByOneHandler>(typeof(TopicListenByOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, HandlingTwoQueuesInOneHandler>(typeof(HandlingTwoQueuesInOneHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, QueueListenByOneHandlerWithException>(typeof(QueueListenByOneHandlerWithException).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, ToPublishHandler>(typeof(ToPublishHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler<Message>, FromPublishHandler>(typeof(FromPublishHandler).FullName, new PerContainerLifetime());
            container.Register<IAsyncMiddleware<MessageContext>, Middleware>(typeof(Middleware).FullName, new PerContainerLifetime());
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
            var factory = container.GetInstance<IComponentFactoryFacade>();
            var parameter = new FileSystemParameter() { Path = appRoot };
            var messagecontext = MessageContext.CreateFromListen(bus, factory);
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
                .AddAzureServiceBusAsDefaultTransport(new AzureServiceBusChannelConnection() { AutoRenewTimeoutInMinutes = 60 })
                //.UseFileSystemAsTransport(parameter)
                .UseAzureStorageAsStorage(new AzureStorage.AzureStorageParameter(config["storage"]) { SagaTableName = "sagasmoke", MessageTableName = "messagessmoke", TableSufix = DateTime.UtcNow.ToString("yyyyMMdd"), ContainerName = "messages", TableStorageMaxColumnSizeOnKilobytes = 64 })
                //.AddMonitoringTask<HeartBeatLogger>(150)
                //.UseMemoryAsTransport()
                .UseNewtonsoftAsSerializer()
                .AddShutdownTask<ChannelDestructor>()
                //.UseMemoryAsStorage()
                //.AddMonitoringTask<StatisticMonitor>(30, true)
                //.AddMonitoringTask<ListenerRestartMonitor>(60)
                //.AddMonitoringTask<PointToPointChannelMonitor>(60)
                //.EnableEntityStorage()
                //.AddShutdownWatcher<SignTermShutdownWatcher>()
                ;

            //var h = HostBuilder.Create(container, "")
            //    .UseAzureServiceBus(new IRouterConfigurationSource[] { new RouterConfigurationSmokeTest() })
            //    .Build();


            //var facade = container.GetInstance<IEntityStorageGateway>();

            //var messages = facade.GetMessages(new DateTime(2019, 7,9), new DateTime(2019, 7, 10), "queuelistenbyonehandler_handler", new Dictionary<string, string> { { "messagestoragename", "messagessmoke20190709" } }).GetAwaiter().GetResult();

            //await host.Startup();

            //messagecontext.Send(new Message(), Options.Create("queuelistenbyonehandler"));

            //Console.ReadLine();

            //await host.Shutdown();

            host.RunAndBlock(()=> bus.Send(new Message(), Options.Create("queuelistenbyonehandler")));

            //var bus = container.GetInstance<IBus>();

            //var context = new MessageContext(bus);

            //context.Send(new Message() { Name = "Hi" }, "endpoint");

            //Console.ReadLine();
        }
    }

    public class FileRouterConfigurationSmokeTest : AbstractRouterConfigurationSource
    {
        public FileRouterConfigurationSmokeTest(IConfiguration config)
        {
            var connectionstring = config["bus"];

            RegisterOrigin("smoketestapp", "123");

            RegisterHandler("handler1")
            .ToListen(x =>
            {
                x.AddPointToPointChannel("queuea", connectionstring);
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, HandlerQueueA>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
                x.ForMessage<Message>().Use((request, context) =>
                {
                    Console.WriteLine("message handled by handler");
                    return Task.CompletedTask;
                });
            });

            RegisterHandler("handler2")
            .ToListen(x =>
            {
                x.AddSubscriptionToPublishSubscribeChannel("topic1", "subscription1", connectionstring).With(c => c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, HandlerTopic1Subscription1>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler("handler3")
            .ToListen(x =>
            {
                x.AddPointToPointChannel("queue3", connectionstring);
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, HandlerQueue3>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("sendtoqueuea")
                .To(x => x.AddPointToPointChannel(connectionstring, "queuea").With(c => c.CreateIfNotExist()));

            RegisterEndPoint("sendtoqueue1")
                .To(x => x.AddPointToPointChannel(connectionstring, "queue1").With(c => c.CreateIfNotExist()));

            RegisterEndPoint("sendtoqueue2")
                .To(x => x.AddPointToPointChannel(connectionstring, "queue2"));

            ////////////////////////

            RegisterEndPoint("sendtotopic1")
                .To(x => x.AddPublishSubscribeChannel(connectionstring, "topic1").With(c => c.CreateIfNotExist()));

            RegisterEndPoint("sendtoqueue3")
                .To(x => x.AddPointToPointChannel(connectionstring, "queue3").With(c => c.CreateIfNotExist()));
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

        public RouterConfigurationSmokeTest(IConfiguration config)
        {
            var connectionstring = config["bus"];

            RegisterHandler(_sendersessionqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_sendersessionqueue, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenSessionSenderHandlers>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_sessionqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_sessionqueue, connectionstring).With(c => { 
                    c.Partition();
                    c.CreateIfNotExist(new AzureServiceBusChannelProperties() { SessionEnabled = true });
                });
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueToReadPartition>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_sessiontopic + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToTopic(_sessiontopic, _subscription, connectionstring).With(c => { 
                    c.Partition();
                    c.CreateIfNotExist(new AzureServiceBusChannelProperties() { SessionEnabled = true }, "1=1");
                });
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueToReadPartition>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("sessionqueueendpoint")
            .To(x => x.AddQueue(connectionstring, _sessionqueue));

            RegisterEndPoint("sessiontopicendpoint")
            .To(x => x.AddTopic(connectionstring, _sessiontopic).With(c=>c.CreateIfNotExist()));

            RegisterEndPoint("queuelistenbyonehandler")
            .To(x => x.AddQueue(connectionstring, _queuelistenbyonehandler));

            RegisterEndPoint("xxxx")
                .To(x => x.AddTopic(connectionstring, _topiclistenbyonehandler).With(c=>c.CreateIfNotExist()));

            RegisterOrigin("smoketestapp", "123");

            RegisterHandler(_queueperformancetoread + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queueperformancetoread, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueToRead>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queueperformancetosend + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queueperformancetosend, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueToSend>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("queueperformanceendpoint")
            .To(x => x.AddPointToPointChannel(connectionstring, _queueperformancetoread));

            RegisterHandler(_toreplyqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_toreplyqueue, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, ToReplyHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_fromreplyqueue + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_fromreplyqueue, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, FromReplyHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("toreplyendpoint")
            .To<Message>(x => x.AddQueue(connectionstring, _fromreplyqueue).AndWaitReplyFromQueue(_replyqueue, connectionstring));

            RegisterEndPoint("replyendpoint")
            .To(x => x.AddQueue(connectionstring, _replyqueue).With(c=>c.CreateIfNotExist(new AzureServiceBusChannelProperties() { SessionEnabled = true })));

            RegisterHandler(_queuetopublishtopic + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuetopublishtopic, connectionstring).With(c => c.CreateIfNotExist()) ;
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, ToPublishHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_topicpublishedfromqueue + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToTopic(_topicpublishedfromqueue, _subscription, connectionstring).With(c => c.CreateIfNotExist(filter:"1=1"));
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, FromPublishHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterEndPoint("fromqueueendpoint")
            .To(x => x.AddPublishSubscribeChannel(connectionstring, _topicpublishedfromqueue).With(c=>c.CreateIfNotExist()));


            RegisterHandler("handlingtwoqueuesinonehandler_handler")
            .ToListen(x =>
            {
                x.AddQueue(_handlingtwoqueuesinonehandlera, connectionstring).With(c => c.CreateIfNotExist());
                x.AddQueue(_handlingtwoqueuesinonehandlerb, connectionstring).With(c => c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, HandlingTwoQueuesInOneHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queuelistenbyonehandler+"_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandler, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenByOneHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queuelistenbytwohandlers + "_A_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbytwohandlers, connectionstring).With(c => c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenByTwoAHandlers>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queuelistenbytwohandlers + "_B_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbytwohandlers, connectionstring).With(c => c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenByTwoBHandlers>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_topiclistenbyonehandler + "_handler")
            .ToListen(x =>
            {
                x.AddSubscriptionToPublishSubscribeChannel(_topiclistenbyonehandler, _subscription, connectionstring).With(c => c.CreateIfNotExist(filter: "1=1"));
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, TopicListenByOneHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            });

            RegisterHandler(_queuelistenbyonehandlerwithwhen + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithwhen, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenByOneHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).When(x=>x.Headers.ContainsKey("test"));

            RegisterHandler(_queuelistenbyonehandlerwithmiddleware + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithmiddleware, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenByOneHandler>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            }).UseMiddleware(x => x.Add<Middleware>());

            Func<MessageContext, Handler, Task> init = (c, r) => {
                Console.WriteLine("HIiii");
                return Task.CompletedTask;
            };

            RegisterEndPoint(_forwardqueueendpoint)
            .To(x => x.AddPointToPointChannel(connectionstring, _forwardqueue).With(c=>c.CreateIfNotExist()));

            RegisterHandler(_queuelistenbyonehandlerwithexception + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithexception, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenByOneHandlerWithException>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
            })
            .OnError(e=>e.ForwardMessageTo<Message>(_errorqueueendpoint))
            .OnEntry(e=> {
                e.Execute(init);
                e.ForwardMessageTo<Message>(_forwardqueueendpoint);
            });

            Func<MessageContext, Exception, ErrorHandler, Task > func = (c, e, r) => {
                Console.WriteLine("HIiii");
                return Task.CompletedTask;
            }; 

            RegisterHandler(_queuelistenbyonehandlerwithexceptionandretry + "_handler")
            .ToListen(x =>
            {
                x.AddQueue(_queuelistenbyonehandlerwithexceptionandretry, connectionstring).With(c=>c.CreateIfNotExist());
            })
            .With(x =>
            {
                x.ForMessage<Message>().Use<IMessageHandler<Message>, QueueListenByOneHandlerWithException>((request, handler, context) => handler.HandleWithContext(request, context)).When((request, handler, context) => true);
                
            })
            //.OnExceptionRetryFailedMessageTo("retryendpoint", x=>x.For<ApplicationException>()).With(new LinearRetryPolicy(5, 3))
            //.OnEntry(x => x.Use<CustomEntryMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value1" } , { "parameter2", "value2" }, { "parameter3", "value3" } }))
            .OnError(x => x.RetryMessageTo<Message>("retryendpoint", new LinearRetryPolicy(5, 3)).For<ApplicationException>())
            //.OnExit(x => x.Use<CustomExitMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value2" } }))
            //.OnException(x=>x.OfType<ApplicationException>().Do<StoreLocally>(parameter));
            //.OnEntry(x=>x.EnableEntityStorage(true))
            //.OnErrorSendFailedMessageTo(_errorqueueendpoint)
            ;

            RegisterEndPoint(_errorqueueendpoint)
                .To(x => x.AddQueue(connectionstring, _errorqueue).With(c=>c.CreateIfNotExist()));
                //.OnEntry(x => x.Use<CustomEntryMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value1" }, { "parameter2", "value2" }, { "parameter3", "value3" } }))
                //.OnError(x => x.Use<CustomErrorMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value1" }, { "parameter2", "value2" } }))
                //.OnExit(x => x.Use<CustomExitMessageHandler>(new Dictionary<string, object>() { { "parameter1", "value2" } }));

            RegisterEndPoint("retryendpoint")
                .To(x => x.AddQueue(connectionstring, _queuelistenbyonehandlerwithexceptionandretry));

        RegisterSaga<Data>("saga", 
            x => {
                x.RegisterHandler("start")
                .ToListen(y => y.AddQueue(_queuestart, connectionstring).With(c=>c.CreateIfNotExist()))
                .With(y => {
                    y.ForMessage<Message>().Use<IMessageHandlerWithData<Message, Data>, StartHandler>((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "START");
                });

                x.RegisterHandler("alternative")
                .ToListen(y => y.AddQueue(_alternativequeuestart, connectionstring).With(c=>c.CreateIfNotExist()))
                .With(y => {
                    y.ForMessage<Message>().Use<IMessageHandlerWithData<Message, Data>, AlternativeStartHandler>((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "START");
                });
            },
            x =>
            {
                x.RegisterHandler("continue")
                .ToListen(y => y.AddQueue(_queuecontinue, connectionstring).With(c=>c.CreateIfNotExist()))
                .With(y =>
                {
                    y.ForMessage<Message>().Use<IMessageHandlerWithData<Message, Data>, ContinueHandler>((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data), "CONTINUE");
                });
            },
            x =>
            {
                x.RegisterHandler("end")
                .ToListen(y => y.AddQueue(_queueend, connectionstring).With(c=>c.CreateIfNotExist()))
                .With(y =>
                {
                    y.ForMessage<Message>().Use<IMessageHandlerWithData<Message, Data>, EndHandler>((request, handler, context, data) => handler.HandleWithContextAndData(request, context, data),"END");
                });
            }
         );

            RegisterEndPoint("continueendpoint")
            .To(x => x.AddQueue(connectionstring, _queuecontinue));

            RegisterEndPoint("endendpoint")
            .To(x => x.AddQueue(connectionstring, _queueend));

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

            return context.Send(new Message() { Name = message.Name }, "endendpoint", id: $"{Guid.NewGuid()}_toend");
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

            return context.Send( new Message() { Name=message.Name }, "continueendpoint", id: $"{Guid.NewGuid()}_tocontinue");
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

            var tracing = new TracingContext(id: context.TracingContext.Id + "_continue", operationid: context.TracingContext.OperationId);

            return context.Send(new Message() { Name = message.Name }, "continueendpoint", tracing);
        }
    }

    public class Middleware : IAsyncMiddleware<MessageContext>
    {
        public async Task ExecuteAsync(AsyncContext<MessageContext> context, Func<AsyncContext<MessageContext>, Task> next)
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

            var result = await context.Reply<Message, Message>(new Message() { }, "toreplyendpoint");
        }
    }

    public class FromReplyHandler : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return context.Send(new Message() { Name = "Hi" }, "replyendpoint");
        }
    }

    
    public class ToPublishHandler : AbstractMessageHandler<Message>
    {
        public override Task HandleWithContext(Message message, MessageContext context)
        {
            Console.WriteLine("message handled by " + GetType().Name);

            return context.Publish(new Message() { }, "fromqueueendpoint", context.TracingContext, context.Origin.Key);
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
            Console.WriteLine("message handled by " + GetType().Name + " " + message.Name+ " groupid "+ context.TracingContext.PartitionId);

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
                await context.Send(new Message() { Name = "Hi"+i }, "queueperformanceendpoint", context.TracingContext);
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
                await context.Publish(new Message() { Name = "Hi 1 " + i }, "sessiontopicendpoint", new TracingContext(id: $"1-{i}", operationid: context.TracingContext.OperationId, parentid: context.TracingContext.Id, partitionid: i.ToString()),"X");
                await context.Publish(new Message() { Name = "Hi 2 " + i }, "sessiontopicendpoint", new TracingContext(id: $"2-{i}", operationid: context.TracingContext.OperationId, parentid: context.TracingContext.Id, partitionid: i.ToString()), "X");
                await context.Publish(new Message() { Name = "Hi 3 " + i }, "sessiontopicendpoint", new TracingContext(id: $"3-{i}", operationid: context.TracingContext.OperationId, parentid: context.TracingContext.Id, partitionid: i.ToString()), "X");
                await context.Publish(new Message() { Name = "Hi 4 " + i }, "sessiontopicendpoint", new TracingContext(id: $"4-{i}", operationid: context.TracingContext.OperationId, parentid: context.TracingContext.Id, partitionid: i.ToString()), "X");
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
