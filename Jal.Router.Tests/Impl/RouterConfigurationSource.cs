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
    public class RouterConfigurationSourceFlowC : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSourceFlowC()
        {
            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowc")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowc", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().Using<TriggerFlowCHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterOrigin("appflowc", "789");

            RegisterEndPoint("appe")
             .ForMessage<RequestToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appequeue");


        }
    }
    public class RouterConfigurationSourceApp : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSourceApp()
        {

            RegisterEndPoint("appz")
            .ForMessage<ResponseToSend>()
            .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appzqueue");

            RegisterOrigin("app", "456");


            RegisterHandler<IRequestResponseHandler<ResponseToSend>>("appz")
                .ToListenQueue<IRequestResponseHandler<ResponseToSend>, AppSettingValueSettingFinder>("appzqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().Using<RequestToSendAppZHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

        }
    }
    public class RouterConfigurationSourceInner : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSourceInner()
        {
            RegisterOrigin("inner","123");

            RegisterSaga<Data>("innersaga", @start =>
            {
                @start.RegisterHandler<IRequestResponseHandler<ResponseToSend, Data>>("appx")
                    .ToListenQueue<IRequestResponseHandler<ResponseToSend, Data>, Data, AppSettingValueSettingFinder>("appxqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                    .ForMessage<ResponseToSend>().Using<RequestToSendAppXHandler>(x =>
                    {
                        x.With(((request, handler, context, data) => handler.Handle(request, context, data)));
                    });
            },
            @continue =>
            {

            },
            @end =>
            {

            });

            RegisterEndPoint("appf")
             .ForMessage<ResponseToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appfqueue");

        }
    }
    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterEndPoint("torequestqueue")
                .ForMessage<RequestToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "requestqueue")
                .AndWaitReplyFromPublishSubscribeChannel<AppSettingValueSettingFinder>("responsetopic","subscription",x=> "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=",10);

            RegisterEndPoint("toresponsetopic")
                .ForMessage<ResponseToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "responsetopic");

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("request")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder>("requestqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().Using<RequestHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterHandler<IRequestResponseHandler<Trigger>>("trigger")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().Using<TriggerHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowa")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowa", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().Using<TriggerFlowAHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appa")
                .ForMessage<RequestToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appaqueue");

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appa")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder>("appaqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().Using<RequestToSendAppAHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appb")
                .ForMessage<ResponseToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appbqueue");

            RegisterHandler<IRequestResponseHandler<ResponseToSend>>("appb")
                .ToListenQueue<IRequestResponseHandler<ResponseToSend>, AppSettingValueSettingFinder>("appbqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().Using<ResponseToSendAppBHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });


            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowb")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowb", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().Using<TriggerFlowBHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appc")
                .ForMessage<RequestToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appcqueue");

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appc")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder>("appcqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().Using<RequestToSendAppCHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appd")
                .ForMessage<ResponseToSend>()
                .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appdtopic");

            RegisterHandler<IRequestResponseHandler<ResponseToSend>>("appd")
                .ToListenTopic<IRequestResponseHandler<ResponseToSend>, AppSettingValueSettingFinder>("appdtopic", "subscription", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().Using<ResponseToSendAppDHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });



            RegisterEndPoint("apph")
             .ForMessage<ResponseToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "apphqueue");


            RegisterEndPoint("appx")
             .ForMessage<ResponseToSend>()
             .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appxqueue");


            RegisterSaga<Data>("saga", start =>
            {
                start.RegisterHandler<IRequestResponseHandler<RequestToSend, Data>>("appe")
                .ToListenQueue<IRequestResponseHandler<RequestToSend, Data>, Data, AppSettingValueSettingFinder>("appequeue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().Using<RequestToSendAppEHandler>(x =>
                {
                    x.With((request, handler, context, data) => handler.Handle(request, context, data), "START");
                });
            }, @continue =>
            {
                @continue.RegisterHandler<IRequestResponseHandler<ResponseToSend, Data>>("appf")
                .ToListenPointToPointChannel<AppSettingValueSettingFinder>("appfqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().Using<ResponseToSendAppFHandler>(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(request, context, data)), "CONTINUE");
                });
            }, end =>
            {
                end.RegisterHandler<IRequestResponseHandler<ResponseToSend, Data>>("apph")
                .ToListenPointToPointChannel<AppSettingValueSettingFinder>("apphqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<ResponseToSend>().Using<ResponseToSendAppHHandler>(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(request, context, data)), "END");
                });
            }).WithTimeout(50);

            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowd")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowd", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().Using<TriggerFlowDHandler>(x =>
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

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appg")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder >("appgqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().Using<ResponseToSendAppGHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                })
                .OnExceptionRetryFailedMessageTo<ApplicationException>("appgretry")
                .Using<AppSettingValueSettingFinder>(y => new LinearRetryPolicy(3, 3))
                .OnErrorSendFailedMessageTo("appgretryerror");

            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowe")
                .ToListenQueue<IRequestResponseHandler<Trigger>, AppSettingValueSettingFinder>("triggerqueueflowe", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<Trigger>().Using<TriggerFlowEHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appi")
                 .ForMessage<RequestToSend>()
                 .To<AppSettingValueSettingFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appiqueue");

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appi")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder>("appiqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().Using<ResponseToSendAppIHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                }).When(x=>x.Headers.ContainsKey("appi"));

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appi")
                .ToListenQueue<IRequestResponseHandler<RequestToSend>, AppSettingValueSettingFinder>("appiqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=")
                .ForMessage<RequestToSend>().Using<ResponseToSendAppJHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                }).When(x => x.Headers.ContainsKey("appj"));

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
