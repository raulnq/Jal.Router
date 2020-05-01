# Jal.Router [![Build status](https://ci.appveyor.com/api/projects/status/gunc3edkhwwl51ge/branch/master?svg=true)](https://ci.appveyor.com/project/raulnq/jal-router/branch/master)
Just another library to do asynchronous communication between applications (message-based).
## Getting Started
Create a console app and install the following NuGet package:
* [Jal.Router.Azure.Standard.LightInject.Installer.All](https://www.nuget.org/packages/Jal.Router.Azure.Standard.LightInject.Installer.All/)

Create a message class
```csharp
public class Message
{
    public string Text { get; set; }
}
```
Create a class to handle the message
```csharp
public interface IMessageHandler
{
    Task Handle(Message message);
}

public class MessageHandler : IMessageHandler
{
    public Task Handle(Message message)
    {
        Console.WriteLine("Hello world!!");
        return Task.CompletedTask;
    }
}
```
Create a router configuration class
```csharp
public class RouterConfigurationSource : AbstractRouterConfigurationSource
{
    public RouterConfigurationSource()
    {
        RegisterHandler("handler")
            .ToListen(x=>x.AddPointToPointChannel("path","connectionstring"))
            .With(x =>
            {
                x.ForMessage<Message>().Use<MessageHandler, MessageHandler>((request, handler, context) => handler.Handle(request));
            });  
            
        RegisterEndPoint("endpoint")
            .To(x => x.AddPointToPointChannel("connectionstring", "path"));
    }
}
```
And on the Program.cs file setup the host
```csharp

var container = new ServiceContainer();

container.AddRouter(c=>{
    c.AddSource<RouterConfigurationSource>();
    c.AddNewtonsoft();
    c.AddMessageHandlerAsSingleton<IMessageHandler, MessageHandler>();
});

var bus = container.GetInstance<IBus>();

var host = container.GetInstance<IHost>();

host.Configuration
    .AddMemoryAsTransport()
    .UseNewtonsoftAsSerializer()
    .UseMemoryAsStorage();

await host.Startup();

await bus.Send(new Message(), Options.Create("endpoint"));

Console.ReadLine();

await host.Shutdown();
```
## Documentation
Documentation can be found on the project [Wiki](https://github.com/raulnq/Jal.Router/wiki/10.-Home).
## Contributing
Contributions, issues and feature requests are welcome.
Feel free to check [issues page](https://github.com/raulnq/Jal.Router/issues) if you want to contribute.
[Check the contributing guide](https://github.com/raulnq/Jal.Router/blob/master/CONTRIBUTING.md).
## Versioning
We use [SemVer](https://semver.org/) for versioning. For the versions available, [see the tags on this repository](https://github.com/raulnq/Jal.Router/tags).
## License
Jal.Router is licensed under the [Apache2 license](https://github.com/raulnq/Jal.Router/blob/master/LICENSE).
## Authors
* Raul Naupari (raulnq@gmail.com)
## RoadMap
* Feature: SqlServer transport
* Feature: Compression/encryption (middleware).
* Feature: Message consumption during scheduled periods of time.
* Feature: Non native duplicate detection (middleware).
* Feature: Non native message defering (middleware).
* Feature: Non native partition listening/sending.
* Feature: Message expiration.
* Feature: Message polling.
* Feature: Message translator (x.Translate<Translator>("sendto")).
* Feature: Return address and Message reply to (context.ReplyToReturnAddress(...)).
* Feature: Recipient list (x.SentTo(new []{"a","b"}))
* Feature: Message resequencer (x.Resequence("sentto", batchsize, timeout, x => x.Sequence, allowduplicates)).