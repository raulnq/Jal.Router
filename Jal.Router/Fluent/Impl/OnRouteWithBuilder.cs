using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OnRouteWithBuilder : IOnRouteWithBuilder
    {
        private readonly Route _route;
        public OnRouteWithBuilder(Route route)
        {
            _route = route;
        }

        public void AsClaimCheck()
        {
            _route.UseClaimCheck = true;
        }
    }
}