namespace Jal.Router.Fluent.Interface
{
    public interface INameRouteBuilder
    {
        IHandlerBuilder<TContent> ForMessage<TContent>();
    }
}