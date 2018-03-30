using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Outbound;

namespace Jal.Router.Model
{
    public class MessageContext
    {
        private readonly IBus _bus;

        private readonly IMessageSerializer _serializer;

        private readonly IStorage _storage;
        public string EndPointName { get; set; }
        public string ToConnectionString { get; set; }
        public string ToSubscription { get; set; }
        public string ToPath { get; set; }
        public string Id { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Version { get; set; }
        public int RetryCount { get; set; }
        public bool LastRetry { get; set; }
        public Route Route { get; set; }     
        public Origin Origin { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public SagaContext SagaContext { get; set; }
        public Saga Saga { get; set; }
        public DateTime? ScheduledEnqueueDateTimeUtc { get; set; }
        public Type ContentType { get; set; }
        public Type ResultType { get; set; }
        public string ContentAsString { get; set; }
        public string ToReplyConnectionString { get; set; }
        public string ToReplyPath { get; set; }
        public int ToReplyTimeOut { get; set; }
        public string ToReplySubscription { get; set; }
        public string ReplyToRequestId { get; set; }
        public string RequestId { get; set; }
        public List<Track> Tracks { get; set; }
        public MessageContext(IBus bus, IMessageSerializer serializer, IStorage storage)
        {
            _bus = bus;
            _serializer = serializer;
            _storage = storage;
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            SagaContext = new SagaContext();
            Tracks = new List<Track>();
        }

        public MessageContext()
        {
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            SagaContext = new SagaContext();
            Tracks = new List<Track>();
        }

        public void AddTrack(string id, string key, string from, string route)
        {
            var tracking = new Track()
            {
                Id = id,
                Key = key,
                From = from,
                Route = route
            };

            Tracks.Add(tracking);
        }

        public void AddTrack(string id, string key, string from, string route, string sagaid, string saga)
        {
            var tracking = new Track()
            {
                Id = id,
                Key = key,
                From = from,
                SagaId = sagaid,
                Route = route,
                Saga = saga
            };

            Tracks.Add(tracking);
        }

        public Track[] GetTracksOfTheSaga()
        {
            if (!string.IsNullOrWhiteSpace(SagaContext.Id))
            {
                return Tracks.Where(x => x.SagaId == SagaContext.Id).ToArray();
            }

            return new Track[] {};
        }

        public Track GetCurrentTrack()
        {
            if (Tracks.Count > 0)
            {
                return Tracks[Tracks.Count - 1];
            }
            return null;
        }

        public Track GetTrackOfTheCaller()
        {
            if (Tracks.Count > 1)
            {
                return Tracks[Tracks.Count - 2];
            }
            return null;
        }

        public Track GetTrackOfTheSagaCaller()
        {
            if (!string.IsNullOrWhiteSpace(SagaContext.Id))
            {
                var index = -1;

                for (var i = 0; i < Tracks.Count; i++)
                {
                    if (Tracks[i].SagaId == SagaContext.Id)
                    {
                        index = i - 1;

                        break;
                    }
                }

                if (index >= 0)
                {
                    return Tracks[index];
                }
            }

            return null;
        }

        private void Update(object data)
        {
            if (_storage != null && _serializer != null)
            {
                var sagaentity = _storage.GetSaga(SagaContext.Id);

                if (sagaentity != null)
                {
                    sagaentity.Data = _serializer.Serialize(data);

                    sagaentity.Updated = DateTimeUtc;

                    sagaentity.Status = SagaContext.Status;

                    _storage.UpdateSaga(this, SagaContext.Id, sagaentity);
                }
            }
        }

        public Dictionary<string, string> CopyHeaders()
        {
            return Headers.ToDictionary(header => header.Key, header => header.Value);
        }

        public void FireAndForget<TContent>(TContent content, Options options)
        {
            _bus.FireAndForget(content, options);
        }

        public void FireAndForget<TContent>(TContent content, Origin origin, Options options)
        {
            _bus.FireAndForget(content, Origin, options);
        }

        public void Send<TContent>(TContent content, Options options)
        {
            _bus.Send(content, options);
        }

        public void Send<TContent, TData>(TData data, TContent content, Options options)
        {
            Update(data);

            _bus.Send(content, options);
        }

        public void Send<TContent>(TContent content, Origin origin, Options options)
        {
            _bus.Send(content, origin, options);
        }

        public void Send<TContent>(TContent content, EndPointSetting endpointsetting, Origin origin, Options options)
        {
            _bus.Send(content, endpointsetting, origin, options);
        }

        public void Send<TContent, TData>(TData data, TContent content, Origin origin, Options options)
        {
            Update(data);

            _bus.Send(content, origin, options);
        }

        public void Publish<TContent>(TContent content, Options options)
        {
            _bus.Publish(content, options);
        }

        public void Publish<TContent, TData>(TData data, TContent content, Options options)
        {
            Update(data);

            _bus.Publish(content, options);
        }

        public void Publish<TContent>(TContent content, Origin origin, Options options)
        {
            _bus.Publish(content, origin, options);
        }

        public void Publish<TContent>(TContent content, EndPointSetting endpointsetting, Origin origin, Options options)
        {
            _bus.Publish(content, endpointsetting, origin, options);
        }

        public void Publish<TContent, TData>(TData data, TContent content, Origin origin, Options options)
        {
            Update(data);

            _bus.Publish(content, origin, options);
        }

        public TResult Reply<TContent, TResult>(TContent content, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, options);
        }

        public TResult Reply<TContent, TResult>(TContent content, Origin origin, Options options)
        {
            return _bus.Reply<TContent, TResult>(content, origin, options);
        }

        public TResult Reply<TContent, TResult, TData>(TData data, TContent content, Options options)
        {
            Update(data);

            return _bus.Reply<TContent, TResult>(content, options);
        }

        public TResult Reply<TContent, TResult, TData>(TData data, TContent content, Origin origin, Options options)
        {
            Update(data);

            return _bus.Reply<TContent, TResult>(content, origin, options);
        }
    }
}