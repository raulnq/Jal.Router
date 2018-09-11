using Jal.Router.Fluent.Impl;

namespace Jal.Router.Fluent.Interface
{
    public interface IFirstRouteBuilder<out TData>
    {
        IFirstListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name);
    }
}