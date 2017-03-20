using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class BrokeredMessageRouterConfigurationSource : AbstractBrokeredMessageRouterConfigurationSource
    {
        public BrokeredMessageRouterConfigurationSource()
        {
            RegisterEndPoint<AppBrokeredMessageSettingsExtractor>()
                .ForMessage<Request>()
                .From(x => x.Get("from"))
                .To(x => x.Get("toconnectionstring"), x => x.Get("topath"))
                .ReplyTo(x => x.Get("replytoconnectionstring"), x => x.Get("replytopath"));
        }
    }
}