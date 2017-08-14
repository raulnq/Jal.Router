using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IRouterEndBuilder
    {
        IRouter<TMessage> Create<TMessage>();
    }
}