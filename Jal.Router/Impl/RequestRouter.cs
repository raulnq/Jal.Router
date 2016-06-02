using System;
using System.Collections.Generic;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class RequestRouter : IRequestRouter
    {
        public RequestRouter(IRouteProvider routeProvider)
        {
            Provider = routeProvider;

            Interceptor = AbstractRequestRouterInterceptor.Instance;
        }

        public TResponse[] Route<TRequest, TResponse>(TRequest request)
        {
            return Route<TRequest, TResponse>(request, string.Empty);
        }

        public TResponse[] Route<TRequest, TResponse>(TRequest request, string route)
        {
            var routes = Provider.Provide<TRequest, TResponse>(request, route);

            var responses = new List<TResponse>();

            if (routes != null || routes.Length > 0)
            {
                foreach (var r in routes)
                {
                    var response = default(TResponse);
                    try
                    {
                        Interceptor.OnEntry(request, r);

                        foreach (var endpoint in r.EndPoints)
                        {
                            if (endpoint.Enabled)
                            {
                                response = r.RequestSender.Send(request, endpoint);
                            }
                                
                            Interceptor.OnSuccess(request, r, endpoint, response);

                            responses.Add(response);
                        }
                    }
                    catch (Exception ex)
                    {
                        Interceptor.OnError(request, r, response, ex);
                        throw;
                    }
                    finally
                    {
                        Interceptor.OnExit(request, r, response);
                    }
                }
            }
            else
            {
                throw new Exception(string.Format("No routes were found for the request type {0} and route {1}", typeof(TRequest).FullName, route));
            }

            return responses.ToArray();
        }

        public IRouteProvider Provider { get; set; }

        public IRequestRouterInterceptor Interceptor { get; set; }
    }
}
