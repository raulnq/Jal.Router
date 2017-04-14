# Jal.Router
Just another library to route in/out messages

## How to use?

On asynchronous scenarios is often needed to send a message (command) to some queue and then wait for a message (event) with the result of the previous process.
The following case will help us to understand how this library helps us to achieve this goal.

1. The app "A" wants to send a Transfer command to the app "B".
2. The app "B" will publish a Transferred event with the result of the processing.
3. The app "A" will receive the Transferred event and will end the process.

Note: The current explanation will use Azure Service Bus as messaging service.

### Send

We are going to start with the App "A" implementing the "Sender" class.
```
public class Sender
{
    private readonly IBus _bus;

    public Sender(IBus bus)
    {
        _router_bus = bus;
    }

    public void Send(Transfer transfer)
    {
        _bus.Send(transfer, new Options() {Id = "Some meaningful value"});
    }
}
```
The method Send receive two parameters, first the message to be send and an instance of the class Options that has these properties:
* Id, The value that will be used to identify the message by the concrete messaging service.
* Correlation, 
* EndPointName, If we have more than one enpoint for the current message type and just one of them needs to be used here is the place to put the name.
* Version, Current version of the message (by default is "1")
* ScheduledEnqueueDateTimeUtc, If we want to defer the delibery of the message to some time in the future.
* Headers, If some extra propereties are needed those can be put here.


The interface IBus will send the message based on the configuration below:
```
public class RouterConfigurationSource : AbstractRouterConfigurationSource
{
    public RouterConfigurationSource()
    {
        RegisterEndPoint().ForMessage<Transfer>().To<ConnectionStringValueSettingFinder, AppSettingValueSettingFinder>(x => x.Find("AzureWebJobsAppB"), x => x.Find("appbqueue"));

		 RegisterOrigin("App A", "59D3EDDD-F8F4-4895-9A1C-FE7A6F9B71EE");
	}
}
```
* RegisterEndPoint, This method starts the registration of a target endpoint where the message will be sent.
* ForMessage&lt;TMessage&gt;, The TMessage parameter indicates the type of message that could use this endpoint.
* To&lt;TConnectionStringValueSettingFinder, TPathValueSettingFinder&gt;, This method allows us get the information about the connection string and path where our message will be sent. 
The TConnectionStringValueSettingFinder and TPathValueSettingFinder parameters should be concrete implementations of the IValueSettingFinder interface. 
Currently there are implementations of this inteface: ConnectionStringValueSettingFinder (to have access to the connection string section of the config file) and AppSettingValueSettingFinder (to have access to the app settings section of the config file).
* RegisterOrigin, This method will register the name of the app and the unique id to identified it.


### Routing

When a messages arrives (event or command) this feature allows us to process it based on a routing configuration.
For instance, if we want to process a Transfer command by the following class:
```
public class TransferMessageHandler : IMessageHandler<Transfer>
{
    public void Handle(Transfer transfer)
    {
	//Do something
    }
}
```
When the message is listen by the standard Azure Webjob SDK
```
public class Listener
{
    public void Listen([ServiceBusTrigger("somequeue")] BrokeredMessage message)
    {
	//Route message somewhere
    }
}
```

In order to achieve that we need to use the class IRouter&lt;T&gt; where T will be the BrokeredMessage class.
The next step is use the method Route&lt;TContent&gt;> where TContent will be the object under the brokered message.
```
public class Listener
{
    private readonly IRouter<BrokeredMessage> _router;

    public Listener(IRouter<BrokeredMessage> router)
    {
        _router = router;
    }

    public void Listen([ServiceBusTrigger("somequeue")] BrokeredMessage message)
    {
        _router.Route<Transfer>(message);
    }
}
```
The last step is setup the routing itself.
```
public class RouterConfigurationSource : AbstractRouterConfigurationSource
{
    public RouterConfigurationSource()
    {
        RegisterRoute<IMessageHandler<Transfer>>().ForMessage<Transfer>().ToBeHandledBy<TransferMessageHandler>(x =>
        {
            x.With(((transfer, handler) => handler.Handle(transfer)));
        });
    }
}
```
Let's see every method used in the class above:

