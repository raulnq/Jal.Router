using Jal.Router.AzureServiceBus.Standard.Extensions;
using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class RouterConfigurationSourceFlowC : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSourceFlowC()
        {
            RegisterHandler("triggerflowc")
                .ToListen(y=>y.AddQueue<AppSettingValueFinder>("triggerqueueflowc", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use< IRequestResponseHandler < Trigger > , TriggerFlowCHandler >(x =>
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


            RegisterHandler("appz")
                .ToListen(y=>y.AddQueue<AppSettingValueFinder>("appzqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use< IRequestResponseHandler < ResponseToSend > , RequestToSendAppZHandler >(x =>
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
                @start.RegisterHandler("appx")
                .ToListen(y=>y.AddQueue<AppSettingValueFinder>("appxqueue", x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                    .ForMessage<ResponseToSend>().Use< IRequestResponseHandler < ResponseToSend, Data > ,RequestToSendAppXHandler >(x =>
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
                .To<Message>(x => x.AddPointToPointChannel<AppSettingValueFinder>(z => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "requestqueue")
                .AndWaitReplyFromSubscriptionToPublishSubscribeChannel<AppSettingValueFinder>("responsetopic", "subscription", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", 10));

            RegisterEndPoint("toresponsetopic")
                .ForMessage<ResponseToSend>()
                .To(y => y.AddPublishSubscribeChannel<AppSettingValueFinder>(x=>"Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "responsetopic"));
                

            RegisterHandler("request")
                .ToListen(x=>x.AddQueue<AppSettingValueFinder>("requestqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use< IRequestResponseHandler < RequestToSend > , RequestHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterHandler("trigger")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use< IRequestResponseHandler < Trigger > ,TriggerHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterHandler("triggerflowa")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueueflowa", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use< IRequestResponseHandler < Trigger > ,TriggerFlowAHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appa")
                .ForMessage<RequestToSend>()
                .To(y=>y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appaqueue"));

            RegisterHandler("appa")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appaqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use< IRequestResponseHandler < RequestToSend > ,RequestToSendAppAHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appb")
                .ForMessage<ResponseToSend>()
                .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appbqueue"));

            RegisterHandler("appb")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appbqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use< IRequestResponseHandler < ResponseToSend > ,ResponseToSendAppBHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });


            RegisterHandler("triggerflowb")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueueflowb", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use< IRequestResponseHandler < Trigger > ,TriggerFlowBHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appc")
                .ForMessage<RequestToSend>()
                .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appcqueue"));

            RegisterHandler("appc")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appcqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use< IRequestResponseHandler < RequestToSend > ,RequestToSendAppCHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appd")
                .ForMessage<ResponseToSend>()
                .To(y => y.AddPublishSubscribeChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appdtopic"));

            RegisterHandler("appd")
                .ToListen(x => x.AddSubscriptionToTopic<AppSettingValueFinder>("appdtopic", "subscription", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use< IRequestResponseHandler < ResponseToSend > ,ResponseToSendAppDHandler >(x =>
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
                start.RegisterHandler("appe")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appequeue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use< IRequestResponseHandler < RequestToSend, Data > , RequestToSendAppEHandler >(x =>
                {
                    x.With((request, handler, context, data) => handler.Handle(request, context, data), "START");
                });
            }, @continue =>
            {
                @continue.RegisterHandler("appf")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appfqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use< IRequestResponseHandler < ResponseToSend, Data > , ResponseToSendAppFHandler >(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(request, context, data)), "CONTINUE");
                });
            }, end =>
            {
                end.RegisterHandler("apph")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("apphqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<ResponseToSend>().Use< IRequestResponseHandler < ResponseToSend, Data > , ResponseToSendAppHHandler >(x =>
                {
                    x.With(((request, handler, context, data) => handler.Handle(request, context, data)), "END");
                });
            }).WithTimeout(50);

            RegisterHandler("triggerflowd")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueueflowd", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use< IRequestResponseHandler < Trigger > ,TriggerFlowDHandler >(x =>
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

            RegisterHandler("appg")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appgqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use< IRequestResponseHandler < RequestToSend > , ResponseToSendAppGHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                })
                //.OnExceptionRetryFailedMessageTo<ApplicationException>("appgretry")
                //.Use<AppSettingValueFinder>(y => new LinearRetryPolicy(3, 3))
                //.OnErrorSendFailedMessageTo("appgretryerror")
                ;

            RegisterHandler("triggerflowe")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("triggerqueueflowe", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<Trigger>().Use< IRequestResponseHandler < Trigger > , TriggerFlowEHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                });

            RegisterEndPoint("appi")
                 .ForMessage<RequestToSend>()
                 .To(y => y.AddPointToPointChannel<AppSettingValueFinder>(x => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo=", "appiqueue"));

            RegisterHandler("appi")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appiqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use< IRequestResponseHandler < RequestToSend > , ResponseToSendAppIHandler >(x =>
                {
                    x.With(((request, handler, context) => handler.Handle(request, context)));
                }).When(x=>x.Headers.ContainsKey("appi"));

            RegisterHandler("appi")
                .ToListen(x => x.AddQueue<AppSettingValueFinder>("appiqueue", y => "Endpoint=sb://raulqueuetests.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=8WpD2e6cWAW3Qj4AECuzdKCySM4M+ZAIW2VGRHvvXlo="))
                .ForMessage<RequestToSend>().Use< IRequestResponseHandler < RequestToSend > , ResponseToSendAppJHandler >(x =>
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
