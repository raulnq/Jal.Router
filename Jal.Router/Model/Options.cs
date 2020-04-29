using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Options
    {
        public static Options Create(string endpointname="endpoint", string id=null, string version=null)
        {
            return new Options(endpointname,
            new Dictionary<string, string>(), null, string.Empty,
            new List<Tracking>(),
            new TracingContext(id??Guid.NewGuid().ToString()),
            null,
            null,
            version,
            null);
        }

        public static Options CreateFromMessageContext(MessageContext context, string endpointname, TracingContext tracingcontext, string sagaid, string version, DateTime? scheduledenqueuedatetimeutc)
        {
            if(string.IsNullOrWhiteSpace(sagaid))
            {
                return new Options(endpointname, context.CloneHeaders(), context.SagaContext?.Data, context.SagaContext?.Id, context.TrackingContext?.Trackings, tracingcontext, context.Route, context.Saga, version, scheduledenqueuedatetimeutc);
            }
            else
            {
                return new Options(endpointname, context.CloneHeaders(), null, sagaid, context.TrackingContext?.Trackings, tracingcontext, context.Route, context.Saga, version, scheduledenqueuedatetimeutc);
            }
        }

        private Options(string endpointname, Dictionary<string, string> headers, 
            SagaData sagadata, string sagaid, List<Tracking> trackings, TracingContext tracingcontext, Route route, Saga saga, string version, DateTime? scheduledenqueuedatetimeutc)
        {
            EndPointName = endpointname;
            Headers= headers;
            SagaData = sagadata;
            SagaId = sagaid;
            Trackings = trackings;
            TracingContext = tracingcontext;
            Route = route;
            Saga = saga;
            Version = version;
            ScheduledEnqueueDateTimeUtc = scheduledenqueuedatetimeutc;
        }

        public TracingContext TracingContext { get; private set; }

        public SagaData SagaData { get; private set; }

        public string SagaId { get; private set; }

        public Route Route { get; private set; }

        public Saga Saga { get; private set; }

        public string EndPointName { get; private set; }

        public string Version { get; private set; }

        public DateTime? ScheduledEnqueueDateTimeUtc { get; private set; }

        public IDictionary<string,string> Headers { get; private set; }

        public List<Tracking> Trackings { get; private set; }
    }
}
