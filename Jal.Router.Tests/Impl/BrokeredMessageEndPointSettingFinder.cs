using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.AzureServiceBus.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class BrokeredMessageEndPointSettingFinder : IBrokeredMessageEndPointSettingFinder<Request>
    {
        public BrokeredMessageEndPoint Find(Request record)
        {
            return new BrokeredMessageEndPoint()
            {
                From = "example"
            };
        }
    }
}