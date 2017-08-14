using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface INameRouteBuilder<THandler>
    {
        IHandlerBuilder<TContent, THandler> ForMessage<TContent>();
    }
}