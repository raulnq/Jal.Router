using System;
using Jal.Router.AzureServiceBus.Extensions;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureStorage.Impl;
using Jal.Router.Impl;
using Jal.Router.Impl.Inbound;
using Jal.Router.Model;
using Jal.Router.Tests.Model;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.Tests.Impl
{
    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterSaga<Data>("saga", start =>
            {
                start.RegisterRoute<IMessageSagaHandler<Message>>("route1saga")
                .ToListenPointToPointChannel<AppSettingValueSettingFinder>("sagainputqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Message>().ToBeHandledBy<SagaInput1HandlerMessageHandler>(x =>
                {
                    x.With((request, handler, context, data) => handler.Handle(context, request, data));
                }).OnExceptionRetryFailedMessageTo<Exception>("retry")
                .Using<AppSettingValueSettingFinder>(y => new LinearRetryPolicy(60, 5))
                .OnErrorSendFailedMessageTo("error");
            }, @continue =>
            {
                @continue.RegisterRoute<IMessageSagaHandler<Message>>("route2saga")
                .ToListenPublishSubscribeChannel<AppSettingValueSettingFinder>("sagainputtopic", "subscription", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Message>().ToBeHandledBy<SagaInputTopicHandlerMessageHandler>(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(context, request, data)));
                });
                @continue.RegisterRoute<IMessageSagaHandler<Message>>("route3saga")
                .ToListenPublishSubscribeChannel<AppSettingValueSettingFinder>("sagainputtopic2", "subscription", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Message>().ToBeHandledBy<SagaInputTopic2HandlerMessageHandler>(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(context, request, data)));
                });
            }
            );//.WithTimeout(100);

            //RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<MessageHandler>(x =>
            //{
            //    x.With(((request, handler) => handler.Handle(request, null)));
            //}).When(((message, context) => context.Origin.Key == "A")); ;


            RegisterRoute<IMessageHandler<Message>>("route1")
                .ToListenQueue<IMessageHandler<Message>, AppSettingValueSettingFinder >("inputqueue", x=> "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Message>().ToBeHandledBy<OtherMessageHandler>(x =>
            {
                x.With(((request, handler) => handler.Handle(request, null)));
            });


            RegisterRoute<IMessageHandler<Message>>("route2")
                .ToListenPublishSubscribeChannel<AppSettingValueSettingFinder>("testtopic", "subscripcion1", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Message>().ToBeHandledBy<OtherMessageHandler>(x =>
                {
                    x.With(((request, handler) => handler.Handle(request, null)));
                })
                .OnExceptionRetryFailedMessageTo<ApplicationException>("retry")
                .Using<AppSettingValueSettingFinder>(y => new LinearRetryPolicy(10, 5))
                .OnErrorSendFailedMessageTo("error");
                //.UsingStorage<AzureTableStorage>()
                //.UsingMessageChannel<AzureServiceBusQueue, AzureServiceBusTopic, AzureServiceBusManager>();

            RegisterOrigin("From", "2CE8F3B2-6542-4D5C-8B08-E7E64EF57D22");

            RegisterEndPoint()
                .ForMessage<Message>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueue");

            
            RegisterEndPoint("SagaInputTopicHandlerMessageHandler")
                .ForMessage<Message>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "sagainputtopic");

            RegisterEndPoint("SagaInputTopic2HandlerMessageHandler")
                .ForMessage<Message>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "sagainputtopic2");

            RegisterEndPoint("retry")
                .ForMessage<Message>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "sagainputqueue");

            RegisterEndPoint("error")
                .ForMessage<Message>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "errorqueue");

            //RegisterEndPoint<EndPointSettingFinder, Message>();

            RegisterSubscriptionToPublishSubscriberChannel<AppSettingValueSettingFinder>("subscripcion12", "testtopic", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            RegisterPointToPointChannel<AppSettingValueSettingFinder>("errorqueue12", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            this.RegisterQueue<AppSettingValueSettingFinder>("errorqueue12", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            RegisterPublishSubscriberChannel<AppSettingValueSettingFinder>("errortopic12", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            //RegisterSubscription();
            //RegisterFlow<Data>(s => s.Id).StartedByMessage<Request>(h => h.Name).UsingRoute("Tag1").FollowingBy(y =>
            //{
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //});
        }
    }
}
