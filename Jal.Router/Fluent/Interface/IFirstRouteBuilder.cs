using Jal.Router.Fluent.Impl;

namespace Jal.Router.Fluent.Interface
{
    public interface IFirstRouteBuilder<out TData>
    {
        IFirstListenerRouteBuilder<TData> RegisterHandler(string name);
    }
}