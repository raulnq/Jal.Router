using Jal.Router.Fluent.Impl;

namespace Jal.Router.Fluent.Interface
{
    public interface IStartingRouteBuilder<out TData>
    {
        IStartingListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name);
    }
}