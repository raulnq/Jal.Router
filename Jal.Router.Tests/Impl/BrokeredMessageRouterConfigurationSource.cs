using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class BrokeredMessageRouterConfigurationSource : AbstractBrokeredMessageRouterConfigurationSource
    {
        public BrokeredMessageRouterConfigurationSource()
        {
            RegisterEndPoint<AppBrokeredMessageEndPointSettingValueFinder>()
                .ForMessage<Request>()
                .From(x => x.Find("from"))
                .To(x => x.Find("toconnectionstring"), x => x.Find("topath"))
                .ReplyTo(x => x.Find("replytoconnectionstring"), x => x.Find("replytopath"));

            RegisterEndPoint<BrokeredMessageEndPointSettingFinder, Request>();
        }
    }
}