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
            telemetrycontext.Operation.Id = $"{context.Identity.OperationId}";
            telemetrycontext.Operation.ParentId = $"{context.Identity.ParentId}";

            if (!string.IsNullOrWhiteSpace(Configuration.ApplicationName))
            {
                telemetrycontext.Cloud.RoleName = Configuration.ApplicationName;
            }
        }

        public void PopulateMetrics(IDictionary<string, double> metrics, MessageContext context)
        {
            metrics.Add("retrycount", context.RetryCount);
        }

        public void PopulateProperties(IDictionary<string, string> properties, MessageContext context)
        {
            properties.Add("identity_id", context.Identity?.Id);
            properties.Add("identity_replytorequestid", context.Identity?.ReplyToRequestId);
            properties.Add("identity_requestid", context.Identity?.RequestId);
            properties.Add("identity_operationid", context.Identity?.OperationId);
            properties.Add("identity_parentid", context.Identity?.ParentId);

            properties.Add("origin_key", context.Origin?.Key);
            properties.Add("origin_from", context.Origin?.From);

            properties.Add("saga_id", context.SagaContext?.Id);
            properties.Add("saga_status", context.SagaContext?.Status);
            properties.Add("saga_name", context.Saga?.Name);

            properties.Add("version", context.Version);
            properties.Add("contentid", context.ContentId);
            properties.Add("lastretry", context.LastRetry.ToString());

            properties.Add("route_name", context.Route?.Name);

            foreach (var h in context.Headers)
            {
                properties.Add(h.Key, h.Value);
            }
        }
    }
}