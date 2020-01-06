using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class OnRouteOptionBuilderExtensions
    {
        public static void ForwardMessageTo(this IOnRouteEntryBuilder builder, string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            builder.Use<ForwardRouteEntryMessageHandler>(new Dictionary<string, object>() { { "endpoint", endpoint } });
        }

        public static void Execute(this IOnRouteEntryBuilder builder, Func<MessageContext, Handler, Task> init)
        {
            if (init==null)
            {
                throw new ArgumentNullException(nameof(init));
            }

            builder.Use<RouteEntryMessageHandler>(new Dictionary<string, object>() { { "init", init } });
        }

        public static IForExceptionBuilder ForwardMessageTo(this IOnRouteErrorBuilder builder, string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            return builder.Use<ForwardRouteErrorMessageHandler>(new Dictionary<string, object>() { { "endpoint", endpoint } });
        }

        public static IForExceptionBuilder RetryMessageTo(this IOnRouteErrorBuilder builder, string endpoint, IRetryPolicy policy, Func<MessageContext, Exception, ErrorHandler, Task> fallback=null)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            if (policy==null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            return builder.Use<RetryRouteErrorMessageHandler>(new Dictionary<string, object>() { { "endpoint", endpoint }, { "policy", policy }, { "fallback", fallback } });
        }
    }
}