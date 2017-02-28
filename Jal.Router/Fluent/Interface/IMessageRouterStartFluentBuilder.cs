using Jal.Factory.Interface;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IMessageRouterStartFluentBuilder
    {
        IMessageRouterFluentBuilder UseObjectFactory(IObjectFactory objectFactory);
    }
}