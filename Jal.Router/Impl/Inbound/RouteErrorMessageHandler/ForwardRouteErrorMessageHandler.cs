﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Extensions;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class ForwardRouteErrorMessageHandler : IRouteErrorMessageHandler
    {
        private readonly IComponentFactoryFacade _factory;

        private readonly ILogger _logger;

        public ForwardRouteErrorMessageHandler(IComponentFactoryFacade factory, ILogger logger)
        {
            _factory = factory;
            _logger = logger;
        }
        public async Task<bool> Handle(MessageContext context, Exception ex, ErrorHandler metadata)
        {
            if(metadata.Parameters.ContainsKey("endpoint") && metadata.Parameters["endpoint"] is string endpointname && !string.IsNullOrEmpty(endpointname) && ex!=null)
            {
                var headers = new Dictionary<string, string>
                {
                    { "exceptionmessage", ex.Message },

                    { "exceptionstacktrace", ex.StackTrace }
                };

                if (ex.InnerException != null)
                {
                    headers.Add("innerexceptionmessage", ex.InnerException.Message);

                    headers.Add("innerexceptionstacktrace", ex.InnerException.StackTrace);
                }

                var serializer = _factory.CreateMessageSerializer();

                var content = serializer.Deserialize(context.ContentContext.Data, context.ContentContext.Type);

                _logger.Log($"Message {context.Id}, sending the message to the error endpoint {endpointname} by route {context.Name}");

                await context.Send(content, endpointname, headers: headers).ConfigureAwait(false);

                return metadata.StopAfterHandle;
            }

            return false;
        }
    }
}