namespace Jal.Router.Fluent.Interface
{
    public interface IRouteBuilder<out TData>
    {
        IListenerRouteBuilder<TData> RegisterHandler(string name);
    }
}