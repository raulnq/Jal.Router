using Jal.Factory.Interface;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class RequestSenderSource : IRequestSenderSource
    {
        private readonly IObjectFactory _objectFactory;

        public RequestSenderSource(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }


        public IRequestSender<TRequest, TResponse>[] Get<TRequest, TResponse>(TRequest request, string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return _objectFactory.Create<TRequest, IRequestSender<TRequest, TResponse>>(request);
            }
            return _objectFactory.Create<TRequest, IRequestSender<TRequest, TResponse>>(request, route);
        }
    }
}