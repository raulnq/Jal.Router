using System;
using System.Collections.Generic;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static void Send<TContent>(this MessageContext context, TContent content, string endpointname, string id, string sagaid,  Dictionary<string,string> headers = null)
        {
            context.Send(content, context.CreateOrigin(), context.CreateOptions(endpointname, id, sagaid, headers));
        }
        public static void Send<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id, string sagaid, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, context.CreateOrigin(), context.CreateOptions(endpointname, id, sagaid, headers));
        }
        public static void Send<TContent>(this MessageContext context, TContent content, string endpointname, string id, Dictionary<string, string> headers = null)
        {
            context.Send(content, context.CreateOptions(endpointname, id, headers));
        }
        public static void Send<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, context.CreateOptions(endpointname, id, headers));
        }

        public static void PublishWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id, string sagaid, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname,  id, sagaid, headers));
        }
        public static void PublishWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id, string sagaid, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOrigin(key), context.CreateOptions(endpointname, id, sagaid, headers));
        }
        public static void Publish<TContent>(this MessageContext context, TContent content, string endpointname, string id, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, id, headers));
        }
        public static void Publish<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOrigin(key), context.CreateOptions(endpointname, id, headers));
        }

        public static Origin CreateOrigin(this MessageContext context, string key = null)
        {
            return new Origin() { Key = key};
        }
        public static Options CreateOptions(this MessageContext context, string endpointname, string id, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(id);
            }
            var options = new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? context.Id : id,
                Tracks = context.Tracks
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    options.Headers.Add(header);
                }
            }

            return options;
        }
        public static Options CreateOptions(this MessageContext context, string endpointname, string id, string sagaid, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(id);
            }
            if (string.IsNullOrWhiteSpace(sagaid))
            {
                throw new ArgumentNullException(sagaid);
            }

            var options = new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = id,
                SagaInfo = new SagaInfo() { Id = sagaid },
                Tracks = context.Tracks
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    options.Headers.Add(header);
                }
            }

            return options;
        }
    }
}