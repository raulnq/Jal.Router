namespace Jal.Router.Fluent.Interface
{
    public interface IMiddleRouteBuilder<out TData>
    {
        IMiddleListenerRouteBuilder<TData> RegisterHandler(string name);
    }
}