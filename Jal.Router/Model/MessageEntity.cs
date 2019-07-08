using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class MessageEntity
    {
        public string Id { get; set; }
        public string Name { get; }
        public ChannelEntity ChannelEntity { get; }
        public IDictionary<string, string> Headers { get; }
        public string Version { get; }
        public DateTime DateTimeUtc { get; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; }
        public RouteEntity RouteEntity { get; }
        public EndpointEntity EndpointEntity { get; }
        public OriginEntity OriginEntity { get; }
        public SagaEntity SagaEntity { get; }
        public ContentContextEntity ContentContextEntity { get; }
        public SagaContextEntity SagaContextEntity { get; }
        public TrackingContextEntity TrackingContextEntity { get;}
        public IdentityContextEntity IdentityContextEntity { get; }

        public MessageEntity(ChannelEntity channelentity, IDictionary<string, string> headers, string version, DateTime datetimeutc,
            DateTime? scheduledenqueuedatetimeutc, RouteEntity routeentity, EndpointEntity endpointentity, OriginEntity originentity,
            SagaEntity sagaentity, ContentContextEntity contentcontextentity, SagaContextEntity sagacontextentity,
            TrackingContextEntity trackingcontextentity, IdentityContextEntity identitycontextentity, string name)
        {
            ChannelEntity = channelentity;
            Headers = headers;
            Version = version;
            DateTimeUtc = datetimeutc;
            ScheduledEnqueueDateTimeUtc = scheduledenqueuedatetimeutc;
            RouteEntity = routeentity;
            EndpointEntity = endpointentity;
            OriginEntity = originentity;
            SagaEntity = sagaentity;
            ContentContextEntity = contentcontextentity;
            SagaContextEntity = sagacontextentity;
            TrackingContextEntity = trackingcontextentity;
            IdentityContextEntity = identitycontextentity;
        }
    }
}
