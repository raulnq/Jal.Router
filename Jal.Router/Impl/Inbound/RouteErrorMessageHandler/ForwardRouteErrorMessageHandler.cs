using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ForwardRouteErrorMessageHandler : IRouteErrorMessageHandler
    {
        private readonly IComponentFactoryGateway _factory;

        private readonly ILogger _logger;

        public ForwardRouteErrorMessageHandler(IComponentFactoryGateway factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }
        public async Task<bool> OnException(MessageContext context, Exception ex, ErrorHandler metadata)
        {
            if(metadata.Parameters.ContainsKey("endpoint"))
            {
                var endpointname = metadata.Parameters["endpoint"] as string;

                var options = new Options(endpointname, context.CreateCopyOfHeaders(), context.SagaContext, context.TrackingContext, context.IdentityContext, context.Route, context.Saga, context.Version);

                if (ex != null)
                {
                    options.Headers.Remove("exceptionmessage");

                    options.Headers.Remove("exceptionstacktrace");

                    options.Headers.Remove("innerexceptionmessage");

                    options.Headers.Remove("innerexceptionstacktrace");

                    options.Headers["exceptionmessage"] = ex.Message;

                    options.Headers["exceptionstacktrace"] = ex.StackTrace;

                    if (ex.InnerException != null)
                    {
                        options.Headers["innerexceptionmessage"] = ex.InnerException.Message;

                        options.Headers["innerexceptionstacktrace"] = ex.InnerException.StackTrace;
                    }
                }

                var serializer = _factory.CreateMessageSerializer();

                var content = serializer.Deserialize(context.ContentContext.Data, context.ContentContext.Type);

                _logger.Log($"Message {context.Id}, sending the message to the error endpoint {endpointname} by route {context.Name}");

                await context.Send(content, context.Origin, options);

                return metadata.StopAfterHandle;
            }

            return false;
        }
    }
}