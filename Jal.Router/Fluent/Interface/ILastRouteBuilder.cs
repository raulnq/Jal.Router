
namespace Jal.Router.Fluent.Interface
{
    public interface ILastRouteBuilder<out TData>
    {
        ILastListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name);
    }
}