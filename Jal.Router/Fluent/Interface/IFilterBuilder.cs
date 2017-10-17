using Jal.Router.Interface.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IFilterBuilder
    {
        void Add<TFilter>() where TFilter : IMiddleware;
    }
}