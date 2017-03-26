using System.Security.Cryptography.X509Certificates;
using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Impl;
using Jal.Router.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<MessageHandler>(x =>
            {
                x.With(((request, handler) => handler.Handle(request)));
            });


            RegisterRoute<IMessageHandler<Message>>().ForMessage<Message>().ToBeHandledBy<OtherMessageHandler>(x =>
            {
                x.With(((request, handler) => handler.Handle(request)));
                //x.With<Response>(((request, handler, context) => handler.Handle(request, context)));
            });

            RegisterEndPoint<AppSettingEndPointValueSettingFinder>()
                .ForMessage<Message>()
                .From(x => x.Find("from"))
                .To(x => x.Find("toconnectionstring"), x => x.Find("topath"))
                .ReplyTo(x => x.Find("replytoconnectionstring"), x => x.Find("replytopath"));

            RegisterEndPoint<EndPointSettingFinder, Message>();

            //RegisterFlow<Data>(s => s.Id).StartedByMessage<Request>(h => h.Name).UsingRoute("Tag1").FollowingBy(y =>
            //{
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //});
        }
    }
}
