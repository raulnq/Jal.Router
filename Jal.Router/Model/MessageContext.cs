using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Outbound;

namespace Jal.Router.Model
{
    public class MessageContext
    {
        private readonly IBus _bus;

        private readonly IStorageFacade _facade;
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
        public SagaInfo SagaInfo { get; set; }
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
        public object Content { get; set; }
        public List<Track> Tracks { get; set; }
        public MessageContext(IBus bus, IStorageFacade facade)
        {
            _bus = bus;
            _facade = facade;
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            SagaInfo = new SagaInfo();
            Tracks = new List<Track>();
        }

        public MessageContext()
        {
            Headers = new Dictionary<string, string>();
            Version = "1";
            LastRetry = true;
            Origin = new Origin();
            SagaInfo = new SagaInfo();
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

        public Track GetStartSaga()
        {
            if (!string.IsNullOrWhiteSpace(SagaInfo.Id))
            {
                return Tracks.FirstOrDefault(x => x.Id == SagaInfo.Id);
            }

            return null;
        }

        public Track GetInvoker()
        {
            if (Tracks.Count > 1)
            {
                return Tracks[Tracks.Count - 2];
            }
            return null;
        }

        public Track GetSagaInvoker()
        {
            if (!string.IsNullOrWhiteSpace(SagaInfo.Id))
            {
                var index = -1;

                for (var i = 0; i < Tracks.Count; i++)
                {
                    if (Tracks[i].SagaId == SagaInfo.Id)
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
            _facade.UpdateSaga(this, data);

            _bus.Send(content, options);
        }

        public void Send<TContent>(TContent content, Origin origin, Options options)
        {
            _bus.Send(content, origin, options);
        }

        public void Send<TContent, TData>(TData data, TContent content, Origin origin, Options options)
        {
            _facade.UpdateSaga(this, data);

            _bus.Send(content, origin, options);
        }

        public void Publish<TContent>(TContent content, Options options)
        {
            _bus.Publish(content, options);
        }

        public void Publish<TContent, TData>(TData data, TContent content, Options options)
        {
            _facade.UpdateSaga(this, data);

            _bus.Publish(content, options);
        }

        public void Publish<TContent>(TContent content, Origin origin, Options options)
        {
            _bus.Publish(content, origin, options);
        }


        public void Publish<TContent, TData>(TData data, TContent content, Origin origin, Options options)
        {
            _facade.UpdateSaga(this, data);

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
            _facade.UpdateSaga(this, data);

            return _bus.Reply<TContent, TResult>(content, options);
        }

        public TResult Reply<TContent, TResult, TData>(TData data, TContent content, Origin origin, Options options)
        {
            _facade.UpdateSaga(this, data);

            return _bus.Reply<TContent, TResult>(content, origin, options);
        }
    }
}