using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class MessageRouterConfigurationSource : AbstractMessageRouterConfigurationSource
    {
        public MessageRouterConfigurationSource()
        {
            Route<Request>().To<MessageSender>();
            Route<Request>().To<OtherMessageSender>();

            Route<Request>("Route", x =>
                           {
                               x.To<MessageSender>();
                               x.To<OtherMessageSender>();
                           });
        }
    }
}
