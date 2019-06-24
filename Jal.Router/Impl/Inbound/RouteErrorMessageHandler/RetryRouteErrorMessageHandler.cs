using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.RouteErrorMessageHandler
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
        public async Task<bool> OnException(MessageContext context, Exception ex, ErrorHandler metadata)
        {
            if(metadata.Parameters.ContainsKey("endpoint") && metadata.Parameters.ContainsKey("policy"))
            {
                var endpointname = metadata.Parameters["endpoint"] as string;

                var policy = metadata.Parameters["policy"] as IRetryPolicy;

                var countname = $"{context.Route.Name}_{policy.GetType().Name.ToLower()}_retrycount";

                var count = 0;

                if (context.Headers.ContainsKey(countname))
                {
                    count = Convert.ToInt32(context.Headers[countname]);
                }
                else
                {
                    context.Headers.Add(countname, count.ToString());
                }

                count++;

                if (policy.CanRetry(count))
                {
                    context.Headers[countname] = count.ToString();

                    var options = new Options(endpointname, context.CopyHeaders(), context.SagaContext, context.Tracks, context.IdentityContext, context.Route, context.Version)
                    {
                        ScheduledEnqueueDateTimeUtc = DateTime.UtcNow.Add(policy.NextRetryInterval(count)),
                    };

                    var serializer = _factory.CreateMessageSerializer();

                    var content = serializer.Deserialize(context.Content, context.ContentType);

                    _logger.Log($"Message {context.IdentityContext.Id}, sending the message to the retry endpoint {endpointname} (retry count {count}) by route {context.GetFullName()}");

                    await context.Send(content, context.Origin, options);

                    return metadata.StopAfterHandle;
                }
                else
                {
                    _logger.Log($"Message {context.IdentityContext.Id}, no more retries by route {context.GetFullName()}");

                    if (metadata.Parameters.ContainsKey("fallback"))
                    {
                        var fallback = metadata.Parameters["fallback"] as Func<MessageContext, Exception, ErrorHandler, Task>;

                        _logger.Log($"Message {context.IdentityContext.Id}, fallback executed by route {context.GetFullName()}");

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