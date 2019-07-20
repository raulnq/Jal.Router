using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class MessageEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public ChannelEntity ChannelEntity { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }
        public string Version { get; private set; }
        public DateTime DateTimeUtc { get; private set; }
        public string Host { get; set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; private set; }
        public RouteEntity RouteEntity { get; private set; }
        public EndpointEntity EndpointEntity { get; private set; }
        public OriginEntity OriginEntity { get; private set; }
        public SagaEntity SagaEntity { get; private set; }
        public ContentContextEntity ContentContextEntity { get; private set; }
        public SagaContextEntity SagaContextEntity { get; private set; }
        public TrackingContextEntity TrackingContextEntity { get; private set; }
        public IdentityContextEntity IdentityContextEntity { get; private set; }

        public MessageEntity(string host, ChannelEntity channelentity, IDictionary<string, string> headers, string version, DateTime datetimeutc,
            DateTime? scheduledenqueuedatetimeutc, RouteEntity routeentity, EndpointEntity endpointentity, OriginEntity originentity,
            SagaEntity sagaentity, ContentContextEntity contentcontextentity, SagaContextEntity sagacontextentity,
            TrackingContextEntity trackingcontextentity, IdentityContextEntity identitycontextentity, string name)
        {
            Host = host;
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
            Name = name;
        }

        public MessageEntity(string id, string host, ChannelEntity channelentity, IDictionary<string, string> headers, string version, 
            DateTime datetimeutc,
            DateTime? scheduledenqueuedatetimeutc, RouteEntity routeentity, EndpointEntity endpointentity, 
            OriginEntity originentity,
            SagaEntity sagaentity, ContentContextEntity contentcontextentity, SagaContextEntity sagacontextentity,
            TrackingContextEntity trackingcontextentity, IdentityContextEntity identitycontextentity, string name)
            :this(host, channelentity, headers, version, datetimeutc, scheduledenqueuedatetimeutc, routeentity, endpointentity,
                 originentity, sagaentity, contentcontextentity, sagacontextentity, trackingcontextentity, identitycontextentity,
                 name)
        {
            Id = id;
        }

        public void UpdateId(string id)
        {
            Id = id;
        }
    }
}
