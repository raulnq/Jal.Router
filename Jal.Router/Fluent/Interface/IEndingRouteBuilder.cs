using Jal.Router.Fluent.Impl;

namespace Jal.Router.Fluent.Interface
{
    public interface IEndingRouteBuilder<out TData>
    {
        IEndingListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name);
    }
}