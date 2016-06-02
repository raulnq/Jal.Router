using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractRequestSender<TRequest, TResponse> : IRequestSender<TRequest, TResponse>
    {
        public virtual TResponse Send(TRequest request, EndPoint endPoint)
        {
            throw new System.NotImplementedException();
        }

        public virtual TResponse Send(TRequest request, EndPoint endPoint, dynamic context)
        {
            return Send(request, endPoint);
        }
    }
}
