using System;
using System.Threading.Tasks;
using Jal.Router.Extensions;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RetryRouteErrorMessageHandler : IRouteErrorMessageHandler
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public RetryRouteErrorMessageHandler(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }
        public async Task<bool> Handle(MessageContext context, Exception ex, ErrorHandler metadata)
        {
            if(metadata.Parameters.ContainsKey("endpoint") && metadata.Parameters.ContainsKey("policy"))
            {
                var endpointname = metadata.Parameters["endpoint"] as string;

                var policy = metadata.Parameters["policy"] as IRetryPolicy;

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

                    var serializer = _factory.CreateMessageSerializer();

                    var content = serializer.Deserialize(context.ContentContext.Data, context.ContentContext.Type);

                    _logger.Log($"Message {context.Id}, sending the message to the retry endpoint {endpointname} (retry count {count}) by route {context.Name}");

                    await context.Send(content, endpointname, scheduledenqueuedatetimeutc: DateTime.UtcNow.Add(policy.NextRetryInterval(count)));

                    return metadata.StopAfterHandle;
                }
                else
                {
                    _logger.Log($"Message {context.Id}, no more retries by route {context.Name}");

                    if (metadata.Parameters.ContainsKey("fallback"))
                    {
                        var fallback = metadata.Parameters["fallback"] as Func<MessageContext, Exception, ErrorHandler, Task>;

                        _logger.Log($"Message {context.Id}, fallback executed by route {context.Name}");

                        await fallback(context, ex, metadata);

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