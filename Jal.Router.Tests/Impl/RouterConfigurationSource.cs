using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class MessageRouterConfigurationSource : AbstractMessageRouterConfigurationSource
    {
        public MessageRouterConfigurationSource()
        {
            Route<Request>().To<MessageHandler>();
            Route<Request>().To<OtherMessageHandler>();

            Route<Request>("Route", x =>
                           {
                               x.To<MessageHandler>();
                               x.To<OtherMessageHandler>();
                           });
        }
    }
}
