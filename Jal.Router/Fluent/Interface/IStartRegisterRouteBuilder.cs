namespace Jal.Router.Fluent.Interface
{
    public interface IStartRegisterRouteBuilder<out TData>
    {
        IStartNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "");
    }
}