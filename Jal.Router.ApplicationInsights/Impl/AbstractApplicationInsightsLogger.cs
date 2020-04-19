using System.Collections.Generic;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Jal.Router.ApplicationInsights
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
            telemetrycontext.Operation.Id = $"{context.TracingContext.OperationId}";
            telemetrycontext.Operation.ParentId = $"{context.TracingContext.ParentId}";

            if (!string.IsNullOrWhiteSpace(Configuration.ApplicationName))
            {
                telemetrycontext.Cloud.RoleName = Configuration.ApplicationName;
            }
        }

        public void PopulateProperties(IDictionary<string, string> properties, MessageContext context)
        {
            properties.Add("tracing_id", context?.Id);
            properties.Add("tracing_replytorequestid", context.TracingContext?.ReplyToRequestId);
            properties.Add("tracing_requestid", context.TracingContext?.RequestId);
            properties.Add("tracing_operationid", context.TracingContext?.OperationId);
            properties.Add("tracing_parentid", context.TracingContext?.ParentId);
            properties.Add("tracing_partitionid", context.TracingContext?.PartitionId);

            properties.Add("origin_key", context.Origin?.Key);
            properties.Add("origin_from", context.Origin?.From);

            properties.Add("saga_id", context.SagaContext?.Data?.Id);
            properties.Add("saga_status", context.SagaContext?.Data?.Status);
            properties.Add("saga_name", context.Saga?.Name);

            properties.Add("version", context.Version);
            properties.Add("contentid", context.ContentContext.ClaimCheckId);

            properties.Add("route_name", context.Route?.Name);
            properties.Add("endpoint_name", context.EndPoint?.Name);

            foreach (var h in context.Headers)
            {
                properties.Add(h.Key, h.Value);
            }
        }
    }
}