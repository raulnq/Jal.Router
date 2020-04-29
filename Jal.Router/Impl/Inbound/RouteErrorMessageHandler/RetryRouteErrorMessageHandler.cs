using System;
using System.Threading.Tasks;
using Jal.Router.Extensions;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RetryRouteErrorMessageHandler : IRouteErrorMessageHandler
    {
        private readonly ILogger _logger;

        public RetryRouteErrorMessageHandler(ILogger logger)
        {
            _logger = logger;
        }
        public async Task<bool> Handle(MessageContext context, Exception ex, ErrorHandler metadata)
        {
            if(metadata.Parameters.ContainsKey("endpoint") && metadata.Parameters["endpoint"] is string endpointname && !string.IsNullOrEmpty(endpointname)
                && metadata.Parameters.ContainsKey("policy") && metadata.Parameters["policy"] is IRetryPolicy policy
                && metadata.Parameters.ContainsKey("type") && metadata.Parameters["type"] is Type type && type != null)
            {
                var countername = $"{context.Route.Name}_{policy.GetType().Name.ToLower()}_retrycount";

                var count = 0;

                if (context.Headers.ContainsKey(countername))
                {
                    count = Convert.ToInt32(context.Headers[countername]);
                }
                else
                {
                    context.Headers.Add(countername, count.ToString());
                }

                count++;

                if (policy.CanRetry(count))
                {
                    context.Headers[countername] = count.ToString();

                    var content = context.Deserialize(context.ContentContext.Data, type);

                    _logger.Log($"Message {context.Id}, sending the message to the retry endpoint {endpointname} (retry count {count}) by route {context.Name}");

                    await context.Send(content, endpointname, scheduledenqueuedatetimeutc: DateTime.UtcNow.Add(policy.NextRetryInterval(count))).ConfigureAwait(false);

                    return metadata.StopAfterHandle;
                }
                else
                {
                    _logger.Log($"Message {context.Id}, no more retries by route {context.Name}");

                    if (metadata.Parameters.ContainsKey("fallback") && metadata.Parameters["fallback"] is Func<MessageContext, Exception, ErrorHandler, Task> fallback)
                    {
                        _logger.Log($"Message {context.Id}, fallback executed by route {context.Name}");

                        await fallback(context, ex, metadata).ConfigureAwait(false);

                        return metadata.StopAfterHandle;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}