* RegisterRoute, allows us to start the creation of a new route handled by the interface specified in the generic parameter.
* ForMessage, indicates the object under the brokered message to be routed
* ToBeHandledBy, This method will tell us the concrete class on charge to handle the message how and to handle this message usign this class (you can add as many ways as you want there).

Suppose that now your handler has two methods to handle the message in different ways under different circumstances.
```
public class TransferMessageHandler : IMessageHandler<Transfer>
{
    public void HandleWay1(Transfer transfer)
    {
	//Do something
    }

    public bool IsWay1(Transfer transfer)
    {
	//Do something
    }

    public void HandleWay2(Transfer transfer)
    {
	//Do something
    }
}
```

You can have the following configuration to handle this scenario.
```
public class RouterConfigurationSource : AbstractRouterConfigurationSource
{
    public RouterConfigurationSource()
    {
        RegisterRoute<IMessageHandler<Transfer>>().ForMessage<Transfer>().ToBeHandledBy<TransferMessageHandler>(x =>
        {
            x.With(((transfer, handler) => handler.HandleWay1(transfer))).When(((message, handler) => handler.IsWay1(transfer)));
            x.With(((transfer, handler) => handler.HandleWay2(transfer))).When(((message, handler) => !handler.IsWay1(transfer)));
        });
    }
}
```
But in the most common scenarios you want to have access as well to the metadata of the original message. In order do that, the With and When method have a generic form to add a extra parameter to the equation.
```
public class TransferMessageHandler : IMessageHandler<Transfer>
{
    public void Handle(Transfer transfer, InboundMessageContext context)
    {
	//Do something
    }
}
```
```
public class RouterConfigurationSource : AbstractRouterConfigurationSource
{
    public RouterConfigurationSource()
    {
        RegisterRoute<IMessageHandler<Transfer>>().ForMessage<Transfer>().ToBeHandledBy<TransferMessageHandler>(x =>
        {
            x.With<InboundMessageContext>(((transfer, handler, context) => handler.Handle(transfer, context)));
        });
    }
}
```
Now you can have access to the Inbound MessageContext class that contains the following properties:
* Id, Id of the message.
* From, Whom send us the message.
* Origin, Unique Id of Whom send us the message.
* Version, Version of the message.
* Headers, Non standard properties of the message.
* RetryCount, How many times the messages was retried.
* LastRetry, It is true if we are in the last retry of the current message.

### Publish

### Retry

### Castle Windsor And Azure Service Bus Integration

Note: The Jal.Locator.CastleWindsor and Jal.Finder library are needed

Setup the Jal.Finder library
```
var directory = AppDomain.CurrentDomain.BaseDirectory;

var finder = AssemblyFinder.Builder.UsePath(directory).Create;

var assemblies = finder.GetAssembliesTagged<AssemblyTagAttribute>();
```
Setup the Castle Windsor container
```
var container = new WindsorContainer();

container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
```
Install the Jal.Locator.CastleWindsor library
```
container.Install(new ServiceLocatorInstaller());
```
Install the Jal.Router and Jal.Router.AzureServiceBus library.
```
container.Install(new AzureServiceBusRouterInstaller());
container.Install(new RouterLoggerInstaller());
```
Create a Handler interface and class
```
public class MessageHandler : IMessageHandler<Message>
{
	public void Handle(Message message, InboundMessageContext context)
	{
		Console.WriteLine("Message:"+ message.Name);
	}
}

public interface IMessageHandler<in T>
{
	void Handle(T message, InboundMessageContext context);
}
```
Create a class to setup the Jal.Route library
```
public class RouterConfigurationSource : AbstractRouterConfigurationSource
{
	public RouterConfigurationSource()
	{
		RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<MessageHandler>(x =>
		{
			x.With<InboundMessageContext>(((message, handler, context) => handler.Handle(message, context)));
		});
	}
}
```
Tag the assembly container of the router configuration source classes in order to be read by the library
```
[assembly: AssemblyTag]
```	
Resolve an instance of the interface IRouter
```
var router = container.Resolve<IRouter<BrokeredMessage>>();
```
Use the Router class
```
var message = new Message();

router.Route<Message>(message);
```