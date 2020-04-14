using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Options
    {
        public Options(string endpointname, Dictionary<string, string> headers, 
            SagaContext sagacontext, TrackingContext trackingcontext, TracingContext tracingcontext, Route route, Saga saga, string version, DateTime? scheduledenqueuedatetimeutc)
        {
            EndPointName = endpointname;
            Headers= headers;
            SagaContext = sagacontext;
            TrackingContext = trackingcontext;
            TracingContext = tracingcontext;
            Route = route;
            Saga = saga;
            Version = version;
            ScheduledEnqueueDateTimeUtc = scheduledenqueuedatetimeutc;
        }

        public TracingContext TracingContext { get; private set; }

        public SagaContext SagaContext { get; private set; }

        public Route Route { get; private set; }

        public Saga Saga { get; private set; }

        public string EndPointName { get; private set; }

        public string Version { get; private set; }

        public DateTime? ScheduledEnqueueDateTimeUtc { get; private set; }

        public IDictionary<string,string> Headers { get; private set; }

        public TrackingContext TrackingContext { get; private set; }
    }
}
