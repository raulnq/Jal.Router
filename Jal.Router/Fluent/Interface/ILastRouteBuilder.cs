
namespace Jal.Router.Fluent.Interface
{
    public interface ILastRouteBuilder<out TData>
    {
        ILastListenerRouteBuilder<TData> RegisterHandler(string name);
    }
}