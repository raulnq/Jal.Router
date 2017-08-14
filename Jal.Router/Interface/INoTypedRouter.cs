using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface INoTypedRouter
    {
        void Route(object content, InboundMessageContext context, Route[] routes, object data);
    }
}