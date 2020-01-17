using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OnEndPointWithBuilder : IOnEndPointWithBuilder
    {
        private readonly EndPoint _endpoint;
        public OnEndPointWithBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public void AsClaimCheck()
        {
            _endpoint.SetUseClaimCheck(true);
        }
    }
}