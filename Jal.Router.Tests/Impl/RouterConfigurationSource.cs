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
            //RegisterSaga<Data>("saga", start =>
            //{
            //    start.RegisterRoute<IMessageSagaHandler<Message>>("route1saga")
            //    .ToListenPointToPointChannel<AppSettingValueSettingFinder>("sagainputqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
            //    .ForMessage<Message>().ToBeHandledBy<SagaInput1HandlerMessageHandler>(x =>
            //    {
            //        x.With((request, handler, context, data) => handler.Handle(context, request, data));
            //    }).OnExceptionRetryFailedMessageTo<Exception>("retry")
            //    .Using<AppSettingValueSettingFinder>(y => new LinearRetryPolicy(60, 5))
            //    .OnErrorSendFailedMessageTo("error");
            //}, @continue =>
            //{
            //    @continue.RegisterRoute<IMessageSagaHandler<Message>>("route2saga")
            //    .ToListenPublishSubscribeChannel<AppSettingValueSettingFinder>("sagainputtopic", "subscription", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
            //    .ForMessage<Message>().ToBeHandledBy<SagaInputTopicHandlerMessageHandler>(x =>
            //    {
            //        x.With(((request, handler, context, data) => handler.Handle(context, request, data)));
            //    });
            //    @continue.RegisterRoute<IMessageSagaHandler<Message>>("route3saga")
            //    .ToListenPublishSubscribeChannel<AppSettingValueSettingFinder>("sagainputtopic2", "subscription", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
            //    .ForMessage<Message>().ToBeHandledBy<SagaInputTopic2HandlerMessageHandler>(x =>
            //    {
            //        x.With(((request, handler, context, data) => handler.Handle(context, request, data)));
            //    });
            //}
            //);//.WithTimeout(100);

            ////RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<MessageHandler>(x =>
            ////{
            ////    x.With(((request, handler) => handler.Handle(request, null)));
            ////}).When(((message, context) => context.Origin.Key == "A")); ;


            //RegisterRoute<IMessageHandler<Message>>("route1")
            //    .ToListenQueue<IMessageHandler<Message>, AppSettingValueSettingFinder >("inputqueue", x=> "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
            //    .ForMessage<Message>().ToBeHandledBy<OtherMessageHandler>(x =>
            //{
            //    x.With(((request, handler) => handler.Handle(request, null)));
            //});


            //RegisterRoute<IMessageHandler<Message>>("route2")
            //    .ToListenPublishSubscribeChannel<AppSettingValueSettingFinder>("testtopic", "subscripcion1", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
            //    .ForMessage<Message>().ToBeHandledBy<OtherMessageHandler>(x =>
            //    {
            //        x.With(((request, handler) => handler.Handle(request, null)));
            //    })
            //    .OnExceptionRetryFailedMessageTo<ApplicationException>("retry")
            //    .Using<AppSettingValueSettingFinder>(y => new LinearRetryPolicy(10, 5))
            //    .OnErrorSendFailedMessageTo("error");
            //    //.UsingStorage<AzureTableStorage>()
            //    //.UsingMessageChannel<AzureServiceBusQueue, AzureServiceBusTopic, AzureServiceBusManager>();

            //RegisterOrigin("From", "2CE8F3B2-6542-4D5C-8B08-E7E64EF57D22");

            //RegisterEndPoint("xyz")
            //    .ForMessage<Message>()
            //    .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueue")
            //    .AndWaitReplyFromPublishSubscribeChannel<AppSettingValueSettingFinder>("testtopic", "subscripcion1", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            //RegisterEndPoint()
            //    .ForMessage<Message>()
            //    .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "outputqueue");

            
            //RegisterEndPoint("SagaInputTopicHandlerMessageHandler")
            //    .ForMessage<Message>()
            //    .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "sagainputtopic");

            //RegisterEndPoint("SagaInputTopic2HandlerMessageHandler")
            //    .ForMessage<Message>()
            //    .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "sagainputtopic2");

            //RegisterEndPoint("retry")
            //    .ForMessage<Message>()
            //    .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "sagainputqueue");

            //RegisterEndPoint("error")
            //    .ForMessage<Message>()
            //    .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "errorqueue");


            RegisterEndPoint("torequestqueue")
                .ForMessage<RequestToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "requestqueue")
                .AndWaitReplyFromPublishSubscribeChannel<AppSettingValueSettingFinder>("responsetopic","subscription",x=> "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=",10);

            RegisterEndPoint("toresponsetopic")
                .ForMessage<ResponseToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "responsetopic");

            RegisterRoute<IRequestResponseHandler<RequestToSend>>("request")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder>("requestqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().ToBeHandledBy<RequestHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterRoute<IRequestResponseHandler<Trigger>>("trigger")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().ToBeHandledBy<TriggerHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterRoute<IRequestResponseHandler<Trigger>>("triggerflowa")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowa", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().ToBeHandledBy<TriggerFlowAHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appa")
                .ForMessage<RequestToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appaqueue");

            RegisterRoute<IRequestResponseHandler<RequestToSend>>("appa")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder>("appaqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().ToBeHandledBy<RequestToSendAppAHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appb")
                .ForMessage<ResponseToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appbqueue");

            RegisterRoute<IRequestResponseHandler<ResponseToSend>>("appb")
                .ToListenQueue<IRequestResponseHandler<ResponseToSend>, AppSettingValueSettingFinder>("appbqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().ToBeHandledBy<ResponseToSendAppBHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });


            RegisterRoute<IRequestResponseHandler<Trigger>>("triggerflowb")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowb", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().ToBeHandledBy<TriggerFlowBHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appc")
                .ForMessage<RequestToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appcqueue");

            RegisterRoute<IRequestResponseHandler<RequestToSend>>("appc")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder>("appcqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().ToBeHandledBy<RequestToSendAppCHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appd")
                .ForMessage<ResponseToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appdtopic");

            RegisterRoute<IRequestResponseHandler<ResponseToSend>>("appd")
                .ToListenTopic<IRequestResponseHandler<ResponseToSend>, AppSettingValueSettingFinder>("appdtopic", "subscription", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().ToBeHandledBy<ResponseToSendAppDHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });


            RegisterRoute<IRequestResponseHandler<Trigger>>("triggerflowc")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowc", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().ToBeHandledBy<TriggerFlowCHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appe")
             .ForMessage<RequestToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appequeue");

            RegisterEndPoint("appf")
             .ForMessage<ResponseToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appfqueue");

            RegisterEndPoint("appx")
             .ForMessage<ResponseToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appxqueue");

            RegisterRoute<IRequestResponseHandler<ResponseToSend>>("appx")
                .ToListenQueue<IRequestResponseHandler<ResponseToSend>, AppSettingValueSettingFinder>("appxqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().ToBeHandledBy<RequestToSendAppXHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterSaga<Data>("saga", start =>
            {
                start.RegisterRoute<IRequestResponseHandler<RequestToSend, Data>>("appe")
                .ToListenQueue<IRequestResponseHandler<RequestToSend, Data>, Data, AppSettingValueSettingFinder>("appequeue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().ToBeHandledBy<RequestToSendAppEHandler>(x =>
                {
                    x.With((request, handler, context, data) => handler.Handle(request, context, data));
                });
            }, @continue =>
            {
                @continue.RegisterRoute<IRequestResponseHandler<ResponseToSend, Data>>("appf")
                .ToListenPointToPointChannel<AppSettingValueSettingFinder>("appfqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().ToBeHandledBy<ResponseToSendAppFHandler>(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(request, context, data)));
                });
            });

            RegisterRoute<IRequestResponseHandler<Trigger>>("triggerflowd")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowd", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().ToBeHandledBy<TriggerFlowDHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appg")
             .ForMessage<RequestToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appgqueue");

            RegisterEndPoint("appgretry")
             .ForMessage<RequestToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appgqueue");

            RegisterEndPoint("appgretryerror")
             .ForMessage<RequestToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appgerrorqueue");

            RegisterRoute<IRequestResponseHandler<RequestToSend>>("appg")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder >("appgqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().ToBeHandledBy<ResponseToSendAppGHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                })
                .OnExceptionRetryFailedMessageTo<ApplicationException>("appgretry")
                .Using<AppSettingValueSettingFinder>(y => new LinearRetryPolicy(3, 3))
                .OnErrorSendFailedMessageTo("appgretryerror");



            //.UsingStorage<AzureTableStorage>()
            //.UsingMessageChannel<AzureServiceBusQueue, AzureServiceBusTopic, AzureServiceBusManager>();

            //RegisterEndPoint<EndPointSettingFinder, Message>();

            //RegisterSubscriptionToPublishSubscriberChannel<AppSettingValueSettingFinder>("subscripcion12", "testtopic", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            //RegisterPointToPointChannel<AppSettingValueSettingFinder>("errorqueue12", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            //this.RegisterQueue<AppSettingValueSettingFinder>("errorqueue12", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            //RegisterPublishSubscriberChannel<AppSettingValueSettingFinder>("errortopic12", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=");

            //RegisterSubscription();
            //RegisterFlow<Data>(s => s.Id).StartedByMessage<Request>(h => h.Name).UsingRoute("Tag1").FollowingBy(y =>
            //{
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //});
        }
    }
}
