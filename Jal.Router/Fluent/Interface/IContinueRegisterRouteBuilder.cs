namespace Jal.Router.Fluent.Interface
{
    public interface IContinueRegisterRouteBuilder<out TData>
    {
        IContinueNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "");
    }
}