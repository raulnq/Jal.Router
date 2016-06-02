using Jal.Router.Interface;

namespace Jal.Router.Model
{
    public class Route<TRequest, TResponse>
    {
        public IRequestSender<TRequest, TResponse> RequestSender { get; set; }

        public EndPoint[] EndPoints { get; set; }
    }
}
