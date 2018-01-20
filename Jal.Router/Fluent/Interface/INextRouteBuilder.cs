namespace Jal.Router.Fluent.Interface
{
    public interface INextRouteBuilder<out TData>
    {
        INextListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name);
    }
}