using Jal.Router.Fluent.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class FilterBuilder : IFilterBuilder
    {
        private readonly Route _route;
        public FilterBuilder(Route route)
        {
            _route = route;
        }

        public void Add<TFilter>() where TFilter : IMiddleware
        {
            _route.Filters.Add(typeof(TFilter));
        }
    }
}