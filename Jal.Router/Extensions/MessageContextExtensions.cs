using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static Task<TResult> Reply<TContent, TResult>(this MessageContext context, TContent content, string endpointname, string id = null, string sagaid = null, Dictionary<string, string> headers = null, string version = null) where TResult : class
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            var options = context.CreateOptions(endpointname, id, sagaid, headers);

            options.TracingContext.SetReplyToRequestId(Guid.NewGuid().ToString());

            return context.Reply<TContent, TResult>(content, options);
        }

        public static Task<TResult> Reply<TContent, TResult>(this MessageContext context, TContent content, string endpointname, TracingContext tracingcontext, string sagaid = null, Dictionary<string, string> headers = null, string version = null) where TResult : class
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            var options = context.CreateOptions(endpointname, tracingcontext, sagaid, headers, version);

            options.TracingContext.SetReplyToRequestId(Guid.NewGuid().ToString());

            return context.Reply<TContent, TResult>(content, options);
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, EndPoint endpoint, TracingContext tracingcontext, string sagaid = null, Dictionary<string,string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            return context.Send(content, endpoint, context.CreateOrigin(), context.CreateOptions(string.Empty, tracingcontext, sagaid, headers, version, scheduledenqueuedatetimeutc));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, EndPoint endpoint, string id=null, string sagaid = null, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            return context.Send(content, endpoint, context.CreateOrigin(), context.CreateOptions(string.Empty, id, sagaid, headers, version, scheduledenqueuedatetimeutc));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, string endpointname, TracingContext tracingcontext, string sagaid = null, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.Send(content, context.CreateOptions(endpointname, tracingcontext, sagaid, headers, version, scheduledenqueuedatetimeutc));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, string endpointname, string id=null, string sagaid=null, Dictionary<string, string> headers = null, string version=null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.Send(content, context.CreateOptions(endpointname, id, sagaid, headers, version, scheduledenqueuedatetimeutc));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, string endpointname, TracingContext tracingcontext, string sagaid=null, string key=null, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, tracingcontext, sagaid, headers, version, scheduledenqueuedatetimeutc));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, EndPoint endpoint, TracingContext tracingcontext, string sagaid = null, string key = null, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            return context.Publish(content, endpoint, context.CreateOrigin(key), context.CreateOptions(string.Empty, tracingcontext, sagaid, headers, version, scheduledenqueuedatetimeutc));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, string sagaid = null, string key = null, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, id, sagaid, headers, version, scheduledenqueuedatetimeutc));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, EndPoint endpoint, string id = null, string sagaid = null, string key = null, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            return context.Publish(content, endpoint, context.CreateOrigin(key), context.CreateOptions(string.Empty, id, sagaid, headers, version, scheduledenqueuedatetimeutc));
        }

        public static Origin CreateOrigin(this MessageContext context, string key = null)
        {
            return new Origin() { Key = key};
        }

        private static Options CreateOptions(this MessageContext context, string endpointname, IDictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc=null)
        {
            var tracingcontext = context.TracingContext.Clone();

            if (string.IsNullOrWhiteSpace(tracingcontext.Id))
            {
                throw new ArgumentNullException(tracingcontext.Id);
            }

            if(!string.IsNullOrWhiteSpace(tracingcontext.ParentId))
            {
                tracingcontext.SetParentId(tracingcontext.Id);
            }

            if (!string.IsNullOrWhiteSpace(tracingcontext.OperationId))
            {
                tracingcontext.SetOperationId(tracingcontext.Id);
            }

            if (string.IsNullOrEmpty(version))
            {
                version = context.Version;
            }

            var options = new Options(endpointname, context.CreateCopyOfHeaders(), context.SagaContext, context.TrackingContext, tracingcontext, context.Route, context.Saga, version, scheduledenqueuedatetimeutc);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (headers.ContainsKey(header.Key))
                    {
                        options.Headers[header.Key] = header.Value;
                    }
                    else
                    {
                        options.Headers.Add(header);
                    }
                }
            }

            return options;
        }

        private static Options CreateOptions(this MessageContext context, string endpointname, TracingContext tracingcontext, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {

            if (string.IsNullOrWhiteSpace(tracingcontext.Id))
            {
                throw new ArgumentNullException(tracingcontext.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.TracingContext.ReplyToRequestId))
            {
                tracingcontext.SetRequestId(context.TracingContext.ReplyToRequestId);
            }

            if (string.IsNullOrEmpty(version))
            {
                version = context.Version;
            }

            var options = new Options(endpointname, context.CreateCopyOfHeaders(), context.SagaContext, context.TrackingContext, tracingcontext, context.Route, context.Saga, version, scheduledenqueuedatetimeutc);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if(headers.ContainsKey(header.Key))
                    {
                        options.Headers[header.Key]= header.Value;
                    }
                    else
                    {
                        options.Headers.Add(header);
                    }
                }
            }

            return options;
        }

        public static Options CreateOptions(this MessageContext context, string endpointname, TracingContext indentitycontext, string sagaid=null, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            var options = CreateOptions(context, endpointname, indentitycontext, headers, version, scheduledenqueuedatetimeutc);

            if (!string.IsNullOrEmpty(sagaid))
            {
                options.SagaContext.SetId(sagaid);
            }

            return options;
        }

        public static Options CreateOptions(this MessageContext context, string endpointname, string id=null, string sagaid = null, Dictionary<string, string> headers = null, string version = null, DateTime? scheduledenqueuedatetimeutc = null)
        {
            var options = CreateOptions(context, endpointname, headers, version, scheduledenqueuedatetimeutc);

            if(!string.IsNullOrEmpty(sagaid))
            {
                options.SagaContext.SetId(sagaid);
            }

            if (!string.IsNullOrEmpty(id))
            {
                options.TracingContext.SetId(id);
            }

            return options;
        }
    }
}