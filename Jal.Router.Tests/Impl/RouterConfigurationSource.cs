using System.Security.Cryptography.X509Certificates;
using Jal.Router.Impl;
using Jal.Router.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class RouterConfigurationSource : AbstractRouterConfigurationSource
    {
        public RouterConfigurationSource()
        {
            RegisterRoute<IMessageHandler<Request>>().ForMessage<Request>().ToBeConsumedBy<MessageHandler>(x =>
            {
                x.With(((request, handler) => handler.Handle(request)));
            });


            RegisterRoute<IMessageHandler<Request>>().ForMessage<Request>().ToBeConsumedBy<OtherMessageHandler>(x =>
            {
                x.With(((request, handler) => handler.Handle(request)));
                //x.With<Response>(((request, handler, context) => handler.Handle(request, context)));
            });


            //RegisterFlow<Data>(s => s.Id).StartedByMessage<Request>(h => h.Name).UsingRoute("Tag1").FollowingBy(y =>
            //{
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //    y.Message<Request>(r => r.Id).UsingRoute("");
            //});

            //RegisterEndpoint("").With<SettingExtractor>().ToSend<Request>().To(x=>x.Get(), "").ReplyTo("", "").WithMessageId(x=>x.D);
        }
    }
}
