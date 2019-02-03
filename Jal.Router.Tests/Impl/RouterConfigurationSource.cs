using System;
using Jal.Router.AzureServiceBus.Standard.Extensions;
using Jal.Router.AzureServiceBus.Standard.Impl;
using Jal.Router.AzureStorage.Impl;
using Jal.Router.Impl;
using Jal.Router.Impl.Inbound;
using Jal.Router.Model;
using Jal.Router.Tests.Model;
using Microsoft.ServiceBus.Messaging;
using Jal.Router.Extensions;
using Jal.Router.Impl.Inbound.RetryPolicy;
using Jal.Router.Impl.ValueFinder;

namespace Jal.Router.Tests.Impl
{
    public class RouterConfigurationSourceFlowC : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSourceFlowC()
        {
            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowc")
                .ToListen(y=>y.AddQueue<AppSettingValueFinder>("triggerqueueflowc", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use<TriggerFlowCHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterOrigin("appflowc", "789");

            RegisterEndPoint("appe")
             .ForMessage<RequestToSend>()
             .To(y=>y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appequeue"));
        }
    }
    public class RouterConfigurationSourceApp : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSourceApp()
        {

            RegisterEndPoint("appz")
            .ForMessage<ResponseToSend>()
            .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appzqueue"));

            RegisterOrigin("app", "456");


            RegisterHandler<IRequestResponseHandler<ResponseToSend>>("appz")
                .ToListen(y=>y.AddQueue<AppSettingValueFinder>("appzqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use<RequestToSendAppZHandler>(x =>
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
                .ToListen(y=>y.AddQueue<AppSettingValueFinder>("appxqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                    .ForMessage<ResponseToSend>().Use<RequestToSendAppXHandler>(x =>
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
             .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appfqueue"));
             

        }
    }
    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterEndPoint("torequestqueue")
                .ForMessage<RequestToSend>()
                .To(x => x.AddPointToPointChannel<AppSettingValueFinder>(z => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "requestqueue")
                .AndWaitReplyFromSubscriptionToPublishSubscribeChannel<AppSettingValueFinder>("responsetopic", "subscription", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", 10));

            RegisterEndPoint("toresponsetopic")
                .ForMessage<ResponseToSend>()
                .To(y => y.AddPublishSubscribeChannel<AppSettingValueFinder>(x=>"Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "responsetopic"));
                

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("request")
                .ToListen(x=>x.AddQueue<AppSettingValueFinder>("requestqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use<RequestHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterHandler<IRequestResponseHandler<Trigger>>("trigger")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use<TriggerHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowa")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueueflowa", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use<TriggerFlowAHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appa")
                .ForMessage<RequestToSend>()
                .To(y=>y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appaqueue"));

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appa")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appaqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use<RequestToSendAppAHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appb")
                .ForMessage<ResponseToSend>()
                .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appbqueue"));

            RegisterHandler<IRequestResponseHandler<ResponseToSend>>("appb")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appbqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use<ResponseToSendAppBHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });


            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowb")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueueflowb", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use<TriggerFlowBHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appc")
                .ForMessage<RequestToSend>()
                .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appcqueue"));

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appc")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appcqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use<RequestToSendAppCHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appd")
                .ForMessage<ResponseToSend>()
                .To(y => y.AddPublishSubscribeChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appdtopic"));

            RegisterHandler<IRequestResponseHandler<ResponseToSend>>("appd")
                .ToListen(x => x.AddSubscriptionToTopic<AppSettingValueFinder>("appdtopic", "subscription", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use<ResponseToSendAppDHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });



            RegisterEndPoint("apph")
             .ForMessage<ResponseToSend>()
             .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "apphqueue"));


            RegisterEndPoint("appx")
             .ForMessage<ResponseToSend>()
             .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appxqueue"));


            RegisterSaga<Data>("saga", start =>
            {
                start.RegisterHandler<IRequestResponseHandler<RequestToSend, Data>>("appe")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appequeue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use<RequestToSendAppEHandler>(x =>
                {
                    x.With((request, handler, context, data) => handler.Handle(request, context, data), "START");
                });
            }, @continue =>
            {
                @continue.RegisterHandler<IRequestResponseHandler<ResponseToSend, Data>>("appf")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appfqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use<ResponseToSendAppFHandler>(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(request, context, data)), "CONTINUE");
                });
            }, end =>
            {
                end.RegisterHandler<IRequestResponseHandler<ResponseToSend, Data>>("apph")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("apphqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use<ResponseToSendAppHHandler>(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(request, context, data)), "END");
                });
            }).WithTimeout(50);

            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowd")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueueflowd", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use<TriggerFlowDHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appg")
             .ForMessage<RequestToSend>()
             .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appgqueue"));

            RegisterEndPoint("appgretry")
             .ForMessage<RequestToSend>()
             .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appgqueue"));

            RegisterEndPoint("appgretryerror")
             .ForMessage<RequestToSend>()
             .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appgerrorqueue"));

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appg")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appgqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use<ResponseToSendAppGHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                })
                //.OnExceptionRetryFailedMessageTo<ApplicationException>("appgretry")
                //.Use<AppSettingValueFinder>(y => new LinearRetryPolicy(3, 3))
                .OnErrorSendFailedMessageTo("appgretryerror");

            RegisterHandler<IRequestResponseHandler<Trigger>>("triggerflowe")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueueflowe", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use<TriggerFlowEHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appi")
                 .ForMessage<RequestToSend>()
                 .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appiqueue"));

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appi")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appiqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use<ResponseToSendAppIHandler>(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                }).When(x=>x.Headers.ContainsKey("appi"));

            RegisterHandler<IRequestResponseHandler<RequestToSend>>("appi")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appiqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use<ResponseToSendAppJHandler>(x =>
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
