# Jal.Router
Just another library to route in/out messages
## How to use?
On asynchronous scenarios is needed to send messages (commands) to some app and then wait for the results (events).
The following case will explain us how this library helps us to achieve this goal.
1. The "App A" sends the "Transfer" command to the "App B".
2. The "App B" routes the "Transfer" to the corresponding handler method.
3. The "App B" publishs the "Transferred" event as response of the "Transfer" command.
4. The "App A" receives the "Transferred" event and ends the process.
Note: The current example will use Azure Service Bus as messaging service.
### Sending the message from "App A" to "App B" ("App A")

The "Sender" class will be on charge to send the message using the "IBus" interface.
```
public class Sender
{
	private readonly IBus _bus;

	public Sender(IBus bus)
	{
		_bus = bus;
	}

	public void Send(Transfer transfer)
	{
		_bus.Send(transfer, new Options() {Id = "Some meaningful value"});
	}
}
```
The "Send" method could receive two parameters, the first one is the content of the message and the second one is an instance of the "Options" class that has the following:
* Id (Optional), The value that will be used to identify the message by the concrete messaging service.
* EndPointName (Optional), If we have more than one enpoint for the current message type and just one of them needs to be used here is the place to put the name.
* Version (Optional), Current version of the message (by default is "1")
* ScheduledEnqueueDateTimeUtc (Optional), If we want to defer the delivery of the message to some time in the future.
* Headers (Optional), If some extra properties are needed those can be put here.
The interface "IBus" will send the message based on the configuration below.
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
* RegisterEndPoint, This method starts the registration of a endpoint.
* ForMessage&lt;TMessage&gt;, The TMessage parameter indicates the type of message that could use this endpoint.
* To&lt;TConnectionStringValueSettingFinder, TPathValueSettingFinder&gt;, This method allows us get the information about the connection string and path where our message will be sent. 
The TConnectionStringValueSettingFinder and TPathValueSettingFinder parameters are concrete implementations of the "IValueSettingFinder" interface. 
Currently there are two implementations of this inteface: "ConnectionStringValueSettingFinder" (to have access to the connection string section of the config file) and "AppSettingValueSettingFinder" (to have access to the app settings section of the config file).
* RegisterOrigin, This method will register the name of the app and the unique id to identified it.
### Routing the message on the "App B" ("App B")
To receive the message in the "App B" we are using the Azure Webjob SDK to implement the "Listener" class.
```
public class Listener
{
	private readonly IRouter<BrokeredMessage> _router;

	public Listener(IRouter<BrokeredMessage> router)
	{
		_router = router;
	}

	public void Listen([ServiceBusTrigger("appbqueue")] BrokeredMessage message)
	{
		_router.Route<Transfer>(message);
	}
}
```
When the message arrives the "Route" method dispatch it to the corresponding handler method.
```
public class TransferMessageHandler : IMessageHandler<Transfer>
{
	public void Handle(Transfer transfer, InboundMessageContext context)
	{
	//Do something
	}
}
```
In order to achieve that we need to setup the routing logic in the following class.
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
Let's see every method used in the class above.
* RegisterRoute, allows us to start the creation of a new route handled by the interface specified in the generic parameter.
* ForMessage, indicates the object type inside the message to be routed.
* ToBeHandledBy, This method will tell us the concrete class on charge to handle the message and how to handle it (you can add as many ways as you want here).
Coming back to the handler class, apart of the Transfer message, you can have access to the InboundMessageContext class that contains the following properties:
* Id, Id of the message.
* Origin.Name, Name of the app that send us the message.
* Origin.Key, Unique id of the app that send us the message.
* Version, Version of the message.
* Headers, Non standard properties of the message.
* RetryCount, How many times the message was retried.
* LastRetry, It is true if we are in the last retry of the current message.
### Publishing the message from "App B" to "App A" (App B)
Now is time to return a message back to the sender app, to do that we need to modify the handler class.
```
public class TransferMessageHandler : IMessageHandler<Transfer>
{
	public void Handle(Transfer transfer, InboundMessageContext context)
	{
	//Do something
	_bus.Publish(transferred, new Origin{ Key = context.Origin.Key }, new Options {Id = context.Id});
	}
}
```
The "Publish" method has the same parameters of the "Send" method the difference lies on the kind of messages that they are delivering. 
The "Send" method is used to send commands and the "Publish" method is used to send events. 
Behind scenes the "Send" method uses as target a Queue and the "Publish" uses a Topic.
A new parameter is needed here: "Origin", in order to sign this message as result from the "App A". 
Now to end with publishing we need to register the endpoint on the configuration class.
```
public class RouterConfigurationSource : AbstractRouterConfigurationSource
{
	public RouterConfigurationSource()
	{
		RegisterRoute<IMessageHandler<Transfer>>().ForMessage<Transfer>().ToBeHandledBy<TransferMessageHandler>(x =>
		{
			x.With<InboundMessageContext>(((transfer, handler, context) => handler.Handle(transfer, context)));
		});

		RegisterEndPoint().ForMessage<Transferred>().To<ConnectionStringValueSettingFinder, AppSettingValueSettingFinder>(x => x.Find("AzureWebJobsAppB"), x => x.Find("appbtopic"));

		RegisterOrigin("App B", "9993E555-Q8F4-1111-0A1C-FE7A6FOO71EE");
	}
}
```
### Routing the message from "App B" to "App A" (App A)
We start adding a "Listener" class to receive the message.
```
public class Listener
{
	private readonly IRouter<BrokeredMessage> _router;

	public Listener(IRouter<BrokeredMessage> router)
	{
		_router = router;
	}

	public void ListenTransferredFromSource([ServiceBusAccount("AppB")][ServiceBusTrigger("appbtopic", "appasubscription")]BrokeredMessage message)
	{
		_router.Route<Transferred>(message);
	}
}
```
Then is time to create our handler class. Notice that now we need to handle two possible scenarios, one if we got a successful processing and the other if it was a failure.
```
public class TransferreCompensableMessageHandler : ICompensableMessageHandler<Transferred>
{
	public void Handle(Transferred message, InboundMessageContext context)
	{
	//Do something when the response is successful
	}

	public bool IsSuccessful(Transferred message)
	{
		return message.IsSuccess;
	}

	public void Compensate(Transferred message, InboundMessageContext context)
	{
	//Do something when the response is a failure
	}
}
```
And then our routing setup.
```
public class RouterConfigurationSource : AbstractRouterConfigurationSource
{
	public RouterConfigurationSource()
	{
		RegisterRoute<IMessageHandler<Transfer>>().ForMessage<Transfer>().ToBeHandledBy<TransferMessageHandler>(x =>
		{
			x.With<InboundMessageContext>(((transfer, handler, context) => handler.Handle(transfer, context)));
		});

		RegisterEndPoint().ForMessage<Transferred>().To<ConnectionStringValueSettingFinder, AppSettingValueSettingFinder>(x => x.Find("AzureWebJobsAppB"), x => x.Find("appbtopic"));

		RegisterOrigin("App B", "9993E555-Q8F4-1111-0A1C-FE7A6FOO71EE");
	}
}
```
### Retry
What happens if during the processing of the "Transfer" message we got a transient error?, Do we have to return the error message or repeat the message?.
```
public class TransferMessageHandler : IMessageHandler<Transfer>
{
	private readonly IBus _bus;

	public TransferMessageHandler(IBus bus)
	{
		_bus = bus;
	}

	public void Handle(Transfer transfer, InboundMessageContext context)
	{
		//Do something
		if (!transferred.IsSuccess && !context.LastRetry)
		{
			throw new ApplicationException();
		}

		_bus.Publish(transferred, new Origin{ Key = context.Origin.Key }, new Options {Id = context.Id});
	}
}
```
```
public RouterConfigurationSource()
{
		RegisterRoute<IMessageHandler<Transfer>>().ForMessage<Transfer>().ToBeHandledBy<TransferMessageHandler>(x =>
		{
			x.With<InboundMessageContext>(((transfer, handler, context) => handler.Handle(transfer, context))).Retry<ApplicationException>().Using<AppSettingValueSettingFinder>(y => new LinearRetryPolicy(600, 3));
		});

		RegisterEndPoint().ForMessage<Transferred>().To<ConnectionStringValueSettingFinder, AppSettingValueSettingFinder>(x => x.Find("AzureWebJobsAppB"), x => x.Find("appbtopic"));

		RegisterOrigin("App B", "9993E555-Q8F4-1111-0A1C-FE7A6FOO71EE");
}
```
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
Tag the assembly container of the router configuration source classes in order to be read by the library
```
[assembly: AssemblyTag]
```	
Resolve an instance of the interface "IRouter" or "IBus"
```
var router = container.Resolve<IRouter<BrokeredMessage>>();

var bus = container.Resolve<IBus>();
```