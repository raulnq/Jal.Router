# Jal.Router
Just another library to route in/out messages

## How to use?

### Castle Windsor Integration

Note: The Jal.Locator.CastleWindsor and Jal.Finder library are needed

Setup the Jal.Finder library

	var directory = AppDomain.CurrentDomain.BaseDirectory;

	var finder = AssemblyFinder.Builder.UsePath(directory).Create;

    var assemblies = finder.GetAssembliesTagged<AssemblyTagAttribute>();

Setup the Castle Windsor container

	var container = new WindsorContainer();

	container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

Install the Jal.Locator.CastleWindsor library

	container.Install(new ServiceLocatorInstaller());

Install the Jal.Router library, use the RouterInstaller class included

    container.Install(new RouterInstaller(assemblies));

Create a Handler interface and class

    public class MessageHandler : IMessageHandler<Message>
    {
        public void Handle(Message message)
        {
            Console.WriteLine("Sender"+ message.Name);
        }
    }

    public interface IMessageHandler<in T>
    {
        void Handle(T message);
    }

Create a class to setup the Jal.Route library

    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<MessageHandler>(x =>
            {
                x.With(((request, handler) => handler.Handle(request)));
            });
        }
    }

Tag the assembly container of the router configuration source classes in order to be read by the library

    [assembly: AssemblyTag]
	
Resolve an instance of the interface IRouter

	var router = container.Resolve<IRouter>();

Use the Router class

    var message = new Message();

    router.Route<Message>(message);

### Azure Service Bus Brokered Message Routing

Note: The Jal.Router.ServiceBus library is needed

Install the Jal.Router library, use the BrokeredMessageRouterInstaller class included

    container.Install(new BrokeredMessageRouterInstaller());

Create a Handler interface and class. The InboundMessageContext class could be used as a parameter, this class will contain useful data from the orginal brokered message.

    public class MessageHandler : IMessageHandler<Message>
    {
        public void Handle(Message message, InboundMessageContext context)
        {
            Console.WriteLine("Sender"+ message.Name);
        }
    }

    public interface IMessageHandler<in T>
    {
        void Handle(T message, InboundMessageContext context);
    }

Create a class to setup the library

    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<MessageHandler>(x =>
            {
                x.With<InboundMessageContext>(((request, handler, context) => handler.Handle(request, context)));
            });
        }
    }

Resolve an instance of the interface IBrokeredMessageRouter

	var router = container.Resolve<IBrokeredMessageRouter>();

Use the BrokeredMessageRouter class to handle brokered messages from a queue or topic. The message should be serialized as string from the origin.

    var brokeredmessage = new BrokeredMessage(@"{""Name"":""Test""}");

    router.Route<Message>(brokeredmessage);

Register an endpoint in the setup class

    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<MessageHandler>(x =>
            {
                x.With<InboundMessageContext>(((request, handler, context) => handler.Handle(request, context)));
            });

            RegisterEndPoint<AppSettingEndPointValueSettingFinder>()
                .ForMessage<Message>()
                .From(x => x.Find("from"))
                .To(x => x.Find("toconnectionstring"), x => x.Find("topath"))
                .ReplyTo(x => x.Find("replytoconnectionstring"), x => x.Find("replytopath"));
        }
    }

Resolve an instance of the interface IBus

	var bus = container.Resolve<IBus>();

Use the BrokeredMessageBus class to send brokered messages to a queue

    var message = new Message();

    _bus.Send(message, new Options());

Use the BrokeredMessageBus class to reply brokered messages to a queue

    var replymessage = new Message();

    _bus.ReplyTo(replymessage, existingcontext);