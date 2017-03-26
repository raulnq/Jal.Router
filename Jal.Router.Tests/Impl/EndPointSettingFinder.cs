using Jal.Router.AzureServiceBus.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class EndPointSettingFinder : IEndPointSettingFinder<Message>
    {
        public EndPointSetting Find(Message content)
        {
            return new EndPointSetting()
            {
                From = "example"
            };
        }
    }
}