using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RouteProvider : IRouteProvider
    {

        public RouteProvider(IRequestSenderSource requestSenderSource, IEndPointSource endPointSource)
        {
            SenderSource = requestSenderSource;

            EndPointSource = endPointSource;
        }

        public IRequestSenderSource SenderSource { get; set; }

        public IEndPointSource EndPointSource { get; set; }

        public Route<TRequest, TResponse>[] Provide<TRequest, TResponse>(TRequest request, string route)
        {
            var list = new List<Route<TRequest, TResponse>>();

            var senders = SenderSource.Get<TRequest, TResponse>(request, route);

            foreach (var sender in senders)
            {
                var endpoints = EndPointSource.Get(request, sender, route);

                if (endpoints.Length > 0)
                {
                    list.Add(new Route<TRequest, TResponse>
                    {
                        RequestSender = sender,
                        EndPoints = endpoints
                    });
                }
            }

            return list.ToArray();
        }

    }
}
