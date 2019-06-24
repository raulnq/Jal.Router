using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Options
    {
        public Options(string endpointname, Dictionary<string, string> headers, 
            SagaContext sagacontext, List<Track> tracks, IdentityContext identitycontext, Route route, string version)
        {
            EndPointName = endpointname;
            Headers= headers;
            SagaContext = sagacontext;
            Tracks = tracks;
            IdentityContext = identitycontext;
            Route = route;
        }
        public IdentityContext IdentityContext { get; }

        public SagaContext SagaContext { get; }

        public Route Route { get; }

        public string EndPointName { get; }

        public string Version { get; }

        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }

        public IDictionary<string,string> Headers { get; }

        public List<Track> Tracks { get; }
    }
}
