namespace Jal.Router.Fluent.Interface
{
    public interface IMiddleRouteBuilder<out TData>
    {
        IMiddleListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name);
    }
}