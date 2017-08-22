using System;
using Jal.Router.Impl;
using Jal.Router.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterSaga<Data>("saga", start =>
            {
                start.RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>(((i, context) => Guid.NewGuid().ToString())).ToBeHandledBy<MessageHandler>(x =>
                {
                    x.With((request, handler, data) => handler.Handle(request, data));
                });
            }, @continue =>
            {
                @continue.RegisterRoute<IMessageHandler<Message1>>().ForMessage<Message1>().ToBeHandledBy<Message1Handler>(x =>
                {
                    x.With(((request, handler, data) => handler.Handle(request, data)));
                });
            }
            );

            RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<MessageHandler>(x =>
            {
                x.With(((request, handler) => handler.Handle(request, null)));
            }).When(((message, context) => context.Origin.Key == "A")); ;


            RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<OtherMessageHandler>(x =>
            {
                x.With(((request, handler) => handler.Handle(request, null))).Retry<ApplicationException>().Using<AppSettingValueSettingFinder>(y => new LinearRetryPolicy(10, 5));
            });

            RegisterOrigin("From", "2CE8F3B2-6542-4D5C-8B08-E7E64EF57D22");

            RegisterEndPoint()
                .ForMessage<Message>()
                .To<AppSettingValueSettingFinder, AppSettingValueSettingFinder>(x => x.Find("toconnectionstring"), x => x.Find("topath"));

            //RegisterEndPoint<EndPointSettingFinder, Message>();

            //RegisterFlow<Data>(s => s.Id).StartedByMessage<Request>(h => h.Name).UsingRoute("Tag1").FollowingBy(y =>
            //{
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //});
        }
    }
}
