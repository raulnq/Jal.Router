using System.Collections.Generic;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights.Impl
{
    public abstract class AbstractApplicationInsightsLogger
    {
        protected readonly TelemetryClient Client;

        protected readonly IConfiguration Configuration;

        protected AbstractApplicationInsightsLogger(TelemetryClient client, IConfiguration configuration)
        {
            Client = client;
            Configuration = configuration;
        }

        public void PopulateContext(TelemetryContext telemetrycontext, MessageContext context)
        {
            telemetrycontext.Operation.Id = $"{context.IdentityContext.OperationId}";
            telemetrycontext.Operation.ParentId = $"{context.IdentityContext.ParentId}";

            if (!string.IsNullOrWhiteSpace(Configuration.ApplicationName))
            {
                telemetrycontext.Cloud.RoleName = Configuration.ApplicationName;
            }
        }

        public void PopulateProperties(IDictionary<string, string> properties, MessageContext context)
        {
            properties.Add("identity_id", context.IdentityContext?.Id);
            properties.Add("identity_replytorequestid", context.IdentityContext?.ReplyToRequestId);
            properties.Add("identity_requestid", context.IdentityContext?.RequestId);
            properties.Add("identity_operationid", context.IdentityContext?.OperationId);
            properties.Add("identity_parentid", context.IdentityContext?.ParentId);

            properties.Add("origin_key", context.Origin?.Key);
            properties.Add("origin_from", context.Origin?.From);

            properties.Add("saga_id", context.SagaContext?.Id);
            properties.Add("saga_status", context.SagaContext?.Status);
            properties.Add("saga_name", context.Saga?.Name);

            properties.Add("version", context.Version);
            properties.Add("contentid", context.ContentId);

            properties.Add("route_name", context.Route?.Name);
            properties.Add("endpoint_name", context.EndPoint?.Name);

            foreach (var h in context.Headers)
            {
                properties.Add(h.Key, h.Value);
            }
        }
    }
}