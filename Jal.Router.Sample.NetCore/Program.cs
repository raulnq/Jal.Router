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

namespace Jal.Router.Sample.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new ServiceContainer();
            container.RegisterRouter(new IRouterConfigurationSource[] { new RouterConfigurationSourceSample() });
            container.RegisterFrom<ServiceLocatorCompositionRoot>();
            container.RegisterAzureServiceBusRouter();
            container.RegisterAzureSagaStorage("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==", "sagacore", "messagescore", DateTime.UtcNow.ToString("yyyyMMdd"));
            container.RegisterAzureMessageStorage("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeus001;AccountKey=xn2flH2joqs8LM0JKQXrOAWEEXc/I4e9AF873p1W/2grHSht8WEIkBbbl3PssTatuRCLlqMxbkvhKN9VmcPsFA==", "messages");
            container.Register<IMessageHandler, MessageHandler>(typeof(MessageHandler).FullName, new PerContainerLifetime());
            container.Register<IMessageHandler, MessageHandlerB>(typeof(MessageHandlerB).FullName, new PerContainerLifetime());

            var host = container.GetInstance<IHost>();
            host.Configuration.UsingAzureServiceBus();
            host.Configuration.UsingAzureSagaStorage();
            host.Configuration.UsingAzureMessageStorage();
            host.Configuration.UsingChannelShuffler<FisherYatesChannelShuffler>();
            host.RunAndBlock();
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
                    x.Add("Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueuenewrelease1");
                    x.Add("Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueuenewrelease2");
                    x.Add("Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueuenewrelease3");
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
