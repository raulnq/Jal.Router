using Jal.Router.Impl;
using Jal.Router.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class RequestRouterConfigurationSource : AbstractRequestRouterConfigurationSource
    {
        public RequestRouterConfigurationSource()
        {
            Route<Request, Response>().To<RequestSender>().With( settings => new []{ new EndPoint { Uri = "xxx"}} );
            Route<Request, Response>().To<OtherRequestSender>().With(settings => new[] { new EndPoint { Uri = "yyy" } });

            Route<Request, Response>("Route", x =>
                           {
                               x.To<RequestSender>().With(settings => new[] { new EndPoint { Uri = "abc" } });
                               x.To<OtherRequestSender>().With(settings => new[] { new EndPoint { Uri = "1234" } });
                           });
        }
    }
}
