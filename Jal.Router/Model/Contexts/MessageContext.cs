using Jal.Router.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class MessageContext
    {
        public IMessageSerializer MessageSerializer { get; private set; }
        public IEntityStorage EntityStorage { get; private set; }
        public IBus Bus { get; private set; }
        public Channel Channel { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }
        public DateTime DateTimeUtc { get; private set; }
        public string Host { get; private set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; private set; }
        public string Version { get; private set; }
        public Route Route { get; private set; }
        public EndPoint EndPoint { get; private set; }
        public Origin Origin { get; private set; }
        public Saga Saga { get; private set; }
        public SagaContext SagaContext { get; private set; }
        public ContentContext ContentContext { get; private set; }
        public TrackingContext TrackingContext { get; private set; }
        public TracingContext TracingContext { get; private set; }
        public string Id
        {
            get
            {
                return TracingContext?.Id;
            }
        }


        public bool FromSaga()
        {
            return Saga != null && SagaContext != null && SagaContext.Data != null && SagaContext.Data.Data != null;
        }

        public string Name
        {
            get
            {
                if (EndPoint != null)
                {
                    return Saga == null ? EndPoint.Name : Saga.Name + "/" + EndPoint.Name;
                }
                else
                {
                    return Saga == null ? Route?.Name : Saga.Name + "/" + Route?.Name;
                }
            }
        }

        public static MessageContext CreateFromListen(IBus bus, IComponentFactoryFacade factory)
        {
            return new MessageContext(bus, factory.CreateMessageSerializer(), factory.CreateEntityStorage(), null, null, null, null, DateTime.UtcNow, new List<Tracking>(), new Origin(), string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public static MessageContext CreateFromListen(IBus bus, IMessageSerializer serializer, IEntityStorage entitystorage)
        {
            return new MessageContext(bus, serializer, entitystorage, null, null, null, null, DateTime.UtcNow, new List<Tracking>(), new Origin(), string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public static MessageContext CreateFromListen(IBus bus, IMessageSerializer serializer, IEntityStorage entitystorage, Route route, 
            EndPoint endpoint, Channel channel, TracingContext tracingcontext, IList<Tracking> trackings, Origin origin, string content, string sagaid, string claimcheckid, DateTime datetimeutc, string version)
        {
            return new MessageContext(bus, serializer, entitystorage, route, endpoint, channel, tracingcontext, datetimeutc, trackings, origin, sagaid, version, claimcheckid, content);
        }

        public static MessageContext CreateToSend(IMessageSerializer serializer, IEntityStorage storage, EndPoint endpoint, Channel channel, Options options, Origin origin, object content, DateTime datetimeutc)
        {
            return new MessageContext(endpoint, channel, serializer, storage, options, datetimeutc, origin, serializer.Serialize(content));
        }

        private MessageContext(IBus bus, IMessageSerializer serializer, IEntityStorage storage, Route route, EndPoint endpoint, Channel channel, TracingContext tracingcontext, DateTime datetimeutc, IList<Tracking> tracks, Origin origin, string sagaid, string version, string claimcheckid, string content)
        {
            Bus = bus;

            MessageSerializer = serializer;
            EntityStorage = storage;
            Headers = new Dictionary<string, string>();
            Version = version;
            Origin = origin;
            SagaContext = new SagaContext(this, sagaid);
            TrackingContext = new TrackingContext(this, tracks);
            TracingContext = tracingcontext;
            DateTimeUtc = datetimeutc;
            ContentContext = new ContentContext(this, claimcheckid, channel?.UseClaimCheck ?? false, content);
            Route = route;
            Saga = route?.Saga;
            EndPoint = endpoint;
            Channel = channel;
            Host = Environment.MachineName;
        }

        private MessageContext(EndPoint endpoint, Channel channel, IMessageSerializer serializer, IEntityStorage storage, Options options, DateTime datetimeutc, Origin origin, string content)
        {
            MessageSerializer = serializer;
            EntityStorage = storage;
            Headers = options.Headers;
            Version = options.Version;
            Origin = origin;
            SagaContext = new SagaContext(this, options.SagaId, options.SagaData);
            TrackingContext = new TrackingContext(this, options.Trackings);
            TracingContext = options.TracingContext;
            DateTimeUtc = datetimeutc;
            ContentContext = new ContentContext(this, channel.UseClaimCheck ? Guid.NewGuid().ToString() : string.Empty, channel.UseClaimCheck, content);
            Route = options.Route;
            Saga = options.Saga;
            EndPoint = endpoint;
            Channel = channel;
            Host = Environment.MachineName;

            ScheduledEnqueueDateTimeUtc = options.ScheduledEnqueueDateTimeUtc;
        }

        public MessageEntity ToEntity()
        {
            return new MessageEntity(Host, Channel?.ToEntity(), Headers, Version, DateTimeUtc, ScheduledEnqueueDateTimeUtc, Route?.ToEntity(),
                EndPoint?.ToEntity(), Origin?.ToEntity(), Saga?.ToEntity(), ContentContext?.ToEntity(), SagaContext?.ToEntity(),
                TrackingContext?.ToEntity(), TracingContext?.ToEntity(), Name);
        }

        public async Task<MessageEntity> CreateAndInsertMessageIntoStorage(MessageEntity entity)
        {
            var id = await EntityStorage.Insert(entity, MessageSerializer).ConfigureAwait(false);

            entity.SetId(id);

            return entity;
        }

        public Dictionary<string, string> CloneHeaders()
        {
            return Headers.ToDictionary(header => header.Key, header => header.Value);
        }

        public Task<SagaData> GetSagaDataFromStorage(string id)
        {
            return EntityStorage.Get(id, MessageSerializer);
        }

        public Task UpdateSagaDataIntoStorage(SagaData sagadata)
        {
            return EntityStorage.Update(sagadata, MessageSerializer);
        }

        public Task<string> InsertSagaDataIntoStorage(SagaData sagadata)
        {
            return EntityStorage.Insert(sagadata, MessageSerializer);
        }

        public object Deserialize(string content, Type type)
        {
            return MessageSerializer.Deserialize(content, type);
        }

        public string Deserialize(object content)
        {
            return MessageSerializer.Serialize(content);
        }

        public Task Send<TContent>(TContent content, Options options)
        {
            return Bus.Send(content, options);
        }

        public Task Send<TContent>(TContent content, Origin origin, Options options)
        {
            return Bus.Send(content, origin, options);
        }

        public Task Send<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return Bus.Send(content, endpoint, origin, options);
        }

        public Task Publish<TContent>(TContent content, Options options)
        {
            return Bus.Publish(content, options);
        }

        public Task Publish<TContent>(TContent content, Origin origin, Options options)
        {
            return Bus.Publish(content, origin, options);
        }

        public Task Publish<TContent>(TContent content, EndPoint endpoint, Origin origin, Options options)
        {
            return Bus.Publish(content, endpoint, origin, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Options options) where TResult : class
        {
            return Bus.Reply<TContent, TResult>(content, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, Origin origin, Options options) where TResult : class
        {
            return Bus.Reply<TContent, TResult>(content, origin, options);
        }

        public Task<TResult> Reply<TContent, TResult>(TContent content, EndPoint endpoint, Origin origin, Options options) where TResult : class
        {
            return Bus.Reply<TContent, TResult>(content, endpoint, origin, options);
        }
    }
}