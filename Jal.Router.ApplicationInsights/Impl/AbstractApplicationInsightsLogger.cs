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
            properties.Add("version", context.Version);
            properties.Add("origin", context.Origin.Key);
            properties.Add("sagaid", context.SagaContext?.Id);
            properties.Add("replytorequestid", context.Identity.ReplyToRequestId);
            properties.Add("requestid", context.Identity.RequestId);
            properties.Add("contentid", context.ContentId);
            foreach (var h in context.Headers)
            {
                properties.Add(h.Key, h.Value);
            }
        }
    }
}